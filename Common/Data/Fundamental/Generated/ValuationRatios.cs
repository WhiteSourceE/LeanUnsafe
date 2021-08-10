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
*/

//------------------------------------------------------------------------------
// This code was generated by a tool.
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

// To get up to date fundamental definition files for your hedgefund contact sales@quantconnect.com

using System;
using System.IO;
using Newtonsoft.Json;

namespace QuantConnect.Data.Fundamental
{
	/// <summary>
	/// Definition of the ValuationRatios class
	/// </summary>
	public class ValuationRatios : BaseData
	{
		/// <summary>
		/// Dividend per share / Diluted earnings per share
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14000
		/// </remarks>
		[JsonProperty("14000")]
		public decimal PayoutRatio { get; set; }

		/// <summary>
		/// ROE * (1 - Payout Ratio)
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14001
		/// </remarks>
		[JsonProperty("14001")]
		public decimal SustainableGrowthRate { get; set; }

		/// <summary>
		/// Refers to the ratio of free cash flow to enterprise value. Morningstar calculates the ratio by using the underlying data reported in
		/// the company filings or reports:   FCF /Enterprise Value.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14002
		/// </remarks>
		[JsonProperty("14002")]
		public decimal CashReturn { get; set; }

		/// <summary>
		/// Sales / Average Diluted Shares Outstanding
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14003
		/// </remarks>
		[JsonProperty("14003")]
		public decimal SalesPerShare { get; set; }

		/// <summary>
		/// Common Shareholder's Equity / Diluted Shares Outstanding
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14004
		/// </remarks>
		[JsonProperty("14004")]
		public decimal BookValuePerShare { get; set; }

		/// <summary>
		/// Cash Flow from Operations / Average Diluted Shares Outstanding
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14005
		/// </remarks>
		[JsonProperty("14005")]
		public decimal CFOPerShare { get; set; }

		/// <summary>
		/// Free Cash Flow / Average Diluted Shares Outstanding
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14006
		/// </remarks>
		[JsonProperty("14006")]
		public decimal FCFPerShare { get; set; }

		/// <summary>
		/// Diluted EPS / Price
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14007
		/// </remarks>
		[JsonProperty("14007")]
		public decimal EarningYield { get; set; }

		/// <summary>
		/// Adjusted Close Price/ EPS. If the result is negative, zero, &gt;10,000 or &lt;0.001, then null.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14008
		/// </remarks>
		[JsonProperty("14008")]
		public decimal PERatio { get; set; }

		/// <summary>
		/// SalesPerShare / Price
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14009
		/// </remarks>
		[JsonProperty("14009")]
		public decimal SalesYield { get; set; }

		/// <summary>
		/// Adjusted close price / Sales Per Share. If the result is negative or zero, then null.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14010
		/// </remarks>
		[JsonProperty("14010")]
		public decimal PSRatio { get; set; }

		/// <summary>
		/// BookValuePerShare / Price
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14011
		/// </remarks>
		[JsonProperty("14011")]
		public decimal BookValueYield { get; set; }

		/// <summary>
		/// Adjusted close price / Book Value Per Share. If the result is negative or zero, then null.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14012
		/// </remarks>
		[JsonProperty("14012")]
		public decimal PBRatio { get; set; }

		/// <summary>
		/// CFOPerShare / Price
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14013
		/// </remarks>
		[JsonProperty("14013")]
		public decimal CFYield { get; set; }

		/// <summary>
		/// Adjusted close price /Cash Flow Per Share. If the result is negative or zero, then null.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14014
		/// </remarks>
		[JsonProperty("14014")]
		public decimal PCFRatio { get; set; }

		/// <summary>
		/// FCFPerShare / Price
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14015
		/// </remarks>
		[JsonProperty("14015")]
		public decimal FCFYield { get; set; }

		/// <summary>
		/// Adjusted close price/ Free Cash Flow Per Share. If the result is negative or zero, then null.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14016
		/// </remarks>
		[JsonProperty("14016")]
		public decimal FCFRatio { get; set; }

		/// <summary>
		/// Dividends Per Share over the trailing 12 months / Price
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14017
		/// </remarks>
		[JsonProperty("14017")]
		public decimal TrailingDividendYield { get; set; }

		/// <summary>
		/// (Current Dividend Per Share * Payout Frequency) / Price
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14018
		/// </remarks>
		[JsonProperty("14018")]
		public decimal ForwardDividendYield { get; set; }

		/// <summary>
		/// Estimated Earnings Per Share / Price
		/// Note:
		/// a) The "Next" Year's EPS Estimate is used; For instance, if today's actual date is March 1, 2009, the "Current" EPS Estimate for
		/// MSFT is June 2009, and the "Next" EPS Estimate for MSFT is June 2010; the latter is used.
		/// b) The eps estimated data is sourced from a third party.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14019
		/// </remarks>
		[JsonProperty("14019")]
		public decimal ForwardEarningYield { get; set; }

		/// <summary>
		/// 1 / ForwardEarningYield
		/// If result is negative, then null
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14020
		/// </remarks>
		[JsonProperty("14020")]
		public decimal ForwardPERatio { get; set; }

		/// <summary>
		/// ForwardPERatio / Long-term Average Earning Growth Rate
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14021
		/// </remarks>
		[JsonProperty("14021")]
		public decimal PEGRatio { get; set; }

		/// <summary>
		/// The number of years it would take for a company's cumulative earnings to equal the stock's current trading price, assuming that the
		/// company continues to increase its annual earnings at the growth rate used to calculate the PEG ratio.
		/// [ Log (PG/E + 1)  / Log (1 + G) ] - 1
		/// Where
		/// P=Price
		/// E=Next Fiscal Year's Estimated EPS
		/// G=Long-term Average Earning Growth
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14022
		/// </remarks>
		[JsonProperty("14022")]
		public decimal PEGPayback { get; set; }

		/// <summary>
		/// The company's total book value less the value of any intangible assets dividend by number of shares.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14023
		/// </remarks>
		[JsonProperty("14023")]
		public decimal TangibleBookValuePerShare { get; set; }

		/// <summary>
		/// The three year average for tangible book value per share.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14024
		/// </remarks>
		[JsonProperty("14024")]
		public decimal TangibleBVPerShare3YrAvg { get; set; }

		/// <summary>
		/// The five year average for tangible book value per share.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14025
		/// </remarks>
		[JsonProperty("14025")]
		public decimal TangibleBVPerShare5YrAvg { get; set; }

		/// <summary>
		/// Latest Dividend * Frequency
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14026
		/// </remarks>
		[JsonProperty("14026")]
		public decimal ForwardDividend { get; set; }

		/// <summary>
		/// (Current Assets - Current Liabilities)/number of shares
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14027
		/// </remarks>
		[JsonProperty("14027")]
		public decimal WorkingCapitalPerShare { get; set; }

		/// <summary>
		/// The three year average for working capital per share.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14028
		/// </remarks>
		[JsonProperty("14028")]
		public decimal WorkingCapitalPerShare3YrAvg { get; set; }

		/// <summary>
		/// The five year average for working capital per share.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14029
		/// </remarks>
		[JsonProperty("14029")]
		public decimal WorkingCapitalPerShare5YrAvg { get; set; }

		/// <summary>
		/// This reflects the fair market value of a company, and allows comparability to other companies as this is capital structure-neutral.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14030
		/// </remarks>
		[JsonProperty("14030")]
		public decimal EVToEBITDA { get; set; }

		/// <summary>
		/// The net repurchase of shares outstanding over the market capital of the company. It is a measure of shareholder return.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14031
		/// </remarks>
		[JsonProperty("14031")]
		public decimal BuyBackYield { get; set; }

		/// <summary>
		/// The total yield that shareholders can expect, by summing Dividend Yield and Buyback Yield.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14032
		/// </remarks>
		[JsonProperty("14032")]
		public decimal TotalYield { get; set; }

		/// <summary>
		/// The five-year average of the company's price-to-earnings ratio.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14033
		/// </remarks>
		[JsonProperty("14033")]
		public decimal RatioPE5YearAverage { get; set; }

		/// <summary>
		/// Price change this month, expressed as latest price/last month end price.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14034
		/// </remarks>
		[JsonProperty("14034")]
		public decimal PriceChange1M { get; set; }

		/// <summary>
		/// Adjusted Close Price/ Normalized EPS. Normalized EPS removes onetime and unusual items from net EPS, to provide investors with
		/// a more accurate measure of the company's true earnings. If the result is negative, zero, &gt;10,000 or &lt;0.001, then null.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14035
		/// </remarks>
		[JsonProperty("14035")]
		public decimal NormalizedPERatio { get; set; }

		/// <summary>
		/// Adjusted close price/EBITDA Per Share. If the result is negative or zero, then null.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14036
		/// </remarks>
		[JsonProperty("14036")]
		public decimal PricetoEBITDA { get; set; }

		/// <summary>
		/// Average of the last 60 monthly observations of trailing dividend yield in the last 5 years.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14037
		/// </remarks>
		[JsonProperty("14037")]
		public decimal DivYield5Year { get; set; }

		/// <summary>
		/// Indicates the method used to calculate Forward Dividend. There are three options: Annual, Look-back and Manual.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14042
		/// </remarks>
		[JsonProperty("14042")]
		public string ForwardCalculationStyle { get; set; }

		/// <summary>
		/// Used to collect the forward dividend for companies where our formula will not produce the correct value.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14043
		/// </remarks>
		[JsonProperty("14043")]
		public decimal ActualForwardDividend { get; set; }

		/// <summary>
		/// Indicates the method used to calculate Trailing Dividend. There are two options: Look-back and Manual.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14044
		/// </remarks>
		[JsonProperty("14044")]
		public string TrailingCalculationStyle { get; set; }

		/// <summary>
		/// Used to collect the trailing dividend for companies where our formula will not produce the correct value.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14045
		/// </remarks>
		[JsonProperty("14045")]
		public decimal ActualTrailingDividend { get; set; }

		/// <summary>
		/// The growth rate from the TrailingDividend to the Forward Dividend: {(Forward Dividend/Trailing Dividend) - 1}*100.
		/// </summary>
		/// <remarks>
		/// Morningstar DataId: 14047
		/// </remarks>
		[JsonProperty("14047")]
		public decimal ExpectedDividendGrowthRate { get; set; }

		/// <summary>
		/// Creates an instance of the ValuationRatios class
		/// </summary>
		public ValuationRatios()
		{
		}

		/// <summary>
		/// Sets values for non existing periods from a previous instance
		/// </summary>
		/// <remarks>Used to fill-forward values from previous dates</remarks>
		/// <param name="previous">The previous instance</param>
		public void UpdateValues(ValuationRatios previous)
		{
			if (previous == null) return;

		}
	}
}
