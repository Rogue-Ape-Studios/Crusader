using RogueApeStudio.Crusader.Audio;
using UnityEngine;
using UnityEngine.VFX;

namespace RogueApeStudio.Crusader.Items
{
    public abstract class ConditionalItem : MonoBehaviour
    {
        [Header("Conditon Settings")]
        [SerializeField, Tooltip("Will be cast to the correct value")] MonoBehaviour _conditionTarget;
        [Header("Visuals Effects Settings")]
        [SerializeField] VisualEffect _itemVisualEffect;
        [Header("Sound settings")]
        [SerializeField] AudioClip _audioClip;
        [SerializeField] Transform _soundTarget;
        [SerializeField] float _volume = 1f;

        /// <summary>
        /// Sets up the condition to be invoked. Cast the _conditonTarget and subscribe to the correct event here.
        /// </summary>
        public virtual void SetupCondition() {}

        /// <summary>
        /// Is invoked when a condition is met, and handles any base logic. Can be overriden, but not necessary.
        /// </summary>
        public void HandleConditionMet()
        {
            if(_itemVisualEffect != null)
            {
                _itemVisualEffect.Play();
            }

            if(_audioClip != null)
            {
                AudioManager.instance.PlaySFX(_audioClip, _soundTarget, _volume);
            }

            ConditionalItemEffect();
        }

        /// <summary>
        /// Override this with the specific implementation of the item, be it spherecast, raycast, or whatever else.
        /// </summary>
        public virtual void ConditionalItemEffect() {}
    }
}
