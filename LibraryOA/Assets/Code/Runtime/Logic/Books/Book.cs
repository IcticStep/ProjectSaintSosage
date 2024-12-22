using UnityEngine;
using UnityEngine.UI;

namespace Code.Runtime.Logic.Books
{
    public class Book : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private Image _icon;

        public void SetView(Material material, Sprite icon)
        {
            _meshRenderer.sharedMaterial = material;
            _icon.sprite = icon;
        }

        public void Show()
        {
            _meshRenderer.enabled = true;
            _icon.enabled = true;
        }

        public void Hide()
        {
            _meshRenderer.enabled = false;
            _icon.enabled = false;
        }
    }
}