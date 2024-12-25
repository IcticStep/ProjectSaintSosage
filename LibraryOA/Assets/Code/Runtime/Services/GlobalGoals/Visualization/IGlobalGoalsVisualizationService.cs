using System.Collections.Generic;
using Code.Runtime.Infrastructure.DiInstallers.Library.GlobalGoals;
using Code.Runtime.Infrastructure.DiInstallers.Library.GlobalGoals.Data;
using Code.Runtime.StaticData.GlobalGoals;
using Cysharp.Threading.Tasks;

namespace Code.Runtime.Services.GlobalGoals.Visualization
{
    public interface IGlobalGoalsVisualizationService
    {
        bool InitializedGlobalGoal { get; }
        GlobalGoalScheme CurrentGoalScheme { get; }
        void InitializeVisualisationSchemes(IReadOnlyList<GlobalGoalScheme> allSchemes);
        void InitializeGlobalGoal(GlobalGoal globalGoal);
        void VisualizeStep(GlobalStep step);

        /// <summary>
        /// Visualize by zero-based index respectively to <see cref="GlobalStep"/> list order in <see cref="GlobalGoal"/>> scriptable object. 
        /// </summary>
        /// <param name="index"></param>
        void VisualizeStep(int index);

        void VisualizeStepAndAllBefore(int index);
        void VisualizeStepAndAllBefore(GlobalStep step);
        void ResetCurrentVisualization();
        void ResetAllVisualizations();
        UniTask PlayFinishCutscene();
        void ShowStartObjects();
    }
}