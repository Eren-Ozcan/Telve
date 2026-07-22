using System;
using System.Collections.Generic;

namespace Telve.Meta
{
    [Serializable]
    public class SavedCustomerResult
    {
        public bool thresholdMet;
        public int payment;
    }

    /// <summary>
    /// ROADMAP.md Faz 3 "Kayıt sistemi: koşu ortası kayıt/devam (mobilde
    /// şart)". Tek bir koşunun tam durumu — JsonUtility ile serileştirilir.
    /// Sembol/tılsım/kombo referansları ScriptableObject yerine id string
    /// olarak tutulur (ScriptableObject'ler JsonUtility ile oturumlar arası
    /// güvenle serileştirilemez); yüklerken GameController'ın zaten
    /// Resources'tan yüklediği kütüphanelerden id'ye göre geri çözülür.
    /// RNG durumu kasıtlı olarak yok — bkz. DaySession.Restore.
    /// </summary>
    [Serializable]
    public class RunSaveData
    {
        public int gold;
        public int currentCustomerIndex;
        public bool dayLost;
        public bool dayComplete;
        public List<SavedCustomerResult> history = new();
        public List<int> archetypes = new();

        public List<string> ownedSymbolIds = new();
        public List<string> activeCharmIds = new();
        public List<string> currentCupSymbolIds = new();
        public List<int> readingOrderCupIndices = new();
        public bool currentCupResolved;

        public string selectedCharacterId = "";
        public int newCombosThisRun;
        public string bestComboId = "";
        public float bestComboImpact;
    }
}
