using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using FishingGame.Data;

namespace FishingGame.Gameplay.Systems
{
    public class ProgressionSystem : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private List<ProgressionStep> steps = new();

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
                int nextIndex = steps.IndexOf(currentStep) + 1;

                if (nextIndex >= steps.Count)
                {
                    Debug.Log("Game Won!!");
                    return;
                }

                PlayerManager.Instance.ChangeEnvironment(steps[nextIndex].SceneName);
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
