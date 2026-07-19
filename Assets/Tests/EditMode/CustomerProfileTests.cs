using NUnit.Framework;
using Telve.Gameplay;

namespace Telve.Tests
{
    /// <summary>ROADMAP.md Faz 2 arketip çarpanlarını doğrular (bkz. CustomerProfile.ArchetypeMultipliers).</summary>
    public class CustomerProfileTests
    {
        [Test]
        public void Regular_DefaultArchetype_MatchesOriginalFormula()
        {
            var profile = CustomerProfile.Regular(3);

            Assert.AreEqual(24, profile.Threshold);
            Assert.AreEqual(12, profile.BasePayment);
            Assert.AreEqual(CustomerArchetype.Regular, profile.Archetype);
            Assert.IsFalse(profile.PunishesNegativeCombos);
        }

        [Test]
        public void Aceleci_LowersThresholdAndPayment()
        {
            var profile = CustomerProfile.Regular(3, CustomerArchetype.Aceleci); // taban: eşik 24, ödeme 12

            Assert.AreEqual(20, profile.Threshold); // round(24*0.85)
            Assert.AreEqual(10, profile.BasePayment); // round(12*0.85)
        }

        [Test]
        public void Kuskucu_RaisesThresholdAndPayment()
        {
            var profile = CustomerProfile.Regular(3, CustomerArchetype.Kuskucu);

            Assert.AreEqual(28, profile.Threshold); // round(24*1.15)
            Assert.AreEqual(14, profile.BasePayment); // round(12*1.15)
        }

        [Test]
        public void Comert_RaisesPaymentOnly()
        {
            var profile = CustomerProfile.Regular(3, CustomerArchetype.Comert);

            Assert.AreEqual(24, profile.Threshold); // değişmez
            Assert.AreEqual(16, profile.BasePayment); // round(12*1.3)
        }

        [Test]
        public void Muhtar_AlwaysPunishesNegativeCombosAndIsRegularArchetype()
        {
            var muhtar = CustomerProfile.Muhtar();

            Assert.IsTrue(muhtar.IsMuhtar);
            Assert.IsTrue(muhtar.PunishesNegativeCombos);
            Assert.AreEqual(CustomerArchetype.Regular, muhtar.Archetype);
        }

        [Test]
        public void OutOfRangeIndex_Throws()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => CustomerProfile.Regular(0));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => CustomerProfile.Regular(9));
        }
    }
}
