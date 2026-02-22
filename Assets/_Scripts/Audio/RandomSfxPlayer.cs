using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingGame.Audio
{
    /// <summary>
    /// Plays random AudioClips from a user-provided list at random intervals.
    /// Configure the clip list and min/max interval in the inspector and call
    /// StartPlaying() / StopPlaying() or enable PlayOnStart.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class RandomSfxPlayer : MonoBehaviour
    {
        [Header("Clips")]
        [Tooltip("List of audio clips to pick from")]
        public List<AudioClip> clips = new();

        [Header("Timing")]
        [Tooltip("Minimum time (seconds) between clips")]
        public float minInterval = 5f;
        [Tooltip("Maximum time (seconds) between clips")]
        public float maxInterval = 15f;

        [Header("Playback")]
        [Range(0f, 1f)] public float volume = 1f;
        [Tooltip("If true, will start automatically on Awake")]
        public bool playOnStart = true;
        [Tooltip("If true, randomize pitch per clip")]
        public bool randomizePitch = false;
        [Tooltip("Pitch range when randomizePitch is true")]
        public Vector2 pitchRange = new Vector2(0.95f, 1.05f);

        private AudioSource source;
        private Coroutine playRoutine;

        [Header("Overlap")]
        [Tooltip("Maximum simultaneous clips that can overlap")]
        [Range(1, 8)]
        public int maxSimultaneous = 3;

        // internal pool of audio sources used to limit simultaneous sounds
        private List<AudioSource> playPool;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
            
            // create pool
            playPool = new List<AudioSource>(maxSimultaneous);
            // use the primary audio source as first in pool
            playPool.Add(source);

            for (int i = 1; i < maxSimultaneous; i++)
            {
                var s = gameObject.AddComponent<AudioSource>();
                s.playOnAwake = false;
                s.spatialBlend = source.spatialBlend;
                s.outputAudioMixerGroup = source.outputAudioMixerGroup;
                playPool.Add(s);
            }
        }

        private void Start()
        {
            if (playOnStart)
                StartPlaying();
        }

        /// <summary>
        /// Start the random playback loop.
        /// </summary>
        public void StartPlaying()
        {
            if (playRoutine != null) return;
            playRoutine = StartCoroutine(RandomPlayLoop());
        }

        /// <summary>
        /// Stop the random playback loop.
        /// </summary>
        public void StopPlaying()
        {
            if (playRoutine == null) return;
            StopCoroutine(playRoutine);
            playRoutine = null;
            // stop any currently playing clips in the pool
            if (playPool != null)
            {
                foreach (var s in playPool)
                {
                    if (s != null) s.Stop();
                }
            }
        }

        private IEnumerator RandomPlayLoop()
        {
            // Basic validation clamp
            if (minInterval < 0f) minInterval = 0f;
            if (maxInterval < minInterval) maxInterval = minInterval;

            while (true)
            {
                float wait = Random.Range(minInterval, maxInterval);
                yield return new WaitForSeconds(wait);

                if (clips == null || clips.Count == 0) continue;

                // pick random non-null clip
                AudioClip clip = null;
                int attempts = 0;
                while (clip == null && attempts < clips.Count)
                {
                    clip = clips[Random.Range(0, clips.Count)];
                    attempts++;
                }

                if (clip == null) continue;

                PlayOnPool(clip, volume);
            }
        }

        private void PlayOnPool(AudioClip clip, float volume)
        {
            if (clip == null || playPool == null || playPool.Count == 0) return;

            // find a free source
            AudioSource free = null;
            foreach (var s in playPool)
            {
                if (!s.isPlaying)
                {
                    free = s;
                    break;
                }
            }

            if (free == null)
            {
                // all busy, skip this play to keep max simultaneous limit
                return;
            }

            float oldPitch = free.pitch;
            if (randomizePitch)
            {
                free.pitch = Random.Range(pitchRange.x, pitchRange.y);
            }

            free.PlayOneShot(clip, volume);

            // restore pitch immediately (the played clip retains the pitch)
            if (randomizePitch)
                free.pitch = oldPitch;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (minInterval < 0f) minInterval = 0f;
            if (maxInterval < minInterval) maxInterval = minInterval;
            if (volume < 0f) volume = 0f;
            if (volume > 1f) volume = 1f;
            if (maxSimultaneous < 1) maxSimultaneous = 1;
        }
#endif
    }
}
