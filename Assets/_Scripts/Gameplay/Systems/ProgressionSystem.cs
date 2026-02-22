using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using DG.Tweening;

using FishingGame.Data;
using UnityEngine.SceneManagement;

namespace FishingGame.Gameplay.Systems
{
    public class ProgressionSystem : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private List<ProgressionStep> steps = new();

        [Header("UI")]
        [SerializeField] private TMP_Text progressionText;

        private CollectionManager collectionManager;

        // EXECUTION FUNCTIONS
        private void Start()
        {
            collectionManager = CollectionManager.Instance;
            collectionManager.OnFishDiscovered += CollectionManager_OnFishDiscovered;
        }

        private void OnDestroy()
        {
            collectionManager.OnFishDiscovered -= CollectionManager_OnFishDiscovered;
        }

        // CALLBACKS
        private void CollectionManager_OnFishDiscovered(FishConfigSO discoveredFish)
        {
            ProgressionStep currentStep = steps.FirstOrDefault(step => step.Planet == LocationManager.Instance.CurrentLocation);

            bool readyForNextLevel = true;

            foreach (var fish in currentStep.Fish)
            {
                if (!CollectionManager.Instance.IsCollected(fish))
                {
                    readyForNextLevel = false;
                }
            }

            if (readyForNextLevel)
            {
                PlayerManager.Instance.Freeze();
                int nextIndex = steps.IndexOf(currentStep) + 1;

                if (nextIndex >= steps.Count)
                {
                    progressionText.text = "My Children... they have all been found...\nthank you... Fisherman...";
                    progressionText.gameObject.SetActive(true);

                    DOVirtual.DelayedCall(5f, () =>
                    {
                        if (SceneLoader.Instance == null)
                        {
                            SceneManager.LoadScene("Win Menu");
                            return;
                        }

                        SceneLoader.Instance.LoadScene("Win Menu");
                    });

                    return;
                }

                foreach (var button in FindObjectsByType<Button>(FindObjectsSortMode.None))
                {
                    button.enabled = false;
                }

                progressionText.text = $"My Children in {currentStep.Planet.Name} have been found... \r\nthank you... {steps[nextIndex].Planet.Name} awaits us...";
                progressionText.gameObject.SetActive(true);

                DOVirtual.DelayedCall(5f, () =>
                {
                    progressionText.gameObject.SetActive(false);
                    PlayerManager.Instance.ChangeEnvironment(steps[nextIndex].SceneName);
                });
            }
        }

        // HELPER CLASSES
        [System.Serializable]
        public class ProgressionStep
        {
            public PlanetConfigSO Planet;
            public FishConfigSO[] Fish;
            public string SceneName;
        }
    }
}
