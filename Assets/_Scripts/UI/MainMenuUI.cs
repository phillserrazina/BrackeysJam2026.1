using UnityEngine;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        // METHODS
        public void PlayButton()
        {
            SceneLoader.Instance.LoadGameplayScene("Earth");
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
