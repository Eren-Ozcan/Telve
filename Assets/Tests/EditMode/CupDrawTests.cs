using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Telve.Data;
using Telve.Gameplay;
using UnityEngine;

namespace Telve.Tests
{
    /// <summary>
    /// docs/design/00-core-loop.md adım 1 ve 05-charms.md DrawRng
    /// grubunu doğrular. İstatistiksel dağılım yerine kesin garantileri
    /// (sayı aralığı, seed tekrarlanabilirliği, garantili sembol) test
    /// eder ki suite flaky olmasın.
    /// </summary>
    public class CupDrawTests
    {
        static SymbolData MakeSymbol(string id, SymbolRarity rarity, int drawWeight)
        {
            var symbol = ScriptableObject.CreateInstance<SymbolData>();
            symbol.symbolId = id;
            symbol.baseValue = 1;
            symbol.rarity = rarity;
            symbol.drawWeight = drawWeight;
            return symbol;
        }

        static CharmData MakeCharm(string id)
        {
            var charm = ScriptableObject.CreateInstance<CharmData>();
            charm.charmId = id;
            return charm;
        }

        static List<SymbolData> SamplePool()
        {
            return new List<SymbolData>
            {
                MakeSymbol("yol", SymbolRarity.Common, 100),
                MakeSymbol("kus", SymbolRarity.Common, 100),
                MakeSymbol("dag", SymbolRarity.Rare, 15),
                MakeSymbol("tac", SymbolRarity.Epic, 4),
            };
        }

        [Test]
        public void Draw_WithoutCharms_CountIsWithinFiveToSeven()
        {
            var pool = SamplePool();

            for (int seed = 0; seed < 200; seed++)
            {
                var result = CupDraw.Draw(pool, new System.Random(seed));
                Assert.That(result.Count, Is.InRange(5, 7));
            }
        }

        [Test]
        public void Draw_WithKahveTelvesiYogun_CountIsWithinSixToEight()
        {
            var pool = SamplePool();
            var charms = new List<CharmData> { MakeCharm("kahve_telvesi_yogun") };

            for (int seed = 0; seed < 200; seed++)
            {
                var result = CupDraw.Draw(pool, new System.Random(seed), charms);
                Assert.That(result.Count, Is.InRange(6, 8));
            }
        }

        [Test]
        public void Draw_SameSeed_ProducesIdenticalSequence()
        {
            var pool = SamplePool();

            var first = CupDraw.Draw(pool, new System.Random(1234));
            var second = CupDraw.Draw(pool, new System.Random(1234));

            CollectionAssert.AreEqual(
                first.Select(s => s.symbolId),
                second.Select(s => s.symbolId));
        }

        [Test]
        public void Draw_WithKaderAnahtari_AlwaysIncludesTac()
        {
            var pool = SamplePool();
            var charms = new List<CharmData> { MakeCharm("kader_anahtari") };

            for (int seed = 0; seed < 200; seed++)
            {
                var result = CupDraw.Draw(pool, new System.Random(seed), charms);
                Assert.That(result.Any(s => s.symbolId == "tac"), Is.True, $"seed={seed}");
            }
        }

        [Test]
        public void Draw_EmptyPool_Throws()
        {
            Assert.Throws<ArgumentException>(() => CupDraw.Draw(new List<SymbolData>(), new System.Random(1)));
        }
    }
}
