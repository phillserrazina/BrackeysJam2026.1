using System;

using UnityEngine;

namespace FishingGame.Gameplay.Systems
{
    public class GameManager : MonoBehaviour
    {
        // VARIABLES
        public static GameManager Instance { get; private set; }

        public bool IsPaused { get; private set; } = false;

        public event Action<bool> OnPauseStateChanged;

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            Instance = this;
        }

        // METHODS
        public void TriggerPause()
        {
            if (IsPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
        
        public void Pause()
        {
            IsPaused = true;
            OnPauseStateChanged?.Invoke(true);
        }

        public void Unpause()
        {
            IsPaused = false;
            OnPauseStateChanged?.Invoke(false);
        }
    }
}
