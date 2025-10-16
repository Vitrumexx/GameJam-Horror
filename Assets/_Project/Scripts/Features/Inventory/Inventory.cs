using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Features.Enemy;
using _Project.Scripts.Features.Items;
using _Project.Scripts.Features.Player;
using _Project.Scripts.Features.Sounds;
using _Project.Scripts.Features.UI;
using UnityEngine;

namespace _Project.Scripts.Features.Inventory
{
    public class Inventory : MonoBehaviour
    { 
        [Header("Services")]
        public SoundsPlayer soundsPlayer;
        public PlayerNotifier playerNotifier;
        public ItemsStorage itemsStorage;
        public ItemsRegistrator itemsRegistrator;
        public PlayerOverlay playerOverlay;
        
        [Header("Transform")]
        public Transform playerHandTransform;
        public Transform droppedItemParentTransform;
        
        [Header("Inventory config")]
        public AudioSource inventoryAudioSource;
        public InventoryConfig config;
        
        [Header("UI")]
        public CanvasGroup inventoryUIStorage;
        public GameObject inventoryUnitPrefab;
        
        private Dictionary<int, ItemInventoryUnit> _inventory = new();
        private int _selectedSlot = 1;
        private Item _pickUpHintItem;

        private EnemyNotifier _enemyNotifier;
        private bool _isInventoryVisible = false; 
        
        private static readonly string PickUpHintOverlayTag = "PickUpHintOverlay";
        private static readonly string DropHintOverlayTag = "DropHintOverlay";

        public void SetupInventoryConfig(InventoryConfig inventoryConfig)
        {
            HideInventory();
            config = inventoryConfig;
            ShowInventory();
        }
        
        private void Start()
        {
            _enemyNotifier = FindAnyObjectByType<EnemyNotifier>();
        }

        private void Update()
        {
            if (!IsNeededProcessing()) return;
            
            HandleInventorySlotSelected();
            HandleItemsToPickUp();
            HandlePickUp();
            HandleDrop();
        }

        private void HandlePickUp()
        {
            if (!Input.GetKeyDown(config.pickUpItemKey)) return;
            
            PickUpItem(_pickUpHintItem);
        }

        private void HandleDrop()
        {
            if (!Input.GetKeyDown(config.dropItemKey)) return;
            
            DropItem(_selectedSlot);
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
            foreach (var inventoryItem in _inventory.Where(x => x.Value.Item is not null))
            {
                DropItem(inventoryItem.Key);
            }
            
            _isInventoryVisible = false;
            _inventory.Clear();
            inventoryUIStorage.gameObject.SetActive(false);
        }

        private void ShowInventory()
        {
            _isInventoryVisible = true;

            for (var i = 0; i < config.inventoryCapacity; i++)
            {
                var inventoryUnit = Instantiate(inventoryUnitPrefab, inventoryUIStorage.transform);

                if (!inventoryUnit.TryGetComponent<ItemInventoryUnit>(out var itemInventoryUnit))
                {
                    Debug.LogError($"Item {i} has no ItemInventoryUnit attached");
                }
                
                _inventory.TryAdd(i + 1, itemInventoryUnit);
            }
            
            inventoryUIStorage.gameObject.SetActive(true);
            OnChangeSelectedSlot(1);
        }

        private void HandleInventorySlotSelected()
        {
            for (var i = 0; i < 9; i++)
            {
                var key = KeyCode.Alpha0 + i;
                if (Input.GetKeyDown(key))
                {
                    OnChangeSelectedSlot(i);
                }
            }
        }

        private void HandleItemsToPickUp()
        {
            if (!IsHintNeeded(out var nearestItem, out var nearestStorableUnit))
            {
                if (_pickUpHintItem is null) return;
                
                _pickUpHintItem = null;
                playerOverlay.RemoveData(PickUpHintOverlayTag);

                return;
            }

            if (_pickUpHintItem == nearestItem) return;
            
            _pickUpHintItem = nearestItem;

            var message = $"Press \"{config.pickUpItemKey}\" to raise";
            playerOverlay.UpdateData(PickUpHintOverlayTag, message, nearestStorableUnit.icon);
        }

        private bool IsHintNeeded(out Item nearestItem, out ItemStorableUnit nearestStorableUnit)
        {
            nearestStorableUnit = null;
            
            if (!itemsRegistrator.TryGetNearestDroppedItemToPickUp(playerHandTransform.position, out nearestItem, config.pickUpDistance))
            {
                return false;
            }
            
            if (!itemsStorage.TryGetItemStorableUnit(nearestItem.id, out nearestStorableUnit))
            {
                return false;
            }
            
            return true;
        }

        private void OnChangeSelectedSlot(int slot)
        {
            if (slot < 0 || slot > config.inventoryCapacity)
            {
                return;
            }

            if (slot == 0)
            {
                slot = 10;
            }

            if (!_inventory.TryGetValue(_selectedSlot, out var prevSelectedSlot) ||
                !_inventory.TryGetValue(slot, out var nextSelectedSlot))
            {
                return;
            }
            
            prevSelectedSlot.SetSelected(false);
            prevSelectedSlot.Item?.gameObject.SetActive(false);
            
            nextSelectedSlot.SetSelected(true);
            if (nextSelectedSlot.Item is not null)
            {
                nextSelectedSlot.Item.gameObject.SetActive(true);
                ShowDropHint();
            }
            else
            {
                HideDropHint();
            }
            
            _selectedSlot = slot;
        }

        private void ShowDropHint()
        {
            var message = $"Press \"{config.dropItemKey}\" to drop";
            playerOverlay.AddData(DropHintOverlayTag, message);
        }

        private void HideDropHint()
        {
            playerOverlay.RemoveData(DropHintOverlayTag);
        }

        private void PickUpItem(Item item)
        {
            if (item is null)
            {
                playerNotifier.NotifyPlayer("Nothing to raise.");
                return;
            }
            
            var insertKey = -1;
            
            if (_inventory[_selectedSlot].Item is null)
            {
                insertKey = _selectedSlot;
            }
            else
            {
                var sortedInventory = GetSortedInventory();

                foreach (var keyValuePair in sortedInventory.Where(keyValuePair => keyValuePair.Value.Item is null))
                {
                    insertKey = keyValuePair.Key;
                }
            }

            if (insertKey < 0)
            {
                playerNotifier.NotifyPlayer("Inventory is full.");
                return;
            }
            
            if (!itemsStorage.TryGetItemStorableUnit(item.id, out var itemStorableUnit))
            {
                return;
            }
            
            item.PickUp(playerHandTransform);
            _inventory[insertKey].SetItem(item, itemStorableUnit);

            if (_selectedSlot != insertKey)
            {
                _inventory[insertKey].Item?.gameObject.SetActive(false);
            }
            else
            {
                item.SetIsSelected(true);
                ShowDropHint();
            }

            if (config.isPickUpClipExist)
            {
                soundsPlayer.PlayClip(inventoryAudioSource, config.pickUpClipId);
            }
        }

        public void DropItem(int slot)
        {
            if (!_inventory.TryGetValue(slot, out var itemInventoryUnit))
            {
                return;
            }

            var item = itemInventoryUnit.Item;

            if (item is null)
            {
                playerNotifier.NotifyPlayer("Nothing to drop.");
                return;
            }
            
            item.Drop(droppedItemParentTransform);
            _inventory[_selectedSlot].Clear();
            HideDropHint();

            if (!itemsStorage.TryGetItemStorableUnit(item.id, out var itemStorableUnit))
            {
                return;
            }
            
            soundsPlayer.PlayClip(inventoryAudioSource, itemStorableUnit.soundId);
                
            if (itemStorableUnit.itemWeight == ItemStorableUnit.ItemWeight.Heavy)
            {
                _enemyNotifier?.NotifyEnemyAboutDroppedItem(item.gameObject.transform.position);
            }
        }

        private List<KeyValuePair<int, ItemInventoryUnit>> GetSortedInventory()
        {
            var inventoryCopy = _inventory.ToList();
            inventoryCopy.Sort((x, y) => y.Key.CompareTo(x.Key));
            return inventoryCopy;
        }

        public bool TryGetItemWithIdAndSlot(string itemId, out Item item, out int slot, bool isSelected = false)
        {
            item = null;
            slot = -1;

            if (_inventory[_selectedSlot].Item is not null && _inventory[_selectedSlot].Item.id == itemId)
            {
                item = _inventory[_selectedSlot].Item;
                slot = _selectedSlot;
            }
            
            if (item is not null) return true;
            if (isSelected) return false;

            var sortedInventory = GetSortedInventory();

            foreach (var inventoryPair in sortedInventory
                         .Where(inventoryPair => inventoryPair.Value.Item is not null && inventoryPair.Value.Item.id == itemId))
            {
                item = inventoryPair.Value.Item;
                slot = inventoryPair.Key;
            }
            
            return item is not null;
        }

        public bool TryGetSlotOfItemInInventory(Item item, out int slot)
        {
            slot = -1;
            
            foreach (var inventorySlot in _inventory)
            {
                if (inventorySlot.Value.Item is null || inventorySlot.Value.Item != item) continue;
                
                slot = inventorySlot.Key;
                break;
            }

            return slot >= 1;
        }
    }
}