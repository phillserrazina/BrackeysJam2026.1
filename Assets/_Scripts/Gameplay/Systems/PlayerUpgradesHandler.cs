using System.Collections.Generic;

using FishingGame.Data;

namespace FishingGame.Gameplay.Systems
{
    public class PlayerUpgradesHandler
    {
        // VARIABLES
        private readonly List<UpgradeConfigSO> ownedUpgrades = new();
    
        // METHODS
        public void Add(UpgradeConfigSO upgrade)
        {
            ownedUpgrades.Add(upgrade);
        }

        public bool Has(UpgradeConfigSO upgrade)
        {
            return ownedUpgrades.Contains(upgrade);
        }

        public void Clear()
        {
            ownedUpgrades.Clear();
        }

        public float GetModifiedValue(UpgradeTypes upgradeType, float value)
        {
            float finalValue = value;

            foreach (var upgrade in ownedUpgrades)
            {
                foreach (var attribute in upgrade.Attributes)
                {
                    if (attribute.Type == upgradeType)
                    {
                        finalValue += attribute.Value;
                    }
                }
            }

            return finalValue;
        }
    }
}
