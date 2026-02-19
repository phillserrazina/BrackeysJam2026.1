using UnityEngine;

namespace FishingGame.Gameplay.Systems
{
    public class CoroutineRunner : MonoBehaviour
    {
        // VARIABLES
        public static CoroutineRunner Instance { get; private set; }

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            Instance = this;
        }
    }
}
