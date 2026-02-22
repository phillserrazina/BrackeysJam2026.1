using UnityEngine;
using UnityEngine.SceneManagement;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class DebugButtonsUI : MonoBehaviour
    {
        // METHODS
        public void PauseButton()
        {
            GameManager.Instance.Pause();
        }

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

        public void DeleteData()
        {
            PlayerSaveSystem.Delete();
            CollectionManager.Instance.Delete();
        }

        public void AddMoney(float value)
        {
            PlayerManager.Instance.Wallet.Add(CurrencyTypes.Gold, value);
        }
    }
}
