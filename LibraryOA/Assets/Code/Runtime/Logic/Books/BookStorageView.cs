using Code.Runtime.Infrastructure.Services.StaticData;
using Code.Runtime.Logic.Interactables;
using Code.Runtime.StaticData.Books;
using UnityEngine;
using Zenject;

namespace Code.Runtime.Logic.Books
{
    internal sealed class BookStorageView : MonoBehaviour
    {
        [SerializeField] 
        private BookStorage _bookStorage;
        [SerializeField]
        private Book _bookObject;
        
        private IStaticDataService _staticData;

        [Inject]
        private void Construct(IStaticDataService staticData) =>
            _staticData = staticData;

        private void Start()
        {
            _bookStorage.BooksUpdated += UpdateView;
            UpdateView();
        }

        private void OnDestroy() =>
            _bookStorage.BooksUpdated -= UpdateView;

        private void UpdateView()
        {
            SetMaterialIfAny();
            if(_bookStorage.HasBook)
                _bookObject.Show();
            else
                _bookObject.Hide();
        }

        private void SetMaterialIfAny()
        {
            Material targetMaterial = GetBookMaterial();
            if(targetMaterial is null)
                return;

            _bookObject.SetView(targetMaterial, GetBookIcon());
        }

        private Material GetBookMaterial()
        {
            string bookId = _bookStorage.CurrentBookId;
            if(string.IsNullOrWhiteSpace(bookId))
                return null;
            
            StaticBook data = _staticData.ForBook(bookId);
            return data.StaticBookType.Material;
        }
        
        private Sprite GetBookIcon()
        {
            string bookId = _bookStorage.CurrentBookId;
            if(string.IsNullOrWhiteSpace(bookId))
                return null;
            
            StaticBook data = _staticData.ForBook(bookId);
            return data.StaticBookType.Icon;
        }
    }
}