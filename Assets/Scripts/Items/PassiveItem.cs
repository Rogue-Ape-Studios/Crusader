using System;
using UnityEngine;

namespace RogueApeStudio.Crusader.Items
{
    internal class PassiveItem : Item
    {
        internal event Action<PlayerProperty,float> OnCollect;
        [SerializeField] private PlayerProperty _property;
        [SerializeField] private float _percentage;

        private void OnDestroy()
        {
            OnCollect = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            OnCollect?.Invoke(_property,_percentage);
            Destroy(gameObject);
        }
    }
}
