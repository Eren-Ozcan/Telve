using NUnit.Framework;
using Telve.Meta;

namespace Telve.Tests
{
    public class WisdomRewardTests
    {
        [Test]
        public void GoldOnlyConvertsAtTenToOne()
        {
            Assert.AreEqual(5, WisdomReward.CalculateRunReward(finalGold: 50, muhtarCleared: false, newCombosDiscoveredThisRun: 0));
        }

        [Test]
        public void GoldConversionFloorsRemainder()
        {
            Assert.AreEqual(5, WisdomReward.CalculateRunReward(finalGold: 59, muhtarCleared: false, newCombosDiscoveredThisRun: 0));
        }

        [Test]
        public void MuhtarClearAddsFlatBonus()
        {
            Assert.AreEqual(10, WisdomReward.CalculateRunReward(finalGold: 50, muhtarCleared: true, newCombosDiscoveredThisRun: 0));
        }

        [Test]
        public void NewCombosAddPerComboBonus()
        {
            Assert.AreEqual(11, WisdomReward.CalculateRunReward(finalGold: 0, muhtarCleared: true, newCombosDiscoveredThisRun: 3));
        }

        [Test]
        public void ZeroGoldNoBonusesYieldsZero()
        {
            Assert.AreEqual(0, WisdomReward.CalculateRunReward(finalGold: 0, muhtarCleared: false, newCombosDiscoveredThisRun: 0));
        }
    }
}
