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
        }

        public int GetUpgradeLevel(UpgradeConfigSO upgrade) => upgradesDictionary.ContainsKey(upgrade) ? upgradesDictionary[upgrade] : 0;

        public void Clear()
        {
            upgradesDictionary.Clear();
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
    }
}
