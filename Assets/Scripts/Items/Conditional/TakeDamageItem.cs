using System;
using RogueApeStudio.Crusader.HealthSystem;

namespace RogueApeStudio.Crusader.Items.Conditional
{
    public class TakeDamageItem : ConditionalItem
    {
        internal override void SetupCondition()
        {
            if(_conditionTarget == null)
            {
                throw new ArgumentNullException(nameof(_conditionTarget));
            }

            var health = _conditionTarget as Health;
            health.OnDamage += HandleConditionMet;
        }

        private void HandleConditionMet(float _)
        {
            base.HandleConditionMet();
        }

        internal override void ConditionalItemEffect()
        {
            
        }
    }
}
