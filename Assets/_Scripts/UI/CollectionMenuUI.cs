using System.Collections.Generic;

using UnityEngine;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class CollectionMenuUI : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private CollectionElementUI collectionElementPrefab;
        [SerializeField] private Transform layout;

        private readonly List<CollectionElementUI> spawnedElements = new();

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            foreach (var fish in DataManager.Instance.AllFishes)
            {
                CollectionElementUI spawnedElement = Instantiate(collectionElementPrefab, layout);
                spawnedElement.Initialize(fish);
                spawnedElements.Add(spawnedElement);
            }
        }

        // METHODS
        public void TriggerVisibility()
        {
            if (gameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void Show()
        {
            foreach (var element in spawnedElements)
            {
                element.UpdateState();
            }

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ExitButton()
        {
            gameObject.SetActive(false);
        }
    }
}
