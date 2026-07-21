using System.Collections.Generic;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 4 "Analitik": event gönderim hedefi. Gerçek bir
    /// backend (Firebase/GameAnalytics vb.) bu arayüzü implemente edip
    /// AnalyticsEvents.Sink'e atanarak tak-çalıştır entegre edilir —
    /// çağıran kod (GameController) hiç değişmez.
    /// </summary>
    public interface IAnalyticsSink
    {
        void LogEvent(string eventName, IReadOnlyDictionary<string, object> parameters);
    }
}
