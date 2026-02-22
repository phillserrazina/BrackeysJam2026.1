using UnityEngine;
using UnityEngine.SceneManagement;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class DebugButtonsUI : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private GameObject debugContents;

        // EXECUTION FUNCTIONS
        private void Update()
        {
            if (OptionsManager.Instance != null)
            {
                debugContents.SetActive(OptionsManager.Instance.DebugModeEnabled);
            }
        }

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
