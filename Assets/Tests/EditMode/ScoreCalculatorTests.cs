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
    /// Validates ScoreCalculator against the three worked examples in
    /// docs/design/03-scoring.md, using the real generated ComboData
    /// library (Assets/Resources/Data/Combos) so a mistranscription in
    /// DataAssetGenerator would also fail this suite.
    /// </summary>
    public class ScoreCalculatorTests
    {
        List<ComboData> _comboLibrary;

        [SetUp]
        public void LoadComboLibrary()
        {
            var guids = AssetDatabase.FindAssets("t:ComboData", new[] { "Assets/Resources/Data/Combos" });
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

        static CharmData MakeCharm(string id)
        {
            var charm = ScriptableObject.CreateInstance<CharmData>();
            charm.charmId = id;
            return charm;
        }

        [Test]
        public void KesinHaber_NoCharms_MatchesDocExample()
        {
            Assert.That(_comboLibrary, Is.Not.Empty, "Assets/Resources/Data/Combos boş — DataAssetGenerator çalıştırıldı mı?");

            var readingOrder = new List<SymbolData> { MakeSymbol("yol", 3), MakeSymbol("kus", 3), MakeSymbol("mektup", 2) };

            var result = ScoreCalculator.Calculate(readingOrder, _comboLibrary);

            // docs/design/03-scoring.md Örnek 1: taban 8, "Kesin Haber" ×2.5 -> 20.
            Assert.AreEqual(8f, result.BaseScore, 0.001f);
            Assert.AreEqual(20f, result.FinalScore, 0.001f);
        }

        [Test]
        public void KesinHaber_WithIlkKomboCarpaniCharm_MatchesDocExample()
        {
            var readingOrder = new List<SymbolData> { MakeSymbol("yol", 3), MakeSymbol("kus", 3), MakeSymbol("mektup", 2) };
            var charms = new List<CharmData> { MakeCharm("ilk_kombo_carpani") };

            var result = ScoreCalculator.Calculate(readingOrder, _comboLibrary, charms);

            // docs/design/03-scoring.md Örnek 2: 8 x (2.5 x 2) = 40.
            Assert.AreEqual(40f, result.FinalScore, 0.001f);
        }

        [Test]
        public void KaraGun_NegativeCombo_MatchesDocExample()
        {
            var readingOrder = new List<SymbolData> { MakeSymbol("yilan", 5), MakeSymbol("dag", 8), MakeSymbol("bulut", 2) };

            var result = ScoreCalculator.Calculate(readingOrder, _comboLibrary);

            // docs/design/03-scoring.md Örnek 3: taban 15, "Kara Gün" ×0.6 -> 9.
            Assert.AreEqual(15f, result.BaseScore, 0.001f);
            Assert.AreEqual(9f, result.FinalScore, 0.001f);
        }
    }
}
