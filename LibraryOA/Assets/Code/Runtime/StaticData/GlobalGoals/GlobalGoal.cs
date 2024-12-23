using System;
using System.Collections.Generic;
using Code.Runtime.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace Code.Runtime.StaticData.GlobalGoals
{
    [CreateAssetMenu(fileName = "Global Goal", menuName = "Static data/Global Goals/Global Goal", order = 0)]
    public class GlobalGoal : ScriptableObject
    {
        [ReadOnly]
        [SerializeField]
        private string _uniqueId = Guid.NewGuid().ToString();
        
        [SerializeField]
        private List<GlobalStep> _globalSteps = new();

        [SerializeField]
        private Sprite _icon;

        [SerializeField]
        private float _cameraMoveDuration;

        [FormerlySerializedAs("_cameraDelay")]
        [SerializeField]
        private float _cameraLookAtStepCompletedDelay;

        [field: SerializeField]
        public LocalizedString LocalizedName { get; private set; }

        public IReadOnlyList<GlobalStep> GlobalSteps => _globalSteps;
        public string UniqueId => _uniqueId;
        public Sprite Icon => _icon;
        public float CameraMoveDuration => _cameraMoveDuration;
        public float CameraLookAtStepCompletedDelay => _cameraLookAtStepCompletedDelay;

        private void OnValidate() =>
            _uniqueId = UniqueId ?? Guid.NewGuid().ToString();
    }
}