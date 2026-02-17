using FishingGame.Data;
using UnityEngine;

namespace FishingGame.Gameplay.Systems
{
    public class LocationManager : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private PlanetConfigSO planet;
        public PlanetConfigSO Planet => planet;

        public static LocationManager Instance { get; private set; }

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            Instance = this;
        }
    }
}
