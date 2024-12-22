using System.Collections.Generic;
using Code.Runtime.Infrastructure.Services.StaticData;
using Code.Runtime.Logic.Books;
using Code.Runtime.Services.Player;
using Code.Runtime.Services.Player.Inventory;
using Code.Runtime.StaticData.Books;
using UnityEngine;
using Zenject;

namespace Code.Runtime.Logic.Player
{
    internal sealed class PlayerInventoryView : MonoBehaviour
    {
        [SerializeField]
        private Book[] _bookViews;
        private IPlayerInventoryService _playerInventoryService;
        private IStaticDataService _staticData;

        [Inject]
        private void Construct(IPlayerInventoryService playerInventoryService, IStaticDataService staticDataService)
        {
            _playerInventoryService = playerInventoryService;
            _staticData = staticDataService;
        }

        private void Awake() =>
            _playerInventoryService.BooksUpdated += UpdateView;

        private void Start() =>
            UpdateView();

        private void OnDestroy() =>
            _playerInventoryService.BooksUpdated -= UpdateView;

        private void UpdateView()
        {
            IReadOnlyList<string> books = _playerInventoryService.Books;
            for(int i = 0; i < _bookViews.Length; i++)
            {
                if(i >= books.Count)
                {
                    _bookViews[i].Hide();
                    continue;
                }

                _bookViews[i].Show();
                string bookId = books[i];
                Material targetMaterial = GetBookMaterial(bookId);
                _bookViews[i].SetView(targetMaterial, GetBookIcon(bookId));
            }
        }

        private Material GetBookMaterial(string bookId)
        {
            if(string.IsNullOrWhiteSpace(bookId))
                return null;
            
            StaticBook data = _staticData.ForBook(bookId);
            return data.StaticBookType.Material;
        }
        
        private Sprite GetBookIcon(string bookId)
        {
            if(string.IsNullOrWhiteSpace(bookId))
                return null;
            
            StaticBook data = _staticData.ForBook(bookId);
            return data.StaticBookType.Icon;
        }
    }
}