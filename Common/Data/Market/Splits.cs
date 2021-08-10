﻿/*
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

namespace QuantConnect.Data.Market
{
    /// <summary>
    /// Collection of splits keyed by <see cref="Symbol"/>
    /// </summary>
    public class Splits : DataDictionary<Split>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Splits"/> dictionary
        /// </summary>
        public Splits()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Splits"/> dictionary
        /// </summary>
        /// <param name="frontier">The time associated with the data in this dictionary</param>
        public Splits(DateTime frontier)
            : base(frontier)
        {
        }
    }
}