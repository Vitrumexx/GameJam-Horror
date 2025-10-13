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
        
        [Header("Transform")]
        public Transform playerHandTransform;
        public Transform droppedItemParentTransform;
        
        [Header("Inventory config")]
        public AudioSource inventoryAudioSource;
        public InventoryConfig config;
        
        [Header("UI")]
        public CanvasGroup inventoryUI;
        public GameObject inventoryUnitPrefab;
        public UIInfoArea hintArea;
        
        private Dictionary<int, ItemInventoryUnit> _inventory;
        private int _selectedSlot = 0;
        private Item _pickUpHintItem;

        private EnemyNotifier _enemyNotifier;
        private bool _isInventoryVisible = true; 
        private bool _isInventoryHintVisible = false;

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
            if (!Input.GetKeyDown(config.pickUpItemKey))
            {
                return;
            }
            
            PickUpItem(_pickUpHintItem);
        }

        private void HandleDrop()
        {
            if (!Input.GetKeyDown(config.dropItemKey))
            {
                return;
            }
            
            DropItem();
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
            inventoryUI.gameObject.SetActive(false);
        }

        private void ShowInventory()
        {
            _isInventoryVisible = true;

            for (var i = 0; i < config.inventoryCapacity; i++)
            {
                var inventoryUnit = Instantiate(inventoryUnitPrefab, inventoryUI.transform);

                if (!inventoryUnit.TryGetComponent<ItemInventoryUnit>(out var itemInventoryUnit))
                {
                    Debug.LogError($"Item {i} has no ItemInventoryUnit attached");
                }
                
                _inventory.TryAdd(i, itemInventoryUnit);
            }
            
            inventoryUI.gameObject.SetActive(true);
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

        private void HandleItemsToPickUp() // TODO: make it not updated every frame
        {
            if (!IsHintNeeded(out var nearestItem, out var nearestStorableUnit))
            {
                if (_isInventoryHintVisible)
                {
                    HideInventoryHint();
                }

                return;
            }

            if (_pickUpHintItem == nearestItem && _isInventoryVisible)
            {
                return;
            }
            _pickUpHintItem = nearestItem;

            ShowInventoryHint($"Чтобы поднять предмет, нажмите {config.pickUpItemKey}.", nearestStorableUnit.icon);
        }

        private bool IsHintNeeded(out Item nearestItem, out ItemStorableUnit nearestStorableUnit)
        {
            nearestStorableUnit = null;
            
            if (!itemsRegistrator.TryGetNearest(playerHandTransform.position, out nearestItem, out var distance))
            {
                return false;
            }

            if (distance > config.pickUpDistance) return false;

            if (nearestItem == _pickUpHintItem)
            {
                return false;
            }
            
            if (!itemsStorage.TryGetItemStorableUnit(nearestItem.id, out nearestStorableUnit))
            {
                return false;
            }

            return true;
        }

        private void ShowInventoryHint(string hint, Sprite icon = null)
        {
            _isInventoryHintVisible = true;
            
            hintArea.infoArea.gameObject.SetActive(false);
            hintArea.text.text = hint;
            if (icon is not null) hintArea.icon.sprite = icon;
        }

        private void HideInventoryHint()
        {
            _isInventoryHintVisible = false;
            
            hintArea.infoArea.gameObject.SetActive(false);
        }

        private void OnChangeSelectedSlot(int slot)
        {
            if (slot <= 0 || slot > config.inventoryCapacity)
            {
                return;
            }

            if (!_inventory.TryGetValue(_selectedSlot, out var prevSelectedSlot) ||
                !_inventory.TryGetValue(slot, out var nextSelectedSlot))
            {
                return;
            }
            
            prevSelectedSlot.Deselect();
            nextSelectedSlot.Select();
        }

        private void PickUpItem(Item item)
        {
            var insertKey = -1;
            
            if (_inventory[_selectedSlot].Item is null)
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
            
            if (!itemsStorage.TryGetItemStorableUnit(item.id, out var itemStorableUnit))
            {
                return;
            }
            
            item.PickUp(playerHandTransform);
            _inventory[insertKey].SetItem(item, itemStorableUnit);

            if (config.isPickUpClipExist)
            {
                soundsPlayer.PlayClip(inventoryAudioSource, config.pickUpClipId);
            }
        }

        private void DropItem()
        {
            if (!_inventory.TryGetValue(_selectedSlot, out var itemInventoryUnit))
            {
                return;
            }

            var item = itemInventoryUnit.Item;
            
            if (item is null)
            {
                playerNotifier.NotifyPlayer("Твоя рука пуста.");
                return;
            }
            
            item.Drop(droppedItemParentTransform);
            _inventory[_selectedSlot].Clear();

            if (!itemsStorage.TryGetItemStorableUnit(item.id, out var itemStorableUnit))
            {
                return;
            }
            
            soundsPlayer.PlayClip(inventoryAudioSource, itemStorableUnit.soundId);
                
            if (itemStorableUnit.itemWeight == ItemStorableUnit.ItemWeight.Heavy)
            {
                _enemyNotifier.NotifyEnemyAboutDroppedItem(item.gameObject.transform.position);
            }
        }

        private List<KeyValuePair<int, ItemInventoryUnit>> GetSortedInventory()
        {
            var inventoryCopy = _inventory.ToList();
            inventoryCopy.Sort((x, y) => x.Key.CompareTo(y.Key));
            return inventoryCopy;
        }
    }
}