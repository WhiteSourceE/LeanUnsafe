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
using System.Linq;
using QuantConnect.Data;
using QuantConnect.Data.Market;
using QuantConnect.Orders;
using QuantConnect.Securities.Option;

namespace QuantConnect.Algorithm.CSharp
{
    /// <summary>
    /// This example demonstrates how to add option strategies for a given underlying equity security.
    /// It also shows how you can prefilter contracts easily based on strikes and expirations.
    /// It also shows how you can inspect the option chain to pick a specific option contract to trade.
    /// </summary>
    public class BasicTemplateOptionStrategyAlgorithm : QCAlgorithm
    {
        private const string UnderlyingTicker = "GOOG";
        public readonly Symbol Underlying = QuantConnect.Symbol.Create(UnderlyingTicker, SecurityType.Equity, Market.USA);
        public readonly Symbol OptionSymbol = QuantConnect.Symbol.Create(UnderlyingTicker, SecurityType.Option, Market.USA);

        public override void Initialize()
        {
            SetStartDate(2015, 12, 24);
            SetEndDate(2015, 12, 24);
            SetCash(1000000);

            var equity = AddEquity(UnderlyingTicker);
            var option = AddOption(UnderlyingTicker);

            equity.SetDataNormalizationMode(DataNormalizationMode.Raw);

            // set our strike/expiry filter for this option chain
            option.SetFilter(-2, +2, TimeSpan.Zero, TimeSpan.FromDays(180));

            // use the underlying equity as the benchmark
            SetBenchmark(equity.Symbol);
        }

        /// <summary>
        /// Event - v3.0 DATA EVENT HANDLER: (Pattern) Basic template for user to override for receiving all subscription data in a single event
        /// </summary>
        /// <param name="slice">The current slice of data keyed by symbol string</param>
        public override void OnData(Slice slice)
        {
            if (!Portfolio.Invested)
            {
                OptionChain chain;
                if (slice.OptionChains.TryGetValue(OptionSymbol, out chain))
                {
                    var atmStraddle = chain
                        .OrderBy(x => Math.Abs(chain.Underlying.Price - x.Strike))
                        .ThenByDescending(x => x.Expiry)
                        .FirstOrDefault();

                    if (atmStraddle != null)
                    {
                        Sell(OptionStrategies.Straddle(OptionSymbol, atmStraddle.Strike, atmStraddle.Expiry), 2);
                    }
                }
            }
            else
            {
                Liquidate();
            }

            foreach(var kpv in slice.Bars)
            {
                Console.WriteLine("---> OnData: {0}, {1}, {2}", Time, kpv.Key.Value, kpv.Value.Close.ToString("0.00"));
            }
        }

        /// <summary>
        /// Order fill event handler. On an order fill update the resulting information is passed to this method.
        /// </summary>
        /// <param name="orderEvent">Order event details containing details of the evemts</param>
        /// <remarks>This method can be called asynchronously and so should only be used by seasoned C# experts. Ensure you use proper locks on thread-unsafe objects</remarks>
        public override void OnOrderEvent(OrderEvent orderEvent)
        {
            Log(orderEvent.ToString());
        }
    }
}
