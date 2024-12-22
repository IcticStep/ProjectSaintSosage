using Code.Runtime.Services.Days;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Code.Runtime.Ui.HudComponents.MainPanel
{
    internal sealed class DaysView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private RectTransform _animationTarget;

        private IDaysService _daysService;
        private Tweener _tweener;

        [Inject]
        private void Construct(IDaysService daysService) =>
            _daysService = daysService;

        private void OnValidate() =>
            _animationTarget ??= GetComponent<RectTransform>();
        
        private void Start()
        {
            _daysService.Updated += UpdateView;
            UpdateView();
        }

        private void OnDestroy() =>
            _daysService.Updated -= UpdateView;

        private void UpdateView()
        {
            _text.text = _daysService.CurrentDay.ToString();
            if(_tweener != null)
            {
                _tweener.Kill(complete: true);
                _tweener = null;
            }
            _tweener = _animationTarget
                .DOPunchScale(Vector3.one * 1.1f, 0.5f, 1, 0.5f)
                .SetLink(gameObject);
        }
    }
}