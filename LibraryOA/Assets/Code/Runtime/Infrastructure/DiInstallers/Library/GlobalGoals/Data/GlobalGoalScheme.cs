using System;
using System.Collections.Generic;
using Code.Runtime.Logic.GlobalGoals;
using Code.Runtime.StaticData.GlobalGoals;
using UnityEngine;

namespace Code.Runtime.Infrastructure.DiInstallers.Library.GlobalGoals.Data
{
    [Serializable]
    public sealed class GlobalGoalScheme
    {
        [SerializeField]
        private GlobalGoal _globalGoal;

        [SerializeField]
        private List<GlobalStepScheme> _globalStepsSchemes;
        
        [SerializeField]
        private GlobalGoalDirector _director;
        
        [SerializeField]
        private List<GameObject> _onStartObjects;

        public GlobalGoal Goal => _globalGoal;
        public IReadOnlyList<GlobalStepScheme> GlobalStepsSchemes => _globalStepsSchemes;
        public GlobalGoalDirector Director => _director;
        public List<GameObject> OnStartObjects => _onStartObjects;

        public GlobalGoalScheme(GlobalGoal globalGoal, List<GlobalStepScheme> globalStepsSchemes, GlobalGoalDirector director, List<GameObject> onStartObjects)
        {
            _globalGoal = globalGoal;
            _globalStepsSchemes = globalStepsSchemes;
            _director = director;
            _onStartObjects = onStartObjects;
        }

        public override string ToString() =>
            $"{nameof(GlobalGoalScheme)} for goal {_globalGoal.name}";
    }
}