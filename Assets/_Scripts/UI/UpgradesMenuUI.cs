using System.Collections.Generic;

using UnityEngine;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class UpgradesMenuUI : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private UpgradeElementUI upgradeElementPrefab;
        [SerializeField] private Transform layout;

        private readonly List<UpgradeElementUI> spawnedElements = new();

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            foreach (var upgrade in DataManager.Instance.AllUpgrades)
            {
                UpgradeElementUI spawnedElement = Instantiate(upgradeElementPrefab, layout);
                spawnedElement.Initialize(upgrade);
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
