    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Player.Bounds
{
    public class Bounds : MonoBehaviour
    {
        [SerializeField] private Transform _resetTransform;
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private string _playerTag = "Player";

        private void OnTriggerExit(Collider other)
        {
            if(other.tag.Equals(_playerTag))
            {
                other.transform.position = _resetTransform.position;
            }
        }
    }
}
