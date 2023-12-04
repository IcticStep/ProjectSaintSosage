using System;
using System.Collections.Generic;
using System.Linq;
using Code.Runtime.Data;
using Code.Runtime.Data.Progress;
using Code.Runtime.Infrastructure.Services.PersistentProgress;
using Code.Runtime.Logic.Interactions.Data;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Code.Runtime.Services.Player
{
    [UsedImplicitly]
    internal sealed class PlayerInventoryService : IPlayerInventoryService
    {
        private List<string> _books = new();

        public bool HasBook => BooksCount > 0;
        public int Coins { get; private set; } = 0;
        public int BooksCount => _books.Count;
        public string CurrentBookId => HasBook ? _books[^1] : null;
        public IReadOnlyList<string> Books => _books;

        public event Action BooksUpdated;
        public event Action CoinsUpdated;
        
        public void InsertBooks(IEnumerable<string> bookIds)
        {
            foreach(string bookId in bookIds)
                InsertBook(bookId);
        }

        public void InsertBook(string id)
        {
            _books.Add(id);
            BooksUpdated?.Invoke();
        }

        public string RemoveBook()
        {
            string removedId = _books[^1];
            _books.RemoveAt(_books.Count-1);
            BooksUpdated?.Invoke();
            return removedId;
        }
        
        public void AddCoins(int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException($"Tried to add {amount} coins. Can't add coins amount less then zero!");

            Coins += amount;
            CoinsUpdated?.Invoke();
            Debug.Log($"Coins amount: {Coins}.");
        }
        
        public void RemoveCoins(int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException($"Tried to remove {amount} coins. Can't remove coins amount less then zero!");

            Coins -= amount;
            CoinsUpdated?.Invoke();         
            Debug.Log($"Coins amount: {Coins}.");
        }

        public void LoadProgress(Progress progress)
        {
            _books = progress.PlayerData.Inventory.Books;
            BooksUpdated?.Invoke();
            Coins = progress.PlayerData.Inventory.Coins;
            CoinsUpdated?.Invoke();
        }

        public void UpdateProgress(Progress progress)
        {
            progress.PlayerData.Inventory.Books = _books;
            progress.PlayerData.Inventory.Coins = Coins;
        }
    }
}