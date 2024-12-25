using System.Collections.Generic;
using System.Linq;
using Code.Runtime.Infrastructure.DiInstallers.Library.GlobalGoals;
using Code.Runtime.Infrastructure.DiInstallers.Library.GlobalGoals.Data;
using Code.Runtime.Infrastructure.Services.StaticData;
using Code.Runtime.Services.GlobalGoals.Visualization;
using Code.Runtime.StaticData.GlobalGoals;
using UnityEditor;

namespace Code.Editor.Editors.DiInstallers.GlobalGoals
{
    public class GlobalGoalEditorVisualizer
    {
        private int _testVisualizationGoalIndex;
        private bool _testVisualizationToggle;
        private int _selectedVisualizationStepIndex;
        private Dictionary<string, GlobalGoal> _globalGoalsByNames;
        private string[] _globalGoalsNames;
        private IGlobalGoalsVisualizationService _globalGoalsVisualizationService;

        public void UpdateData(GlobalGoalsInstaller globalGoalsInstaller)
        {
            _globalGoalsByNames = GetGlobalGoalsByNames();
            _globalGoalsNames = _globalGoalsByNames
                .Values
                .Select(goal => goal.name)
                .ToArray();

            _globalGoalsVisualizationService = new GlobalGoalsVisualizationService();
            _globalGoalsVisualizationService.InitializeVisualisationSchemes(globalGoalsInstaller.GlobalGoalsVisualizationSchemes);
        }

        public void DrawTestVisualizationUi(GlobalGoalsInstaller globalGoalsInstaller)
        {
            using EditorGUILayout.ToggleGroupScope scope = new("Editor Visualization", _testVisualizationToggle);
            bool oldVisualizationToggleValue = _testVisualizationToggle;
            _testVisualizationToggle = scope.enabled;
            
            if(!_testVisualizationToggle)
                return;
            
            bool visualisationToggleChanged = _testVisualizationToggle != oldVisualizationToggleValue;
            if(visualisationToggleChanged)
                UpdateData(globalGoalsInstaller);
            
            InitDataIfNone(globalGoalsInstaller);
            DrawGlobalGoalPopup(out bool globalGoalChanged);
            if(globalGoalChanged || !_globalGoalsVisualizationService.InitializedGlobalGoal)
            {
                ReinitializeVisualization();
                _globalGoalsVisualizationService.ShowStartObjects();
            }

            DrawVisualizationSlider(globalGoalsInstaller, out bool visualizationStepChanged);
            if(visualizationStepChanged)
                VisualizeSelectedStepAndAllBefore();
        }

        private void InitDataIfNone(GlobalGoalsInstaller globalGoalsInstaller)
        {
            if(_globalGoalsNames is null)
                UpdateData(globalGoalsInstaller);
        }

        private void DrawGlobalGoalPopup(out bool changed)
        {
            int previousValue = _testVisualizationGoalIndex;
            _testVisualizationGoalIndex = EditorGUILayout.Popup(_testVisualizationGoalIndex, _globalGoalsNames);
            changed = previousValue != _testVisualizationGoalIndex;
        }

        private void ReinitializeVisualization()
        {
            _selectedVisualizationStepIndex = 0;
            _globalGoalsVisualizationService.ResetAllVisualizations();
            _globalGoalsVisualizationService.InitializeGlobalGoal(GetSelectedGoal());
        }

        private void DrawVisualizationSlider(GlobalGoalsInstaller globalGoalsInstaller, out bool changed)
        {
            GlobalGoal selectedGoal = GetSelectedGoal();
            GlobalGoalScheme selectedGoalScheme = GetSelectedGoalScheme(globalGoalsInstaller, selectedGoal);
            int stepsCount = selectedGoalScheme.GlobalStepsSchemes.Count;

            int oldValue = _selectedVisualizationStepIndex;
            _selectedVisualizationStepIndex = EditorGUILayout.IntSlider(_selectedVisualizationStepIndex, 0, stepsCount);
            changed = oldValue != _selectedVisualizationStepIndex;
        }

        private void VisualizeSelectedStepAndAllBefore()
        {
            _globalGoalsVisualizationService.ResetCurrentVisualization();
            if(_selectedVisualizationStepIndex == 0)
                return;
            
            _globalGoalsVisualizationService.VisualizeStepAndAllBefore(_selectedVisualizationStepIndex - 1);
        }

        private GlobalGoal GetSelectedGoal()
        {
            string selectedName = _globalGoalsNames[_testVisualizationGoalIndex];
            GlobalGoal selectedGoal = _globalGoalsByNames[selectedName];
            return selectedGoal;
        }

        private static GlobalGoalScheme GetSelectedGoalScheme(GlobalGoalsInstaller globalGoalsInstaller, GlobalGoal selectedGoal) =>
            globalGoalsInstaller
                .GlobalGoalsVisualizationSchemes
                .First(scheme => scheme.Goal == selectedGoal);

        private Dictionary<string, GlobalGoal> GetGlobalGoalsByNames()
        {
            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.LoadGlobalGoals();
            
            return staticDataService
                .GlobalGoals
                .ToDictionary(
                    goal => goal.name, 
                    goal => goal);
        }
    }
}