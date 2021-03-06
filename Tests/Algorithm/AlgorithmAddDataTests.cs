using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using QuantConnect.Algorithm;
using QuantConnect.Configuration;
using QuantConnect.Data;
using QuantConnect.Data.Auxiliary;
using QuantConnect.Data.Custom;
using QuantConnect.Data.Market;
using QuantConnect.Securities;

namespace QuantConnect.Tests.Algorithm
{

    [TestFixture]
    public class AlgorithmAddDataTests
    {
        [Test]
        public void DefaultDataFeeds_CanBeOverwritten_Successfully()
        {
            Config.Set("security-data-feeds", "{ Forex: [\"Trade\"] }");
            var algo = new QCAlgorithm();

            // forex defult - should be tradebar
            var forexTrade = algo.AddForex("EURUSD");
            Assert.IsTrue(forexTrade.Subscriptions.Count() == 1);
            Assert.IsTrue(GetMatchingSubscription(forexTrade, typeof(QuoteBar)) != null);

            // Change 
            var dataFeedsConfigString = Config.Get("security-data-feeds");
            Dictionary<SecurityType, List<TickType>> dataFeeds = new Dictionary<SecurityType, List<TickType>>();
            if (dataFeedsConfigString != string.Empty)
            {
                dataFeeds = JsonConvert.DeserializeObject<Dictionary<SecurityType, List<TickType>>>(dataFeedsConfigString);
            }

            algo.SetAvailableDataTypes(dataFeeds);

            // new forex - should be quotebar
            var forexQuote = algo.AddForex("EURUSD");
            Assert.IsTrue(forexQuote.Subscriptions.Count() == 1);
            Assert.IsTrue(GetMatchingSubscription(forexQuote, typeof(TradeBar)) != null);
        }


        [Test]
        public void DefaultDataFeeds_AreAdded_Successfully()
        {
            var algo = new QCAlgorithm();

            // forex
            var forex = algo.AddSecurity(SecurityType.Forex, "eurusd");
            Assert.IsTrue(forex.Subscriptions.Count() == 1);
            Assert.IsTrue(GetMatchingSubscription(forex, typeof(QuoteBar)) != null);

            // equity
            var equity = algo.AddSecurity(SecurityType.Equity, "goog");
            Assert.IsTrue(equity.Subscriptions.Count() == 1);
            Assert.IsTrue(GetMatchingSubscription(equity, typeof(TradeBar)) != null);

            // option
            var option = algo.AddSecurity(SecurityType.Option, "goog");
            Assert.IsTrue(option.Subscriptions.Count() == 1);
            Assert.IsTrue(GetMatchingSubscription(option, typeof(ZipEntryName)) != null);

            // cfd 
            var cfd = algo.AddSecurity(SecurityType.Cfd, "abc");
            Assert.IsTrue(cfd.Subscriptions.Count() == 1);
            Assert.IsTrue(GetMatchingSubscription(cfd, typeof(QuoteBar)) != null);

            // future 
            var future = algo.AddSecurity(SecurityType.Future, "ES");
            Assert.IsTrue(future.Subscriptions.Count() == 1);
            Assert.IsTrue(future.Subscriptions.FirstOrDefault(x => typeof(ZipEntryName).IsAssignableFrom(x.Type)) != null);
        }


        [Test]
        public void CustomDataTypes_AreAddedToSubscriptions_Successfully()
        {
            var qcAlgorithm = new QCAlgorithm();

            // Add a bitcoin subscription
            qcAlgorithm.AddData<Bitcoin>("BTC");
            var bitcoinSubscription = qcAlgorithm.SubscriptionManager.Subscriptions.FirstOrDefault(x => x.Type == typeof(Bitcoin));
            Assert.AreEqual(bitcoinSubscription.Type, typeof(Bitcoin));

            // Add a quandl subscription
            qcAlgorithm.AddData<Quandl>("EURCAD");
            var quandlSubscription = qcAlgorithm.SubscriptionManager.Subscriptions.FirstOrDefault(x => x.Type == typeof(Quandl));
            Assert.AreEqual(quandlSubscription.Type, typeof(Quandl));
        }


        private static SubscriptionDataConfig GetMatchingSubscription(Security security, Type type)
        {
            // find a subscription matchin the requested type with a higher resolution than requested
            return (from sub in security.Subscriptions.OrderByDescending(s => s.Resolution)
                    where type.IsAssignableFrom(sub.Type)
                    select sub).FirstOrDefault();
        }
    }
}
