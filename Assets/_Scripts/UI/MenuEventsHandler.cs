using UnityEngine;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class MenuEventsHandler : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private GameObject pauseMenu;

        private GameManager gameManager;

        // EXECUTION FUNCTIONS
        private void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.OnPauseStateChanged += GameManager_OnPauseStateChanged;
        }


        private void OnDestroy()
        {
            gameManager.OnPauseStateChanged -= GameManager_OnPauseStateChanged;
        }

        // CALLBACKS
        private void GameManager_OnPauseStateChanged(bool pauseState)
        {
            pauseMenu.SetActive(pauseState);
        }
    }
}
