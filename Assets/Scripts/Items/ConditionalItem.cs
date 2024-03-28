using RogueApeStudio.Crusader.Audio;
using UnityEngine;
using UnityEngine.VFX;

namespace RogueApeStudio.Crusader.Items
{
    public abstract class ConditionalItem : MonoBehaviour
    {
        [SerializeField] Transform _target;
        [SerializeField] VisualEffect _itemVisualEffect;
        [Header("Sound settings")]
        [SerializeField] AudioClip _audioClip;
        [SerializeField] float _volume = 1f;
        public void HandleConditionMet()
        {
            if(_itemVisualEffect != null)
            {
                _itemVisualEffect.Play();
            }

            if(_audioClip != null)
            {
                AudioManager.instance.PlaySFX(_audioClip, _target, _volume);
            }

            ConditionalItemEffect();
        }

        /// <summary>
        /// Override this with the specific implementation of the item, be it spherecast, raycast, or whatever else.
        /// </summary>
        public virtual void ConditionalItemEffect() {}
    }
}
