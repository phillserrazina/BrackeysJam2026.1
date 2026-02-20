#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace FishingGame.Editor
{
	public class SceneUtilities : MonoBehaviour 
	{
		// METHODS
		[MenuItem("Scene/Open/Main Menu", priority = 0)]
		static void OpenMainMenuScene()
		{
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
			    EditorSceneManager.OpenScene("Assets/_Scenes/Main Menu.unity");
            }
		}

        [MenuItem("Scene/Open/Earth", priority = 101)]
        static void OpenEarth()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/_Scenes/Earth.unity");
                LoadLogicScenes();
            }
        }

        [MenuItem("Scene/Open/Laeto", priority = 102)]
        static void OpenLaeto()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/_Scenes/Laeto.unity");
                LoadLogicScenes();
            }
        }

        [MenuItem("Scene/Open/Cirrus", priority = 103)]
        static void OpenCirrus()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/_Scenes/Cirrus.unity");
                LoadLogicScenes();
            }
        }

        [MenuItem("Scene/Open/Insolitum", priority = 103)]
        static void OpenInsolitum()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/_Scenes/Insolitum.unity");
                LoadLogicScenes();
            }
        }

        [MenuItem("Scene/Open/Load Logic Scenes", priority = 205)]
        static void LoadLogicScenes()
        {
            EditorSceneManager.OpenScene("Assets/_Scenes/Gameplay.unity", OpenSceneMode.Additive);
            EditorSceneManager.OpenScene("Assets/_Scenes/UI.unity", OpenSceneMode.Additive);
        }

        static void CloseScene(string sceneName)
        {
            var targetScene = EditorSceneManager.GetSceneByName(sceneName);

            if (targetScene != null)
            {
                EditorSceneManager.CloseScene(targetScene, true);
            }
        }
    }
}
#endif