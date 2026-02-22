using UnityEngine;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        // METHODS
        public void PlayButton()
        {
            if (PlayerSaveSystem.Exists())
            {
                try
                {
                    string sceneName = PlayerSaveSystem.Load().Planet;
                    SceneLoader.Instance.LoadGameplayScene(string.IsNullOrEmpty(sceneName) ? "Earth" : sceneName);
                }
                catch
                {
                    SceneLoader.Instance.LoadGameplayScene("Earth");
                }
            }
            else
            {
                SceneLoader.Instance.LoadScene("Intro");
            }
        }

        public void GoToScene(string sceneName)
        {
            SceneLoader.Instance.LoadScene(sceneName);
        }

        public void ExitButton()
        {
            Application.Quit();
        }
    }
}
