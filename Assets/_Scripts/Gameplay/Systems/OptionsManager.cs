using UnityEngine;
using UnityEngine.Audio;

namespace FishingGame.Gameplay.Systems
{
    public class OptionsManager : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private AudioMixer mixer;

        public float CurrentVolume { get; private set; } = 0f;
        public bool DebugModeEnabled { get; private set; } = false;

        public static OptionsManager Instance { get; private set; }

        private const string VOLUME_KEY = "Volume";
        private const string DEBUG_KEY = "DebugMode";

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
            }
        }

        private void Start()
        {
            CurrentVolume = PlayerPrefs.GetFloat(VOLUME_KEY, -20f);
            mixer.SetFloat(VOLUME_KEY, CurrentVolume);

            DebugModeEnabled = PlayerPrefs.GetInt(DEBUG_KEY, 0) == 1;
        }

        // METHODS
        public void SetMixerVolume(float volume)
        {
            CurrentVolume = volume;
            mixer.SetFloat(VOLUME_KEY, volume);
            PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        }

        public void SetDebugModeEnabled(bool active)
        {
            DebugModeEnabled = active;
            int saveValue = active ? 1 : 0;
            PlayerPrefs.SetInt(DEBUG_KEY, saveValue);
        }
    }
}
