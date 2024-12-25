using System.Collections.Generic;
using System.Linq;
using Code.Runtime.Infrastructure.DiInstallers.Library.GlobalGoals.Data;
using Code.Runtime.Logic.GlobalGoals;
using Code.Runtime.StaticData.GlobalGoals;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Runtime.Services.GlobalGoals.Visualization
{
    [UsedImplicitly]
    public sealed class GlobalGoalsVisualizationService : IGlobalGoalsVisualizationService
    {
        private Dictionary<GlobalGoal, GlobalGoalScheme> _goalSchemes;
        private Dictionary<GlobalStep, GlobalStepScheme> _currentGoalStepsSchemes;
        private GlobalGoal _currentGoal;

        public bool InitializedGlobalGoal => _currentGoal is not null;

        private IEnumerable<GameObject> AllStartObjects => _goalSchemes.Values.SelectMany(x => x.OnStartObjects);

        public GlobalGoalScheme CurrentGoalScheme => _goalSchemes.GetValueOrDefault(_currentGoal);

        public void InitializeVisualisationSchemes(IReadOnlyList<GlobalGoalScheme> allSchemes)
        {
            _goalSchemes = allSchemes.ToDictionary(scheme => scheme.Goal, scheme => scheme);
            _currentGoalStepsSchemes = null;
        }

        public void InitializeGlobalGoal(GlobalGoal globalGoal) =>
            _currentGoal = globalGoal;

        public void VisualizeStep(GlobalStep step)
        {
            GlobalStepScheme stepScheme = GetCurrentGoalStepScheme(step);
            VisualizeStepScheme(stepScheme);
        }

        /// <summary>
        /// Visualize by zero-based index respectively to <see cref="GlobalStep"/> list order in <see cref="GlobalGoal"/>> scriptable object. 
        /// </summary>
        /// <param name="index"></param>
        public void VisualizeStep(int index)
        {
            ShowStartObjects();
            GlobalStep step = _currentGoal.GlobalSteps[index];
            GlobalStepScheme stepScheme = GetCurrentGoalStepScheme(step);
            VisualizeStepScheme(stepScheme);
        }

        public void VisualizeStepAndAllBefore(int index)
        {
            ShowStartObjects();
            GlobalStep step = _currentGoal.GlobalSteps[index];
            VisualizeStepAndAllBefore(step);
        }

        public void VisualizeStepAndAllBefore(GlobalStep step)
        {
            ShowStartObjects();
            foreach(GlobalStepScheme stepScheme in CurrentGoalScheme.GlobalStepsSchemes)
            {
                VisualizeStepScheme(stepScheme);
                if(stepScheme.Step == step)
                    return;
            }
        }

        public void ResetCurrentVisualization() =>
            ResetVisualization(CurrentGoalScheme);

        public void ResetAllVisualizations()
        {
            foreach(GlobalGoalScheme goalScheme in _goalSchemes.Values)
            {
                HideStartObjects(goalScheme);
                ResetVisualization(goalScheme);
            }                
        }

        public UniTask PlayFinishCutscene() =>
            CurrentGoalScheme.Director.PlayFinishCutscene();

        private static void VisualizeStepScheme(GlobalStepScheme stepScheme)
        {
            foreach(GlobalStepPartVisualizer visualizer in stepScheme.Visualizers)
                visualizer.Visualize();
        }

        private static void ResetVisualization(GlobalGoalScheme goalScheme)
        {
            foreach(GlobalStepScheme globalStepsScheme in goalScheme.GlobalStepsSchemes)
                foreach(GlobalStepPartVisualizer visualizer in globalStepsScheme.Visualizers)
                    visualizer.Reset();
        }

        private GlobalStepScheme GetCurrentGoalStepScheme(GlobalStep globalStep)
        {
            InitializeCurrentGoalStepsSchemesIfNone();
            return _currentGoalStepsSchemes[globalStep];
        }

        private void InitializeCurrentGoalStepsSchemesIfNone() =>
            _currentGoalStepsSchemes ??= CurrentGoalScheme
                .GlobalStepsSchemes
                .ToDictionary(stepScheme => stepScheme.Step, stepScheme => stepScheme);

        public void ShowStartObjects()
        {
            foreach(GameObject startObject in AllStartObjects)
                startObject.SetActive(false);

            foreach(GameObject onStartObject in CurrentGoalScheme.OnStartObjects)
                onStartObject.SetActive(true);
        }

        private void HideStartObjects(GlobalGoalScheme goalScheme)
        {
            foreach(GameObject onStartObject in goalScheme.OnStartObjects)
                onStartObject.SetActive(false);
        }
    }
}