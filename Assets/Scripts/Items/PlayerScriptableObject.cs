using UnityEngine;

namespace RogueApeStudio.Crusader.Items
{
    [CreateAssetMenu(menuName ="CustomConfig/Player", fileName = "PlayerConfig")]
    public class PlayerScriptableObject : ScriptableObject
    {
        [Header("Health")]
        public int HitPoints = 100;
        public int RegenPerSecond = 0;
        [Header("Attack")]
        public int AttackDamage = 5;
        public int AttackSpeed = 1;
        [Header("Mobility")]
        public int MovementSpeed = 1;
        public int DashSpeed = 1;
    }
}
