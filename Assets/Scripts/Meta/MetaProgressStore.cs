using System;
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
        const string UnlockedCharactersKey = "Telve.Meta.UnlockedCharacters";
        const string SelectedCharacterKey = "Telve.Meta.SelectedCharacter";
        const string LanguageKey = "Telve.Meta.Language";
        const string TutorialCompletedKey = "Telve.Meta.TutorialCompleted";
        const string OwnedCosmeticsKey = "Telve.Meta.OwnedCosmetics";
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

        /// <summary>ROADMAP.md Faz 3 "Açılabilir sembol desteleri" + "2-3 falcı karakteri": hangi FalciCharacter.characterId'lerin açıldığı.</summary>
        public static IEnumerable<string> LoadUnlockedCharacterIds()
        {
            string raw = PlayerPrefs.GetString(UnlockedCharactersKey, string.Empty);
            return string.IsNullOrEmpty(raw) ? Enumerable.Empty<string>() : raw.Split(ComboIdSeparator);
        }

        public static void SaveUnlockedCharacterIds(IEnumerable<string> characterIds)
        {
            PlayerPrefs.SetString(UnlockedCharactersKey, string.Join(ComboIdSeparator, characterIds));
            PlayerPrefs.Save();
        }

        public static string LoadSelectedCharacterId() => PlayerPrefs.GetString(SelectedCharacterKey, string.Empty);

        public static void SaveSelectedCharacterId(string characterId)
        {
            PlayerPrefs.SetString(SelectedCharacterKey, characterId);
            PlayerPrefs.Save();
        }

        /// <summary>Varsayılan Türkçe — ROADMAP.md: proje TR odaklı başladı, EN Faz 4'te eklendi.</summary>
        public static Language LoadLanguage()
        {
            int stored = PlayerPrefs.GetInt(LanguageKey, (int)Language.Turkish);
            return Enum.IsDefined(typeof(Language), stored) ? (Language)stored : Language.Turkish;
        }

        public static void SaveLanguage(Language language)
        {
            PlayerPrefs.SetInt(LanguageKey, (int)language);
            PlayerPrefs.Save();
        }

        /// <summary>ROADMAP.md Faz 4 "Tutorial: ilk müşteri = öğretici fal" — bir daha gösterilmesin diye kalıcı işaret.</summary>
        public static bool LoadTutorialCompleted() => PlayerPrefs.GetInt(TutorialCompletedKey, 0) == 1;

        public static void SaveTutorialCompleted()
        {
            PlayerPrefs.SetInt(TutorialCompletedKey, 1);
            PlayerPrefs.Save();
        }

        /// <summary>ROADMAP.md Faz 4 "IAP altyapısı" — MockPurchaseService'in yerel sahiplik kaydı.</summary>
        public static IEnumerable<string> LoadOwnedCosmeticIds()
        {
            string raw = PlayerPrefs.GetString(OwnedCosmeticsKey, string.Empty);
            return string.IsNullOrEmpty(raw) ? Enumerable.Empty<string>() : raw.Split(ComboIdSeparator);
        }

        public static void SaveOwnedCosmeticIds(IEnumerable<string> cosmeticIds)
        {
            PlayerPrefs.SetString(OwnedCosmeticsKey, string.Join(ComboIdSeparator, cosmeticIds));
            PlayerPrefs.Save();
        }
    }
}
