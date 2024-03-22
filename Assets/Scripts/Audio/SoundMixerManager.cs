using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace RogueApeStudio.Crusader.Audio
{
    public class SoundMixerManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;

        public void SetMasterVolume(float level)
        {
            _audioMixer.SetFloat("Master Volume", level);
        }

        public void SetSFXVolume(float level) 
        {
            _audioMixer.SetFloat("SFX Volume", level);
        }

        public void SetBGMVolume(float level)
        {
            _audioMixer.SetFloat("BGM Volume", level);
        }
    }
}
