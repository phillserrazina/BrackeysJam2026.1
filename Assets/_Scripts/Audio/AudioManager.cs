using UnityEngine;

namespace FishingGame.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("SFX Clips")]
        public AudioClip moneyGain;
        public AudioClip menuOpen;
        public AudioClip menuClose;
        public AudioClip castStart;
        public AudioClip castEnd;
        public AudioClip bite;
        public AudioClip catchSuccess;
        public AudioClip catchFail;
        public AudioClip victory;
        public AudioClip upgradeBuy;
        public AudioClip collectFish;
        public AudioClip reelLoop;

        private AudioSource source;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            source = GetComponent<AudioSource>();
            if (source == null)
                source = gameObject.AddComponent<AudioSource>();

            // separate audio source for looping sounds (reel loop)
            loopSource = gameObject.AddComponent<AudioSource>();
            loopSource.loop = true;
            loopSource.playOnAwake = false;
        }

        // Explicit SFX methods to avoid string names
        public void PlayMoneyGain(float volume = 1f) => PlayClip(moneyGain, volume);
        public void PlayMenuOpen(float volume = 1f) => PlayClip(menuOpen, volume);
        public void PlayMenuClose(float volume = 1f) => PlayClip(menuClose, volume);
        public void PlayCastStart(float volume = 1f) => PlayClip(castStart, volume);
        public void PlayCastEnd(float volume = 1f) => PlayClip(castEnd, volume);
        public void PlayBite(float volume = 1f) => PlayClip(bite, volume);
        public void PlayCatchSuccess(float volume = 1f) => PlayClip(catchSuccess, volume);
        public void PlayCatchFail(float volume = 1f) => PlayClip(catchFail, volume);
        public void PlayVictory(float volume = 1f) => PlayClip(victory, volume);
        public void PlayUpgradeBuy(float volume = 1f) => PlayClip(upgradeBuy, volume);
        public void PlayCollectFish(float volume = 1f) => PlayClip(collectFish, volume);

        // Generic play utility
        private void PlayClip(AudioClip clip, float volume)
        {
            if (clip == null || source == null) return;
            source.PlayOneShot(clip, volume);
        }

        // Backwards-compatible string-based API (maps common names to clips)
        public void Play(string name, float volume = 1f)
        {
            if (string.IsNullOrEmpty(name)) return;

            switch (name)
            {
                case "money_gain": PlayMoneyGain(volume); break;
                case "menu_open": PlayMenuOpen(volume); break;
                case "menu_close": PlayMenuClose(volume); break;
                case "cast_start": PlayCastStart(volume); break;
                case "cast_end": PlayCastEnd(volume); break;
                case "bite": PlayBite(volume); break;
                case "catch_success": PlayCatchSuccess(volume); break;
                case "catch_fail": PlayCatchFail(volume); break;
                case "victory": PlayVictory(volume); break;
                case "upgrade_buy": PlayUpgradeBuy(volume); break;
                case "collect_fish": PlayCollectFish(volume); break;
                default:
                    Debug.LogWarning($"AudioManager::Play() - unknown SFX name '{name}'");
                    break;
            }
        }

        // Reel loop control
        private AudioSource loopSource;

        public void StartReelLoop(float volume = 1f)
        {
            if (reelLoop == null || loopSource == null) return;
            if (loopSource.clip == reelLoop && loopSource.isPlaying) return;
            loopSource.clip = reelLoop;
            loopSource.volume = volume;
            loopSource.Play();
        }

        public void StopReelLoop()
        {
            if (loopSource == null) return;
            loopSource.Stop();
            loopSource.clip = null;
        }
    }
}
