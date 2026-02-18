using System.Linq;

using UnityEngine;

using FishingGame.Data;
using System.Collections.Generic;

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
            
            if (discoveredFish == currentStep.Fish)
            {
                int nextIndex = steps.IndexOf(currentStep) + 1;

                if (nextIndex >= steps.Count)
                {
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
            public FishConfigSO Fish;
            public string SceneName;
        }
    }
}
