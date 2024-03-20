using UnityEngine.Audio;
using UnityEngine;
using Codice.Client.Common.GameUI;

namespace RogueApeStudio.Crusader.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [SerializeField] private AudioSource _soundFXObject;
        
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        public void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume)
        {
            AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);

            audioSource.clip = audioClip;

            audioSource.volume = volume;

            audioSource.Play();

            float _clipLength = audioSource.clip.length;

            Destroy(audioSource.gameObject, _clipLength);
        }

        public void PlayRandomSwordSFX(AudioClip[] audioClip, Transform spawnTransform, float volume)
        {
            int _random = Random.Range(0, audioClip.Length);
            
            AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);

            audioSource.clip = audioClip[_random];

            audioSource.volume = volume;

            audioSource.Play();

            float _clipLength = audioSource.clip.length;

            Destroy(audioSource.gameObject, _clipLength);
        }
    }
}
