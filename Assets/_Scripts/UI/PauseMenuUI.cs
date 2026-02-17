using UnityEngine;
using UnityEngine.SceneManagement;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        // METHODS
        public void ResumeButton()
        {
            GameManager.Instance.Unpause();
        }

        public void QuitToMenuButton()
        {
            if (SceneLoader.Instance == null)
            {
                SceneManager.LoadScene("Main Menu");
                return;
            }

            SceneLoader.Instance.LoadScene("Main Menu");
        }

        public void QuitToDesktopButton()
        {
            Application.Quit();
        }
    }
}
