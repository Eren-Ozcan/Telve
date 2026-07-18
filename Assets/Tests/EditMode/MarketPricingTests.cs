using System.Collections.Generic;
using NUnit.Framework;
using Telve.Data;
using Telve.Gameplay;
using UnityEngine;

namespace Telve.Tests
{
    /// <summary>docs/design/04-economy.md pazar fiyat tablosunu doğrular.</summary>
    public class MarketPricingTests
    {
        static CharmData MakeCharm(string id, int price = 0)
        {
            var charm = ScriptableObject.CreateInstance<CharmData>();
            charm.charmId = id;
            charm.price = price;
            return charm;
        }

        [TestCase(SymbolRarity.Common, 8)]
        [TestCase(SymbolRarity.Uncommon, 15)]
        [TestCase(SymbolRarity.Rare, 28)]
        [TestCase(SymbolRarity.Epic, 50)]
        public void SymbolPrice_MatchesDocTable(SymbolRarity rarity, int expected)
        {
            Assert.AreEqual(expected, MarketPricing.SymbolPrice(rarity));
        }

        [Test]
        public void SymbolPrice_WithEkonomikFal_Is15PercentCheaper()
        {
            var charms = new List<CharmData> { MakeCharm("ekonomik_fal") };

            // 8 * 0.85 = 6.8 -> 7
            Assert.AreEqual(7, MarketPricing.SymbolPrice(SymbolRarity.Common, charms));
        }

        [Test]
        public void CharmPrice_WithEkonomikFal_Is15PercentCheaper()
        {
            var target = MakeCharm("ilk_kombo_carpani", price: 22);
            var activeCharms = new List<CharmData> { MakeCharm("ekonomik_fal") };

            // 22 * 0.85 = 18.7 -> 19
            Assert.AreEqual(19, MarketPricing.CharmPrice(target, activeCharms));
        }

        [Test]
        public void CharmPrice_WithoutEkonomikFal_IsUnchanged()
        {
            var target = MakeCharm("ilk_kombo_carpani", price: 22);
            Assert.AreEqual(22, MarketPricing.CharmPrice(target));
        }
    }
}
