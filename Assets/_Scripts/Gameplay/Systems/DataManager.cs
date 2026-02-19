using System.Linq;

using UnityEngine;

using FishingGame.Data;

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
        public FishConfigSO GetRandomFishData(PlanetConfigSO location) => location.Fishes[Random.Range(0, location.Fishes.Length)];

    }
}