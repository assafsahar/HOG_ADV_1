using HOG.Core;
using UnityEngine;

namespace HOG.GameLogic
{
    public class HOGSoundManager : HOGMonoBehaviour
    {
        [SerializeField] AudioSource musicAudioSource;
        [SerializeField] AudioSource sfxAudioSource;
        static public HOGSoundManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        public void PlayBackgroundMusic()
        {
            if (musicAudioSource != null)
            {
                musicAudioSource.Play();
            }
        }

        public void StopBackgroundMusic()
        {
            if (musicAudioSource != null)
            {
                musicAudioSource.Stop();
            }
        }

    }
}
