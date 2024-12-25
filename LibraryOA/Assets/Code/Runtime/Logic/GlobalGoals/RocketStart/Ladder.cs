using DG.Tweening;
using UnityEngine;

namespace Code.Runtime.Logic.GlobalGoals.RocketStart
{
    internal sealed class Ladder : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _targetRotation;
        [SerializeField]
        private float _duration;
        [SerializeField]
        private Ease _ease;

        private Transform _transform;

        private void Awake() =>
            _transform = transform;

        public void Throw() =>
            _transform
                .DOLocalRotate(_targetRotation, _duration, RotateMode.LocalAxisAdd)
                .SetLink(gameObject)
                .SetEase(_ease);
    }
}