using System.Linq;

using UnityEngine;

using FishingGame.Data;
using System.Collections.Generic;

namespace FishingGame.Gameplay.Systems
{
    public class DataManager : MonoBehaviour
    {
        // VARIABLES
        [Header("Debug (Don't Add Anything Here)")]
        [SerializeField] private FishConfigSO[] fishArray;
        [SerializeField] private PlanetConfigSO[] planetArray;
        [SerializeField] private UpgradeConfigSO[] upgradesArray;

        public FishConfigSO[] AllFishes => fishArray;
        public UpgradeConfigSO[] AllUpgrades => upgradesArray;

        public static DataManager Instance { get; private set; }

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            fishArray = Resources.LoadAll("Scriptable Objects/Fishes").Cast<FishConfigSO>().ToArray();
            planetArray = Resources.LoadAll("Scriptable Objects/Planets").Cast<PlanetConfigSO>().ToArray();
            upgradesArray = Resources.LoadAll("Scriptable Objects/Upgrades").Cast<UpgradeConfigSO>().ToArray();
        }

        // METHODS
        public FishConfigSO GetFishByID(string id) => fishArray.FirstOrDefault(fish => fish.ObjectID == id);
        public PlanetConfigSO GetPlanetByID(string id) => planetArray.FirstOrDefault(planet => planet.ObjectID == id);
        public UpgradeConfigSO GetUpgradeByID(string id) => upgradesArray.FirstOrDefault(upgrade => upgrade.ObjectID == id);

        public FishConfigSO GetRandomFishData() => fishArray[Random.Range(0, fishArray.Length)];
        public FishConfigSO GetRandomFishData(PlanetConfigSO location, float luckScore)
        {
            Rarities randomRarity = GetRandomRarity(luckScore, location, new()
            {
                new RarityConfig { Rarity = Rarities.Common,    BaseWeight = 10,   ScalingFactor = 1 },
                new RarityConfig { Rarity = Rarities.Uncommon,  BaseWeight = 5,    ScalingFactor = 3  },
                new RarityConfig { Rarity = Rarities.Rare,      BaseWeight = 1,    ScalingFactor = 5  },
                new RarityConfig { Rarity = Rarities.VeryRare,  BaseWeight = 0.5f,    ScalingFactor = 7  },
                new RarityConfig { Rarity = Rarities.Legendary, BaseWeight = 0.1f,    ScalingFactor = 9  },
                new RarityConfig { Rarity = Rarities.Cosmic,    BaseWeight = 0.001f,    ScalingFactor = 10  }
            });

            var matchingRarityFishes = location.Fishes.Where(fish => fish.Rarity == randomRarity).ToArray();
            Debug.Log($"Rarity: {randomRarity}; Matches: {matchingRarityFishes.Length}");

            return matchingRarityFishes[Random.Range(0, matchingRarityFishes.Length)];
        }

        private Rarities GetRandomRarity(float luckScore, PlanetConfigSO location, List<RarityConfig> rarityConfigs)
        {
            var pool = new List<(RarityConfig rarity, float weight)>();
            float total = 0;

            foreach (var rarityConfig in rarityConfigs)
            {
                float weight = rarityConfig.BaseWeight + Mathf.Max(0, (luckScore - location.LuckModifier) * rarityConfig.ScalingFactor);
                pool.Add((rarityConfig, weight));
                total += weight;
            }

            float roll = Random.value * total;

            foreach (var (rarity, weight) in pool)
            {
                roll -= weight;
                if (roll <= 0)
                {
                    return rarity.Rarity;
                }
            }

            return Rarities.Common;
        }

        [System.Serializable]
        public class RarityConfig
        {
            // VARIABLES
            public Rarities Rarity;
            public float BaseWeight = 0f;
            public float ScalingFactor = 1f;
        }
    }
}