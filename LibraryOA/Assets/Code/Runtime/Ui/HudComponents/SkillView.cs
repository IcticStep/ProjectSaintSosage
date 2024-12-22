using System.Linq;
using Code.Runtime.Infrastructure.Services.StaticData;
using Code.Runtime.Services.Skills;
using Code.Runtime.StaticData.Books;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Code.Runtime.Ui.HudComponents
{
    public sealed class SkillView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private Image _iconImage;
        [SerializeField]
        private BookType _bookType;
        [FormerlySerializedAs("_rectTransform")]
        [SerializeField]
        private RectTransform _animationTarget;
        [SerializeField]
        private Image _borderImage;
        
        private IPlayerSkillService _playerSkillService;
        private IStaticDataService _staticDataService;
        private StaticBookType _staticBookType;
        private int _skillValue;
        private Tweener _tweener;

        public BookType BookType => _bookType;
        public TextMeshProUGUI Text => _text;
        public Image IconImage => _iconImage;
        public Image BorderImage => _borderImage;

        [Inject]
        private void Construct(IPlayerSkillService playerSkillService, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _playerSkillService = playerSkillService;
        }
        
        private void OnValidate() =>
            _text ??= GetComponentInChildren<TextMeshProUGUI>();

        private void Awake() =>
            _staticBookType = _staticDataService.BookTypes.First(type => type.BookType == _bookType);

        private void Start()
        {
            _playerSkillService.Updated += UpdateView;
            UpdateView();
        }

        private void OnDestroy() =>
            _playerSkillService.Updated -= UpdateView;

        private void UpdateView()
        {
            int skill = _playerSkillService.GetSkillByBookType(_bookType); 
            string textText = skill.ToString();
            _text.text = textText;
            _iconImage.sprite = _staticBookType.Icon;

            PlayAnimationIfChanged(skill);
        }

        private void PlayAnimationIfChanged(int skill)
        {
            if(_skillValue == skill)
                return;

            if(_tweener != null)
            {
                _tweener.Kill(complete: true);
                _tweener = null;
            }
            
            _tweener = _animationTarget
                .DOPunchScale(Vector3.one * 1.1f, 0.5f, 1, 0.5f)
                .SetLink(gameObject);
            _skillValue = skill;
        }
    }
}