using UnityEngine;

namespace RogueApeStudio.Crusader.Items
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    internal abstract class Item : MonoBehaviour
    {
        [Header("Base Properties")]
        [SerializeField] string _name;
        [SerializeField] string _description;
        [SerializeField] ItemRarity _itemRarity = ItemRarity.Common;

        private void OnTriggerEnter(Collider other) 
        {
            Debug.Log(_name + " - " + _itemRarity.ToString() +"\n" + _description);
            Destroy(gameObject);
        }
    }
}
