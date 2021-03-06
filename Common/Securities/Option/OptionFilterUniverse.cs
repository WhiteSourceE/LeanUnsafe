/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/

using System;
using System.Collections.Generic;
using QuantConnect.Data;
using System.Linq;
using System.Collections;
using QuantConnect.Util;
using QuantConnect.Securities.Option;

namespace QuantConnect.Securities
{
    /// <summary>
    /// Represents options symbols universe used in filtering.
    /// </summary> 
    public class OptionFilterUniverse : IDerivativeSecurityFilterUniverse
    {
        /// <summary>
        /// Defines listed option types
        /// </summary>
        public enum Type : int
        {
            /// <summary>
            /// Listed stock options that expire 3rd Friday of the month
            /// </summary>
            Standard = 1,

            /// <summary>
            /// Weeklys options that expire every week
            /// These are options listed with approximately one week to expiration
            /// </summary>
            Weeklys = 2
        }

        internal IEnumerable<Symbol> _allSymbols;

        /// <summary>
        /// The underlying price data
        /// </summary>
        public BaseData Underlying
        {
            get
            {
                // underlying value changes over time, so accessing it makes universe dynamic
                _isDynamic = true;
                return _underlying;
            }
        }

        internal BaseData _underlying;

        /// <summary>
        /// True if the universe is dynamic and filter needs to be reapplied
        /// </summary>
        public bool IsDynamic
        {
            get
            {
                return _isDynamic;
            }
        }

        internal bool _isDynamic;
        internal Type _type = Type.Standard;

        // Fields used in relative strikes filter
        private decimal _strikeSize;
        private DateTime _strikeSizeResolveDate;

        /// <summary>
        /// Constructs OptionFilterUniverse
        /// </summary>
        public OptionFilterUniverse(IEnumerable<Symbol> allSymbols, BaseData underlying)
        {
            _allSymbols = allSymbols;
            _underlying = underlying;
            _type = Type.Standard;
            _isDynamic = false;
        }

        /// <summary>
        /// Includes universe of weeklys options (if any) into selection
        /// </summary>
        /// <param name="universe"></param>
        /// <returns></returns>
        public OptionFilterUniverse IncludeWeeklys()
        {
            _type |= Type.Weeklys;
            return this;
        }

        /// <summary>
        /// Sets universe of weeklys options (if any) as a selection
        /// </summary>
        /// <param name="universe"></param>
        /// <returns></returns>
        public OptionFilterUniverse WeeklysOnly()
        {
            _type = Type.Weeklys;
            return this;
        }


        /// <summary>
        /// Returns universe, filtered by option type
        /// </summary>
        /// <returns></returns>
        internal OptionFilterUniverse ApplyOptionTypesFilter()
        {
            // memoization map for ApplyOptionTypesFilter()
            var memoizedMap = new Dictionary<DateTime, bool>();

            Func<Symbol, bool> memoizedIsStandardType = symbol =>
            {
                var dt = symbol.ID.Date;

                if (memoizedMap.ContainsKey(dt))
                    return memoizedMap[dt];
                var res = OptionSymbol.IsStandardContract(symbol);
                memoizedMap[dt] = res;

                return res;
            };

            var filtered = _allSymbols.Where(x =>
            {
                switch (_type)
                {
                    case Type.Weeklys:
                        return !memoizedIsStandardType(x);
                    case Type.Standard:
                        return memoizedIsStandardType(x);
                    case Type.Standard | Type.Weeklys:
                        return true;
                    default:
                        return false;
                }
            });

            _allSymbols = filtered.ToList();
            return this;
        }

        /// <summary>
        /// Returns front month contract
        /// </summary>
        /// <param name="universe"></param>
        /// <returns></returns>
        public OptionFilterUniverse FrontMonth()
        {
            if (_allSymbols.Count() == 0) return this;
            var ordered = this.OrderBy(x => x.ID.Date).ToList();
            var frontMonth = ordered.TakeWhile(x => ordered[0].ID.Date == x.ID.Date);

            _allSymbols = frontMonth.ToList();
            return this;
        }


        /// <summary>
        /// Returns a list of back month contracts
        /// </summary>
        /// <param name="universe"></param>
        /// <returns></returns>
        public OptionFilterUniverse BackMonths()
        {
            if (_allSymbols.Count() == 0) return this;
            var ordered = this.OrderBy(x => x.ID.Date).ToList();
            var backMonths = ordered.SkipWhile(x => ordered[0].ID.Date == x.ID.Date);

            _allSymbols = backMonths.ToList();
            return this;
        }

        /// <summary>
        /// Returns first of back month contracts
        /// </summary>
        /// <param name="universe"></param>
        /// <returns></returns>
        public OptionFilterUniverse BackMonth()
        {
            return BackMonths().FrontMonth();
        }

        /// <summary>
        /// Applies filter selecting options contracts based on a range of strikes in relative terms
        /// </summary>
        /// <param name="minStrike">The minimum strike relative to the underlying price, for example, -1 would filter out contracts further than 1 strike below market price</param>
        /// <param name="maxStrike">The maximum strike relative to the underlying price, for example, +1 would filter out contracts further than 1 strike above market price</param>
        /// <returns></returns>
        public OptionFilterUniverse Strikes(int minStrike, int maxStrike)
        {
            if (_underlying == null)
            {
                return this;
            }

            if (_underlying.Time.Date != _strikeSizeResolveDate)
            {
                // each day we need to recompute the strike size
                var uniqueStrikes = _allSymbols.DistinctBy(x => x.ID.StrikePrice).OrderBy(x => x.ID.StrikePrice).ToList();
                _strikeSize = uniqueStrikes.Zip(uniqueStrikes.Skip(1), (l, r) => r.ID.StrikePrice - l.ID.StrikePrice).DefaultIfEmpty(5m).Min();
                _strikeSizeResolveDate = _underlying.Time.Date;
            }

            // compute the bounds, no need to worry about rounding and such
            var minPrice = _underlying.Price + minStrike * _strikeSize;
            var maxPrice = _underlying.Price + maxStrike * _strikeSize;

            var filtered =
                   from symbol in _allSymbols
                   let contract = symbol.ID
                   where contract.StrikePrice >= minPrice
                      && contract.StrikePrice <= maxPrice
                   select symbol;

            // new universe is dynamic
            _isDynamic = true;
            _allSymbols = filtered.ToList();
            return this;
        }

        /// <summary>
        /// Applies filter selecting options contracts based on a range of expiration dates relative to the current day 
        /// </summary>
        /// <param name="minExpiry">The minimum time until expiry to include, for example, TimeSpan.FromDays(10)
        /// would exclude contracts expiring in less than 10 days</param>
        /// <param name="maxExpiry">The maxmium time until expiry to include, for example, TimeSpan.FromDays(10)
        /// would exclude contracts expiring in more than 10 days</param>
        /// <returns></returns>
        public OptionFilterUniverse Expiration(TimeSpan minExpiry, TimeSpan maxExpiry)
        {
            if (_underlying == null)
            {
                return this;
            }

            if (maxExpiry > Time.MaxTimeSpan) maxExpiry = Time.MaxTimeSpan;

            var minExpiryToDate = _underlying.Time.Date + minExpiry;
            var maxExpiryToDate = _underlying.Time.Date + maxExpiry;

            // ReSharper disable once PossibleMultipleEnumeration - ReSharper is wrong here due to the ToList call
            var filtered =
                   from symbol in _allSymbols
                   let contract = symbol.ID
                   where contract.Date >= minExpiryToDate
                      && contract.Date <= maxExpiryToDate
                   select symbol;

            _allSymbols = filtered.ToList();
            return this;
        }

        /// <summary>
        /// IEnumerable interface method implementation
        /// </summary>
        public IEnumerator<Symbol> GetEnumerator()
        {
            return _allSymbols.GetEnumerator();
        }

        /// <summary>
        /// IEnumerable interface method implementation
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _allSymbols.GetEnumerator();
        }
    }

    /// <summary>
    /// Extensions for Linq support
    /// </summary>
    public static class OptionFilterUniverseEx
    {
        /// <summary>
        /// Filters universe 
        /// </summary>
        /// <param name="universe"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static OptionFilterUniverse Where(this OptionFilterUniverse universe, Func<Symbol, bool> predicate)
        {
            universe._allSymbols = universe._allSymbols.Where(predicate).ToList();
            universe._isDynamic = true;
            return universe;
        }

        /// <summary>
        /// Maps universe 
        /// </summary>
        public static OptionFilterUniverse Select(this OptionFilterUniverse universe, Func<Symbol, Symbol> mapFunc)
        {
            universe._allSymbols = universe._allSymbols.Select(mapFunc).ToList();
            universe._isDynamic = true;
            return universe;
        }

        /// <summary>
        /// Binds universe 
        /// </summary>
        public static OptionFilterUniverse SelectMany(this OptionFilterUniverse universe, Func<Symbol, IEnumerable<Symbol>> mapFunc)
        {
            universe._allSymbols = universe._allSymbols.SelectMany(mapFunc).ToList();
            universe._isDynamic = true;
            return universe;
        }
    }
}
