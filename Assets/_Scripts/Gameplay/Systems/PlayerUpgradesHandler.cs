using System;
using System.Collections.Generic;
using System.Linq;
using FishingGame.Data;

namespace FishingGame.Gameplay.Systems
{
    public class PlayerUpgradesHandler
    {
        // VARIABLES
        private readonly Dictionary<UpgradeConfigSO, int> upgradesDictionary = new();
        public event Action OnUpgradesChanged;
    
        // METHODS
        public void Increment(UpgradeConfigSO upgrade)
        {
            if (upgradesDictionary.ContainsKey(upgrade))
            {
                upgradesDictionary[upgrade]++;
            }
            else
            {
                upgradesDictionary.Add(upgrade, 1);
            }

            OnUpgradesChanged?.Invoke();
        }

        public int GetUpgradeLevel(UpgradeConfigSO upgrade) => upgradesDictionary.ContainsKey(upgrade) ? upgradesDictionary[upgrade] : 0;

        public void Clear()
        {
            upgradesDictionary.Clear();

            OnUpgradesChanged?.Invoke();
        }

        public float GetModifiedValue(UpgradeTypes upgradeType, float value)
        {
            float finalValue = value;

            foreach (var upgrade in upgradesDictionary.Keys.ToArray())
            {
                if (upgrade.Type == upgradeType)
                {
                    finalValue += upgrade.ValuePerLevel * upgradesDictionary[upgrade];
                }
            }

            return finalValue;
        }

        public Dictionary<string, int> GetAllUpgradeLevelsById()
        {
            var dict = new Dictionary<string, int>();

            foreach (var kvp in upgradesDictionary)
            {
                if (kvp.Key != null && !string.IsNullOrEmpty(kvp.Key.ObjectID))
                    dict[kvp.Key.ObjectID] = kvp.Value;
            }

            return dict;
        }
    }
}
