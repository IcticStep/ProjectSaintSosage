using Code.Editor.Editors;
using Code.Runtime.Infrastructure.Services.StaticData;
using Code.Runtime.StaticData;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Code.Editor.App
{
    [InitializeOnLoad]
    public class StaticDataAutoCollect
    {
        static StaticDataAutoCollect()
        {
            EditorSceneManager.sceneSaved += OnSceneSaved;
        }

        ~StaticDataAutoCollect()
        {
            EditorSceneManager.sceneSaved -= OnSceneSaved;
        }

        private static void OnSceneSaved(Scene scene)
        {
            IStaticDataService staticData = LoadLevelsData();
            LevelStaticData levelData = GetCurrentLevelData(staticData);
            if(levelData is null)
                return;
            
            LevelStaticDataEditor.UpdateLevelData(levelData);
        }

        private static LevelStaticData GetCurrentLevelData(IStaticDataService staticData)
        {
            string current = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = staticData.ForLevel(current);
            return levelData;
        }

        private static IStaticDataService LoadLevelsData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.LoadLevels();
            return staticData;
        }
    }
}