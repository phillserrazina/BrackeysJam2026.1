using FishingGame.Gameplay.Systems;
using UnityEngine;

namespace FishingGame.UI
{
    public class IntroMenuUI : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private GameObject[] screens;

        private int currentScreenIndex = 0;

        // EXECUTION FUNCTIONS
        private void Start()
        {
            foreach (var screen in screens)
            {
                screen.SetActive(false);
            }

            screens[currentScreenIndex].SetActive(true);
        }

        // METHODS
        public void GoToNext()
        {
            currentScreenIndex++;

            if (currentScreenIndex >= screens.Length)
            {
                SceneLoader.Instance.LoadGameplayScene("Earth");
            }
            else
            {
                screens[currentScreenIndex - 1].SetActive(false);
                screens[currentScreenIndex].SetActive(true);
            }
        }
    }
}
