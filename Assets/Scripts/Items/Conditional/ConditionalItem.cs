using RogueApeStudio.Crusader.Audio;
using UnityEngine;
using UnityEngine.VFX;

namespace RogueApeStudio.Crusader.Items.Conditional
{
    internal abstract class ConditionalItem : MonoBehaviour
    {
        [Header("Conditon Settings")]
        [SerializeField, Tooltip("Will be cast to the correct value")] protected MonoBehaviour _conditionTarget;
        [Header("Visuals Effects Settings")]
        [SerializeField] protected VisualEffect _itemVisualEffect;
        [Header("Sound settings")]
        [SerializeField] protected AudioClip _audioClip;
        [SerializeField] protected Transform _soundTarget;
        [SerializeField] protected float _volume = 1f;

        /// <summary>
        /// Sets up the condition to be invoked. Cast the _conditonTarget and subscribe to the correct event here.
        /// </summary>
        internal virtual void SetupCondition() {}

        /// <summary>
        /// Is invoked when a condition is met, and handles any base logic. Can be overriden, but not necessary.
        /// </summary>
        internal void HandleConditionMet()
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
        internal virtual void ConditionalItemEffect() {}
    }
}
