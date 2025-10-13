using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Features.Enemy;
using _Project.Scripts.Features.Items;
using _Project.Scripts.Features.Player;
using _Project.Scripts.Features.Sounds;
using UnityEngine;

namespace _Project.Scripts.Features.Inventory
{
    public class Inventory : MonoBehaviour
    { 
        [Header("Services")]
        public SoundsPlayer soundsPlayer;
        public PlayerNotifier playerNotifier;
        public ItemsStorage itemsStorage;
        
        [Header("Transform")]
        public Transform playerHandTransform;
        public Transform droppedItemParentTransform;
        
        [Header("Inventory config")]
        public AudioSource inventoryAudioSource;
        public InventoryConfig config;
        
        private Dictionary<int, Item> _inventory;
        private int _selectedSlot = 0;

        private EnemyNotifier _enemyNotifier;
        private bool _isInventoryVisible = true; 

        private void Start()
        {
            _enemyNotifier = FindAnyObjectByType<EnemyNotifier>();
        }

        private void Update()
        {
            if (!IsNeededProcessing()) return;
            
            HandleInventorySlotSelected();
        }

        private bool IsNeededProcessing()
        {
            if (config.inventoryCapacity <= 0)
            {
                if (_isInventoryVisible)
                {
                    HideInventory();
                }

                return false;
            }

            if (!_isInventoryVisible)
            {
                ShowInventory();
            }
            
            return true;
        }

        private void HideInventory()
        {
            _isInventoryVisible = false;
            _inventory.Clear();
            
            // TODO:Add hide inventory
        }

        private void ShowInventory()
        {
            _isInventoryVisible = true;

            for (var i = 0; i < config.inventoryCapacity; i++)
            {
                _inventory.TryAdd(i, null);
            }

            // TODO:Add show inventory
        }

        private void HandleInventorySlotSelected()
        {
            for (var i = 0; i < 10; i++)
            {
                var key = KeyCode.Alpha0 + i;
                if (Input.GetKeyDown(key))
                {
                    OnChangeSelectedSlot(i);
                }
            }
        }

        private void OnChangeSelectedSlot(int slot)
        {
            if (slot <= 0 || slot > config.inventoryCapacity)
            {
                return;
            }
            
            // TODO: Add processing for change slot
        }

        public void PickUpItem(Item item)
        {
            var insertKey = -1;
            
            if (_inventory[_selectedSlot] == null)
            {
                insertKey = _selectedSlot;
            }
            else
            {
                var sortedInventory = GetSortedInventory();

                foreach (var keyValuePair in sortedInventory.Where(keyValuePair => keyValuePair.Value == null))
                {
                    insertKey = keyValuePair.Key;
                }
            }

            if (insertKey < 0)
            {
                playerNotifier.NotifyPlayer("В инвентаре недостаточно мест.");
                return;
            }
            
            item.PickUp(playerHandTransform);
            _inventory[insertKey] = item;

            if (config.isPickUpClipExist)
            {
                soundsPlayer.PlayClip(inventoryAudioSource, config.pickUpClipId);
            }
        }

        public void DropItem()
        {
            if (!_inventory.TryGetValue(_selectedSlot, out var item))
            {
                return;
            }
            
            if (item == null)
            {
                playerNotifier.NotifyPlayer("Твоя рука пуста.");
                return;
            }
            
            item.Drop(droppedItemParentTransform);
            _inventory[_selectedSlot] = null;

            if (itemsStorage.TryGetItemStorableUnit(item.id, out var itemStorableUnit))
            {
                soundsPlayer.PlayClip(inventoryAudioSource, itemStorableUnit.soundId);
            }
        }

        private List<KeyValuePair<int, Item>> GetSortedInventory()
        {
            var inventory = _inventory.ToList();
            inventory.Sort((x, y) => x.Key.CompareTo(y.Key));
            return inventory;
        }
    }
}