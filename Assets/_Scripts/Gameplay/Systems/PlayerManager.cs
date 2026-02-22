using UnityEngine;
using UnityEngine.UI;
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
        private System.Collections.Generic.List<UpgradeSaveEntry> pendingUpgrades = new();

        public static PlayerManager Instance { get; private set; }

        public bool IsFrozen { get; private set; } = false;

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            Instance = this;

            // Initialize systems
            Wallet = new();
            Upgrades = new();

            // Load saved player data (gold + upgrades)
            var saved = PlayerSaveSystem.Load();

            if (saved != null)
            {
                if (saved.gold > 0f)
                    Wallet.Add(CurrencyTypes.Gold, saved.gold);

                // Store upgrade entries and apply when DataManager is available
                if (saved.upgrades != null)
                    pendingUpgrades = new System.Collections.Generic.List<UpgradeSaveEntry>(saved.upgrades);
            }

            // Apply debug upgrades (if any)
            foreach (var upgrade in debugUpgrades)
            {
                Upgrades.Increment(upgrade);
            }

            // Subscribe to changes so we save progress
            Wallet.OnWalletChanged += _ => SavePlayer();
            Upgrades.OnUpgradesChanged += SavePlayer;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;

            // Attempt to apply pending upgrades now that other Awakes have run
            ApplyPendingUpgrades();
        }

        private void OnApplicationQuit()
        {
            SavePlayer();
        }

        private void ApplyPendingUpgrades()
        {
            if (pendingUpgrades == null || pendingUpgrades.Count == 0)
                return;

            if (DataManager.Instance == null)
            {
                // Give up if DataManager still not present
                Debug.LogWarning("PlayerManager::ApplyPendingUpgrades() - DataManager not found, skipping applying saved upgrades.");
                return;
            }

            foreach (var entry in pendingUpgrades)
            {
                var cfg = DataManager.Instance.GetUpgradeByID(entry.id);
                if (cfg == null) continue;

                for (int i = 0; i < entry.level; i++)
                    Upgrades.Increment(cfg);
            }

            pendingUpgrades.Clear();
        }

        private void SavePlayer()
        {
            var data = new PlayerSaveData();
            data.gold = Wallet.Get(CurrencyTypes.Gold);

            var upgradesDict = Upgrades.GetAllUpgradeLevelsById();
            foreach (var kvp in upgradesDict)
            {
                data.upgrades.Add(new UpgradeSaveEntry { id = kvp.Key, level = kvp.Value });
            }

            data.Planet = LocationManager.Instance.CurrentLocation.Name;

            PlayerSaveSystem.Save(data);
        }

        // INPUT
        private void OnCast(InputValue input)
        {
            if (IsFrozen)
            {
                return;
            }

            fishingController.OnFishingInput(input.isPressed);
        }

        private void OnFish(InputValue input)
        {
            if (IsFrozen)
            {
                return;
            }

            fishingController.OnFishingInput(input.isPressed);
        }

        private void OnPause(InputValue input)
        {
            if (IsFrozen)
            {
                return;
            }

            GameManager.Instance.TriggerPause();
        }

        private void OnCollection(InputValue input)
        {
            if (IsFrozen)
            {
                return;
            }

            if (collectionMenu == null)
            {
                collectionMenu = FindFirstObjectByType<CollectionMenuUI>(FindObjectsInactive.Include);
            }

            collectionMenu.TriggerVisibility();
        }

        private void OnUpgrades(InputValue input)
        {
            if (IsFrozen)
            {
                return;
            }

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
                foreach (var button in FindObjectsByType<Button>(FindObjectsSortMode.None))
                {
                    button.enabled = true;
                }

                IsFrozen = false;
            });
        }

        public float GetUpgradeModifiedValue(UpgradeTypes upgradeType, float value)
        {
            return Upgrades.GetModifiedValue(upgradeType, value);
        }

        public void Freeze()
        {
            IsFrozen = true;
        }
    }
}