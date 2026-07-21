namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 3 "Bilgelik puanı: koşu sonu kazanım". Kalıcı
    /// ilerlemeye (açılımlar ağacı) harcanacak puanın koşu sonunda nasıl
    /// hesaplandığı — saklama (MetaProgressStore) ve harcama (açılımlar
    /// ağacı, henüz yok) bu sınıfın dışında.
    /// </summary>
    public static class WisdomReward
    {
        const int GoldPerWisdomPoint = 10;
        const int MuhtarClearBonus = 5;
        const int WisdomPerNewCombo = 2;

        public static int CalculateRunReward(int finalGold, bool muhtarCleared, int newCombosDiscoveredThisRun)
        {
            int reward = finalGold / GoldPerWisdomPoint;
            if (muhtarCleared) reward += MuhtarClearBonus;
            reward += newCombosDiscoveredThisRun * WisdomPerNewCombo;
            return reward;
        }
    }
}
