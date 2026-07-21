using System.Collections.Generic;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 4 "Analitik: koşu tamamlama, ölüm noktaları, kombo
    /// keşif oranları, D1/D7 retention". Tek giriş noktası — GameController
    /// bu sınıfı çağırır, gerçek olay gönderimi Sink'e devredilir (bkz.
    /// IAnalyticsSink). D1/D7 retention hesaplaması çoklu-oturum verisi ve
    /// bir backend gerektirir; burada sadece oturum başlangıcı loglanır,
    /// hesaplama bu sınıfın kapsamı dışında.
    /// </summary>
    public static class AnalyticsEvents
    {
        public static IAnalyticsSink Sink { get; set; } = new LocalFileAnalyticsSink();

        public static void SessionStarted()
        {
            Sink?.LogEvent("session_started", null);
        }

        public static void RunStarted(string characterId)
        {
            Sink?.LogEvent("run_started", new Dictionary<string, object> { ["character_id"] = characterId });
        }

        public static void RunEnded(bool won, int customersReached, int finalGold, int wisdomEarned)
        {
            Sink?.LogEvent("run_ended", new Dictionary<string, object>
            {
                ["won"] = won,
                ["customers_reached"] = customersReached,
                ["final_gold"] = finalGold,
                ["wisdom_earned"] = wisdomEarned,
            });
        }

        /// <summary>customerIndex: kaybedilen müşteri sırası (1-8 sıradan, 9 = muhtar).</summary>
        public static void DeathPoint(int customerIndex)
        {
            Sink?.LogEvent("death_point", new Dictionary<string, object> { ["customer_index"] = customerIndex });
        }

        public static void ComboDiscovered(string comboId)
        {
            Sink?.LogEvent("combo_discovered", new Dictionary<string, object> { ["combo_id"] = comboId });
        }
    }
}
