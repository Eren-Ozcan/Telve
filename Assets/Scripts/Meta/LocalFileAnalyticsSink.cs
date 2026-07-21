using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 4 "Analitik" varsayılan hedefi: gerçek bir backend
    /// bağlanana kadar olayları cihazda JSON-lines olarak biriktirir
    /// (Application.persistentDataPath/analytics.log). Üretim için değil —
    /// yerel geliştirme/QA sırasında olay akışını görünür kılmak için.
    /// </summary>
    public class LocalFileAnalyticsSink : IAnalyticsSink
    {
        readonly string _path;

        public LocalFileAnalyticsSink(string path = null)
        {
            _path = path ?? Path.Combine(Application.persistentDataPath, "analytics.log");
        }

        public void LogEvent(string eventName, IReadOnlyDictionary<string, object> parameters)
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append($"\"ts\":\"{DateTime.UtcNow:o}\",\"event\":\"{eventName}\"");

            if (parameters != null)
            {
                foreach (var kv in parameters)
                {
                    sb.Append($",\"{kv.Key}\":{FormatValue(kv.Value)}");
                }
            }

            sb.Append('}');

            try
            {
                File.AppendAllText(_path, sb.ToString() + Environment.NewLine);
            }
            catch (IOException)
            {
                // Yerel log yazımı başarısız olsa da oyunu bloklamaz — analitik kritik değil.
            }
        }

        static string FormatValue(object value) => value switch
        {
            null => "null",
            bool b => b ? "true" : "false",
            string s => $"\"{s.Replace("\"", "'")}\"",
            _ => Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture),
        };
    }
}
