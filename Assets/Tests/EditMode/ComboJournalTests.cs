using System.Collections.Generic;
using NUnit.Framework;
using Telve.Data;
using Telve.Gameplay;
using UnityEngine;

namespace Telve.Tests
{
    /// <summary>ROADMAP.md Faz 2 "Falcı defteri v1" keşif kaydını doğrular.</summary>
    public class ComboJournalTests
    {
        static ComboData MakeCombo(string id)
        {
            var combo = ScriptableObject.CreateInstance<ComboData>();
            combo.comboId = id;
            return combo;
        }

        [Test]
        public void RecordEncounter_NewCombo_ReturnsAsNewlyDiscovered()
        {
            var journal = new ComboJournal();
            var matches = new List<ComboMatch> { new ComboMatch(MakeCombo("haber_geliyor"), 0) };

            var newlyDiscovered = journal.RecordEncounter(matches);

            Assert.AreEqual(1, newlyDiscovered.Count);
            Assert.AreEqual("haber_geliyor", newlyDiscovered[0]);
            Assert.IsTrue(journal.IsDiscovered("haber_geliyor"));
        }

        [Test]
        public void RecordEncounter_AlreadyDiscovered_NotReturnedAgain()
        {
            var journal = new ComboJournal(new[] { "haber_geliyor" });
            var matches = new List<ComboMatch> { new ComboMatch(MakeCombo("haber_geliyor"), 0) };

            var newlyDiscovered = journal.RecordEncounter(matches);

            Assert.IsEmpty(newlyDiscovered);
            Assert.IsTrue(journal.IsDiscovered("haber_geliyor"));
        }

        [Test]
        public void RecordEncounter_MixOfNewAndKnown_OnlyReturnsNew()
        {
            var journal = new ComboJournal(new[] { "bilinen" });
            var matches = new List<ComboMatch>
            {
                new ComboMatch(MakeCombo("bilinen"), 0),
                new ComboMatch(MakeCombo("yeni"), 1),
            };

            var newlyDiscovered = journal.RecordEncounter(matches);

            Assert.AreEqual(1, newlyDiscovered.Count);
            Assert.AreEqual("yeni", newlyDiscovered[0]);
        }

        [Test]
        public void IsDiscovered_UnknownCombo_ReturnsFalse()
        {
            var journal = new ComboJournal();
            Assert.IsFalse(journal.IsDiscovered("bilinmeyen"));
        }
    }
}
