using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Items
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private PlayerScriptableObject _currentPlayer;
        [SerializeField] private List<Item> _items;
        [SerializeField] private List<Transform> _itemSpawns;

        /// <summary>
        /// Spawns a random item.
        /// </summary>
        /// <param name="position">The position to spawn the item at.</param>
        public void SpawnRandomItem(Vector3 position)
        {
            var item = Instantiate(_items[UnityEngine.Random.Range(0,_items.Count)], position, Quaternion.identity);

            if(item is PassiveItem)
                HandlePassiveItem(item as PassiveItem);
        }

        /// <summary>
        /// Spawns a random item, based on rarity.
        /// </summary>
        /// <param name="position">The position to spawn the item at.</param>
        /// <param name="rarity">The rarity of the item.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SpawnRandomItem(Vector3 position, ItemRarity rarity)
        {
            throw new NotImplementedException();
        }

        private void HandleCommonItemCollection(PlayerProperty property, float percentage)
        {
            switch (property)
            {
                case PlayerProperty.Hitpoints:
                _currentPlayer.HitPoints = (int)(_currentPlayer.HitPoints * percentage);
                    break;
                case PlayerProperty.AttackDamage:
                _currentPlayer.AttackDamage *= (int)(_currentPlayer.AttackDamage * percentage);
                    break;
                case PlayerProperty.AttackSpeed:
                _currentPlayer.AttackSpeed *= (int)(_currentPlayer.AttackSpeed * percentage);
                    break;
                default:
                throw new NotImplementedException("That property has not been implemented yet!");
            }
        }

        [ContextMenu("Spawn Item")]
        private void SpawnItem()
        {
            SpawnRandomItem(_itemSpawns[0].position);
        }

        private void HandlePassiveItem(PassiveItem item)
        {
            item.OnCollect += HandleCommonItemCollection;       
        }

    }
}

