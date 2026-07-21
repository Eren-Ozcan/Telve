using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 3 "Kayıt sistemi: meta ilerleme kalıcılığı". Bilgelik
    /// puanı toplamını ve falcı defteri keşiflerini oturumlar arası
    /// PlayerPrefs ile saklar. Koşu-ortası kayıt/devam (aynı maddenin diğer
    /// yarısı) kapsam dışı — o, tek bir koşunun kendi durumunu saklamayı
    /// gerektirir, bu sınıf sadece koşular-arası kalıcı ilerlemeyi tutar.
    /// </summary>
    public static class MetaProgressStore
    {
        const string TotalWisdomKey = "Telve.Meta.TotalWisdom";
        const string DiscoveredCombosKey = "Telve.Meta.DiscoveredCombos";
        const char ComboIdSeparator = '|';

        public static int LoadTotalWisdom() => PlayerPrefs.GetInt(TotalWisdomKey, 0);

        public static void SaveTotalWisdom(int total)
        {
            PlayerPrefs.SetInt(TotalWisdomKey, total);
            PlayerPrefs.Save();
        }

        public static IEnumerable<string> LoadDiscoveredComboIds()
        {
            string raw = PlayerPrefs.GetString(DiscoveredCombosKey, string.Empty);
            return string.IsNullOrEmpty(raw) ? Enumerable.Empty<string>() : raw.Split(ComboIdSeparator);
        }

        public static void SaveDiscoveredComboIds(IEnumerable<string> comboIds)
        {
            PlayerPrefs.SetString(DiscoveredCombosKey, string.Join(ComboIdSeparator, comboIds));
            PlayerPrefs.Save();
        }
    }
}
