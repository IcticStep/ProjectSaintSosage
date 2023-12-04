using System.Collections.Generic;
using System.Linq;
using Code.Runtime.Data.Progress;
using Code.Runtime.Infrastructure.Services.PersistentProgress;
using Code.Runtime.Infrastructure.Services.StaticData;
using Code.Runtime.Services.Random;
using JetBrains.Annotations;

namespace Code.Runtime.Services.BooksDelivering
{
    [UsedImplicitly]
    internal sealed class BooksDeliveringService : IBooksDeliveringService
    {
        private readonly IPersistantProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IRandomService _randomService;

        private BooksDeliveringData DeliveringData => _progressService.Progress.WorldData.BooksDeliveringData;
        public int CurrentDayBooksDelivering => _staticDataService.BookReceiving.BooksPerDeliveringAmount;

        public BooksDeliveringService(IPersistantProgressService progressService, IStaticDataService staticDataService, IRandomService randomService)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
            _randomService = randomService;
        }

        public void DeliverBooksInTruck()
        {
            IReadOnlyList<string> booksToChoose = GetBooksToChoose();
            for(int i = 0; i < CurrentDayBooksDelivering; i++)
            {
                int chosenIndex = _randomService.GetInRange(0, booksToChoose.Count);
                string chosenId = booksToChoose[chosenIndex];
                DeliveringData.AddToPreparedForDelivering(chosenId);
            }   
        }

        private IReadOnlyList<string> GetBooksToChoose() =>
            _staticDataService
                .AllBooks
                .Select(book => book.Id)
                .ToList();
    }
}