using UnityEngine;
using UnityEngine.SceneManagement;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class DebugButtonsUI : MonoBehaviour
    {
        // METHODS
        public void LoadScene(string sceneName)
        {
            if (SceneLoader.Instance == null)
            {
                SceneManager.LoadScene(sceneName);
                return;
            }

            SceneLoader.Instance.LoadScene(sceneName);
        }

        public void LoadEnvironment(string envName)
        {
            PlayerManager.Instance.ChangeEnvironment(envName);
        }
    }
}
