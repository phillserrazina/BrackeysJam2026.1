using UnityEngine;
using UnityEngine.InputSystem;

using FishingGame.UI;
using FishingGame.Data;

namespace FishingGame.Gameplay.Systems
{
	public class PlayerManager : MonoBehaviour
	{
        // VARIABLES
        [SerializeField] private PlayerFishingController fishingController;

        [Header("Debug")]
        [SerializeField] private UpgradeConfigSO[] debugUpgrades;

        public PlayerWallet Wallet { get; private set; }
        public PlayerUpgradesHandler Upgrades { get; private set; }
        private CollectionMenuUI collectionMenu;
        private UpgradesMenuUI upgradesMenu;

        public static PlayerManager Instance { get; private set; }

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            Instance = this;
            Wallet = new();
            Upgrades = new();

            foreach (var upgrade in debugUpgrades)
            {
                Upgrades.Increment(upgrade);
            }
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        // INPUT
        private void OnCast(InputValue input)
        {
            fishingController.OnFishingInput(input.isPressed);
        }

        private void OnFish(InputValue input)
        {
            fishingController.OnFishingInput(input.isPressed);
        }

        private void OnPause(InputValue input)
        {
            GameManager.Instance.TriggerPause();
        }

        private void OnCollection(InputValue input)
        {
            if (collectionMenu == null)
            {
                collectionMenu = FindFirstObjectByType<CollectionMenuUI>(FindObjectsInactive.Include);
            }

            collectionMenu.TriggerVisibility();
        }

        private void OnUpgrades(InputValue input)
        {
            if (upgradesMenu == null)
            {
                upgradesMenu = FindFirstObjectByType<UpgradesMenuUI>(FindObjectsInactive.Include);
            }

            upgradesMenu.TriggerVisibility();
        }

        // METHODS
        public void ChangeEnvironment(string levelName)
        {
            SceneLoader.Instance.LoadEnvironment(levelName, () =>
            {
                
            });
        }

        public float GetUpgradeModifiedValue(UpgradeTypes upgradeType, float value)
        {
            return Upgrades.GetModifiedValue(upgradeType, value);
        }
    }
}