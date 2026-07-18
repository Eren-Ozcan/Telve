using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Telve.Data;
using Telve.Gameplay;
using UnityEditor;
using UnityEngine;

namespace Telve.Tests
{
    /// <summary>
    /// docs/design/00-core-loop.md "Bir Müşteri Turu" uçtan uca:
    /// gerçek Assets/Data/Combos kütüphanesiyle puanlama + ödeme.
    /// </summary>
    public class CustomerEncounterTests
    {
        List<ComboData> _comboLibrary;

        [SetUp]
        public void LoadComboLibrary()
        {
            var guids = AssetDatabase.FindAssets("t:ComboData", new[] { "Assets/Data/Combos" });
            _comboLibrary = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<ComboData>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToList();
        }

        static SymbolData MakeSymbol(string id, int baseValue)
        {
            var symbol = ScriptableObject.CreateInstance<SymbolData>();
            symbol.symbolId = id;
            symbol.baseValue = baseValue;
            return symbol;
        }

        [Test]
        public void Resolve_KesinHaber_AgainstFirstCustomer_MeetsThresholdAndPays()
        {
            var profile = CustomerProfile.Regular(1); // threshold 16, base 8
            var readingOrder = new List<SymbolData> { MakeSymbol("yol", 3), MakeSymbol("kus", 3), MakeSymbol("mektup", 2) };

            var result = CustomerEncounter.Resolve(profile, readingOrder, _comboLibrary);

            // Skor: 8 x 2.5 = 20 (bkz. ScoreCalculatorTests). Eşik 16, aşım 4 -> +round(4*0.3)=+1.
            Assert.AreEqual(20f, result.Score.FinalScore, 0.001f);
            Assert.IsTrue(result.Payment.ThresholdMet);
            Assert.AreEqual(9, result.Payment.Payment);
        }

        [Test]
        public void Resolve_WeakReading_AgainstLastCustomer_MissesThreshold()
        {
            var profile = CustomerProfile.Regular(8); // threshold 44
            var readingOrder = new List<SymbolData> { MakeSymbol("goz", 2) }; // tek sembol, kombo yok, skor 2

            var result = CustomerEncounter.Resolve(profile, readingOrder, _comboLibrary);

            Assert.AreEqual(2f, result.Score.FinalScore, 0.001f);
            Assert.IsFalse(result.Payment.ThresholdMet);
        }
    }
}
