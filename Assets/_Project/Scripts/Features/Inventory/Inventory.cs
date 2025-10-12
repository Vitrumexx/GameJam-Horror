using System.Collections.Generic;
using _Project.Scripts.Features.Enemy;
using _Project.Scripts.Features.Items;
using _Project.Scripts.Features.Sounds;
using UnityEngine;

namespace _Project.Scripts.Features.Inventory
{
    public class Inventory : MonoBehaviour
    { 
        [Header("Sounds")]
        public SoundsStorage soundsStorage;
        public AudioSource audioSource;
        
        [Header("Transform")]
        public Transform playerHandTransform;
        public Transform droppedItemParentTransform;
        
        [Header("Inventory config")]
        [SerializeField] private InventoryConfig config;
        
        private List<KeyValuePair<int, Item>> _inventory;
        
        public List<Item> Items { get; private set; } = new();
        public int SelectedIndex { get; private set; } = -1;

        public Item SelectedItem
        {
            get
            {
                if (SelectedIndex >= 0 && SelectedIndex < Items.Count) return Items[SelectedIndex];
                
                return null;
            }
        }

        private EnemyNotifier _enemyNotifier;

        private void Start()
        {
            _enemyNotifier = FindAnyObjectByType<EnemyNotifier>();
        }

        public void PickUpItem(Item item)
        {
            if (Items.Count >= inventoryCapacity)
            {
                SendMessageToPlayer("Инвентарь полон!");
                return;
            }
            
            Items.Add(item);
            
            item.transform.SetParent(playerHandTransform);
            item.Collider.enabled = false;
            item.Rigidbody.useGravity = false;
            
            if (!isPickUpClipExist) return;
            if (!soundsStorage.Items.TryGetValue(pickUpClipId, out var sound)) return;
            
            audioSource.volume = sound.volume;
            audioSource.PlayOneShot(sound.clip);
        }

        public void DropItem()
        {
            var item = SelectedItem;
            
            if (item == null)
            {
                SendMessageToPlayer("Твоя рука пуста.");
                return;
            }
            
            Items.Remove(item);
            
            item.transform.SetParent(droppedItemParentTransform);
            item.Collider.enabled = true;
            item.Rigidbody.useGravity = true;

            if (soundsStorage.Items.TryGetValue(item.soundId, out var sound))
            {
                audioSource.volume = sound.volume;
                audioSource.PlayOneShot(sound.clip);
            }
            
            if (item.itemWeight == Item.ItemWeight.Heavy)
            {
                _enemyNotifier.NotifyEnemy();
            }
        }

        public void SendMessageToPlayer(string msg)
        {
            
        }
    }
}