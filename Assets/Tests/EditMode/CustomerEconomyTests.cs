using System.Collections.Generic;
using NUnit.Framework;
using Telve.Data;
using Telve.Gameplay;
using UnityEngine;

namespace Telve.Tests
{
    /// <summary>docs/design/04-economy.md müşteri eşik/ödeme tablosunu doğrular.</summary>
    public class CustomerEconomyTests
    {
        static CharmData MakeCharm(string id)
        {
            var charm = ScriptableObject.CreateInstance<CharmData>();
            charm.charmId = id;
            return charm;
        }

        [Test]
        public void Regular_FirstCustomer_MatchesDocTable()
        {
            var profile = CustomerProfile.Regular(1);
            Assert.AreEqual(16, profile.Threshold);
            Assert.AreEqual(8, profile.BasePayment);
        }

        [Test]
        public void Regular_LastCustomer_MatchesDocTable()
        {
            var profile = CustomerProfile.Regular(8);
            Assert.AreEqual(44, profile.Threshold);
            Assert.AreEqual(22, profile.BasePayment);
        }

        [Test]
        public void Muhtar_MatchesDocTable()
        {
            var profile = CustomerProfile.Muhtar();
            Assert.AreEqual(53, profile.Threshold);
            Assert.AreEqual(35, profile.BasePayment);
            Assert.IsTrue(profile.IsMuhtar);
        }

        [TestCase(0)]
        [TestCase(9)]
        public void Regular_IndexOutOfRange_Throws(int index)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => CustomerProfile.Regular(index));
        }

        [Test]
        public void Evaluate_ExactlyAtThreshold_PaysBaseAmountOnly()
        {
            var profile = CustomerProfile.Regular(1); // threshold 16, base 8

            var result = CustomerEconomy.Evaluate(profile, satisfaction: 16f);

            Assert.IsTrue(result.ThresholdMet);
            Assert.AreEqual(8, result.Payment);
        }

        [Test]
        public void Evaluate_AboveThreshold_AddsThirtyPercentOfExcess()
        {
            var profile = CustomerProfile.Regular(1); // threshold 16, base 8

            var result = CustomerEconomy.Evaluate(profile, satisfaction: 26f); // excess 10 -> +3

            Assert.IsTrue(result.ThresholdMet);
            Assert.AreEqual(11, result.Payment);
        }

        [Test]
        public void Evaluate_BelowThreshold_PaysFortyPercentOfBase()
        {
            var profile = CustomerProfile.Regular(1); // threshold 16, base 8

            var result = CustomerEconomy.Evaluate(profile, satisfaction: 10f); // 8 * 0.4 = 3.2 -> 3

            Assert.IsFalse(result.ThresholdMet);
            Assert.AreEqual(3, result.Payment);
        }

        [Test]
        public void Evaluate_MuhtarWithGozdesiCharm_PaymentIsTimesOnePointFive()
        {
            var profile = CustomerProfile.Muhtar(); // threshold 53, base 35
            var charms = new List<CharmData> { MakeCharm("muhtarin_gozdesi") };

            var result = CustomerEconomy.Evaluate(profile, satisfaction: 53f, charms); // met -> 35 * 1.5

            // MathF.Round uses banker's rounding (ToEven): 52.5 -> 52.
            Assert.AreEqual(52, result.Payment);
        }

        [Test]
        public void Evaluate_MuhtarWithoutGozdesiCharm_PaymentIsUnchanged()
        {
            var profile = CustomerProfile.Muhtar(); // threshold 53, base 35

            var result = CustomerEconomy.Evaluate(profile, satisfaction: 53f);

            Assert.AreEqual(35, result.Payment);
        }
    }
}
