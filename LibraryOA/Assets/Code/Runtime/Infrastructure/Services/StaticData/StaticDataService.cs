using System.Collections.Generic;
using System.Linq;
using Code.Runtime.Data.Progress;
using Code.Runtime.StaticData;
using Code.Runtime.StaticData.Balance;
using Code.Runtime.StaticData.Books;
using Code.Runtime.StaticData.Interactables;
using Code.Runtime.StaticData.Level;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Runtime.Infrastructure.Services.StaticData
{
    [UsedImplicitly]
    public sealed class StaticDataService : IStaticDataService
    {
        private const string BooksPath = "Static Data/Books/Instances";
        private const string LevelsPath = "Static Data/Levels";
        private const string ReadingTablePath = "Static Data/Interactables/ReadingTableData";
        private const string StartupSettingsPath = "Static Data/StartupSettings";
        private const string TruckPath = "Static Data/Interactables/Truck static data";
        private const string BookSlotPath = "Static Data/Interactables/BookSlotData";
        private const string PlayerPath = "Static Data/Player";
        private const string BookReceivingPath = "Static Data/Book Receiving";

        private Dictionary<string, StaticBook> _books = new();
        private Dictionary<string, LevelStaticData> _levels = new();

        public StartupSettings StartupSettings { get; private set; }
        public InteractablesStaticData Interactables { get; private set; }
        public StaticPlayer Player { get; private set; }
        public StaticBookReceiving BookReceiving { get; private set; }
        public IReadOnlyList<StaticBook> AllBooks => _books.Values.ToList();
        public LevelStaticData CurrentLevelData => ForLevel(SceneManager.GetActiveScene().name);

        public void LoadAll()
        {
            LoadStartupSettings();
            LoadLevels();
            LoadPlayer();
            LoadBookReceiving();
            LoadBooks();
            LoadInteractables();
        }

        public void LoadBooks() =>
            _books = Resources
                .LoadAll<StaticBook>(BooksPath)
                .ToDictionary(x => x.Id, x => x);

        public void LoadLevels() =>
            _levels = Resources
                .LoadAll<LevelStaticData>(LevelsPath)
                .ToDictionary(x => x.LevelKey, x => x);

        public void LoadPlayer() =>
            Player = Resources
                .Load<StaticPlayer>(PlayerPath);

        public void LoadBookReceiving() =>
            BookReceiving = Resources
                .Load<StaticBookReceiving>(BookReceivingPath);

        public void LoadInteractables()
        {
            StaticReadingTable readingTable = Resources.Load<StaticReadingTable>(ReadingTablePath);
            StaticBookSlot bookSlot = Resources.Load<StaticBookSlot>(BookSlotPath);
            StaticTruck truck = Resources.Load<StaticTruck>(TruckPath);
            
            Interactables = new InteractablesStaticData(readingTable, bookSlot, truck);
        }

        public void LoadStartupSettings() =>
            StartupSettings = Resources
                .Load<StartupSettings>(StartupSettingsPath);
        
        public StaticBook ForBook(string id) =>
            _books.TryGetValue(id, out StaticBook result)
                ? result
                : null;
        
        public LevelStaticData ForLevel(string key) =>
            _levels.TryGetValue(key, out LevelStaticData result)
                ? result
                : null;
    }
}