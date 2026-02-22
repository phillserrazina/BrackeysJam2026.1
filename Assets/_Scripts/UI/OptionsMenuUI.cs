using UnityEngine;
using UnityEngine.UI;

using FishingGame.Gameplay.Systems;

namespace FishingGame.UI
{
    public class OptionsMenuUI : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Toggle debugToggle;

        // EXECUTION FUNCTIONS
        private void OnEnable()
        {
            volumeSlider.SetValueWithoutNotify(OptionsManager.Instance.CurrentVolume);
            debugToggle.SetIsOnWithoutNotify(OptionsManager.Instance.DebugModeEnabled);
        }

        // METHODS
        public void SetVolumeSlider(float volume)
        {
            OptionsManager.Instance.SetMixerVolume(volume);
        }

        public void SetDebugToggle(bool value)
        {
            OptionsManager.Instance.SetDebugModeEnabled(value);
        }
    }
}
