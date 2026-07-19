using NUnit.Framework;
using Telve.Gameplay;

namespace Telve.Tests
{
    /// <summary>docs/design/00-core-loop.md "Gün Döngüsü" akışını doğrular.</summary>
    public class DaySessionTests
    {
        static EncounterResult MakeEncounter(bool thresholdMet, int payment)
        {
            var score = new ScoreResult(0f, 0f, new System.Collections.Generic.List<ComboMatch>());
            var customerResult = new CustomerResult(thresholdMet, payment);
            return new EncounterResult(score, customerResult);
        }

        [Test]
        public void FullDay_AllThresholdsMet_CompletesAndAccumulatesGold()
        {
            var day = new DaySession(startingGold: 20);

            for (int i = 1; i <= CustomerEconomy.RegularCustomerCount; i++)
            {
                Assert.AreEqual(i, day.CurrentCustomerIndex);
                Assert.IsFalse(day.IsMuhtarTurn);
                day.SubmitEncounter(MakeEncounter(thresholdMet: true, payment: 10));
            }

            Assert.IsTrue(day.IsMuhtarTurn);
            day.SubmitEncounter(MakeEncounter(thresholdMet: true, payment: 50));

            Assert.IsTrue(day.DayComplete);
            Assert.IsFalse(day.DayLost);
            Assert.AreEqual(20 + 8 * 10 + 50, day.Gold);
            Assert.AreEqual(9, day.History.Count);
        }

        [Test]
        public void RegularCustomerMissesThreshold_DayContinues()
        {
            var day = new DaySession(startingGold: 0);

            day.SubmitEncounter(MakeEncounter(thresholdMet: false, payment: 3));

            Assert.IsFalse(day.DayLost);
            Assert.IsFalse(day.DayComplete);
            Assert.AreEqual(2, day.CurrentCustomerIndex);
            Assert.AreEqual(3, day.Gold);
        }

        [Test]
        public void MuhtarMissesThreshold_DayIsLost()
        {
            var day = new DaySession(startingGold: 0);

            for (int i = 1; i <= CustomerEconomy.RegularCustomerCount; i++)
            {
                day.SubmitEncounter(MakeEncounter(thresholdMet: true, payment: 10));
            }

            day.SubmitEncounter(MakeEncounter(thresholdMet: false, payment: 14));

            Assert.IsTrue(day.DayLost);
            Assert.IsFalse(day.DayComplete);
        }

        [Test]
        public void SubmitEncounter_AfterDayOver_Throws()
        {
            var day = new DaySession(startingGold: 0);
            for (int i = 1; i <= CustomerEconomy.RegularCustomerCount; i++)
            {
                day.SubmitEncounter(MakeEncounter(thresholdMet: true, payment: 1));
            }
            day.SubmitEncounter(MakeEncounter(thresholdMet: true, payment: 1)); // muhtar -> DayComplete

            Assert.Throws<System.InvalidOperationException>(
                () => day.SubmitEncounter(MakeEncounter(thresholdMet: true, payment: 1)));
        }

        [Test]
        public void NoRngConstructor_AllRegularCustomersAreRegularArchetype()
        {
            var day = new DaySession(startingGold: 0); // eski tek parametreli constructor — geriye dönük uyumluluk

            for (int i = 1; i <= CustomerEconomy.RegularCustomerCount; i++)
            {
                Assert.AreEqual(CustomerArchetype.Regular, day.CurrentProfile().Archetype);
                day.SubmitEncounter(MakeEncounter(thresholdMet: true, payment: 1));
            }
        }

        [Test]
        public void RngConstructor_AssignsArchetypesFromPool()
        {
            var day = new DaySession(startingGold: 0, new System.Random(12345));

            bool anyNonRegular = false;
            for (int i = 1; i <= CustomerEconomy.RegularCustomerCount; i++)
            {
                if (day.CurrentProfile().Archetype != CustomerArchetype.Regular) anyNonRegular = true;
                day.SubmitEncounter(MakeEncounter(thresholdMet: true, payment: 1));
            }

            // Sabit seed ile 8 çekilişin hepsi Regular çıkma olasılığı (1/5)^8 —
            // pratikte imkansız; en az bir arketip farkı bekleniyor.
            Assert.IsTrue(anyNonRegular);
        }

        [Test]
        public void CurrentProfile_MuhtarTurn_IgnoresArchetypePool()
        {
            var day = new DaySession(startingGold: 0, new System.Random(1));
            for (int i = 1; i <= CustomerEconomy.RegularCustomerCount; i++)
            {
                day.SubmitEncounter(MakeEncounter(thresholdMet: true, payment: 1));
            }

            Assert.IsTrue(day.IsMuhtarTurn);
            Assert.AreEqual(CustomerArchetype.Regular, day.CurrentProfile().Archetype);
            Assert.IsTrue(day.CurrentProfile().PunishesNegativeCombos);
        }
    }
}
