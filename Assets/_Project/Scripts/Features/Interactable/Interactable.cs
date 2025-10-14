using System.Collections.Generic;
using _Project.Scripts.Features.Items;
using UnityEngine;

namespace _Project.Scripts.Features.Interactable
{
    public abstract class Interactable : MonoBehaviour
    {
        [Header("Config")]
        public bool isInteractable = true;
        public InteractConditions interactCondition = InteractConditions.OnAnyConditions;

        [Header("Condition on item in inventory config")]
        public string itemId;
        public bool isItemSelected = false;
        public bool isDelItemOnUse = true;
        
        private InteractableProvider _interactableProvider;
        private Inventory.Inventory _inventory;
        private ItemsStorage _itemsStorage;
        private KeyValuePair<int, Item> _selectedItem;

        public enum InteractConditions
        {
            OnAnyConditions = 0,
            OnItemInInventory = 1
        }
        
        protected abstract void Interact();

        public void ProcessInteract()
        {
            Interact();

            if (!isDelItemOnUse) return;
            
            _inventory.DropItem(_selectedItem.Key);
            Destroy(_selectedItem.Value.gameObject);
        }

        protected virtual void Start()
        {
            _interactableProvider = FindObjectOfType<InteractableProvider>();
            _interactableProvider?.RegisterItem(this);
            
            _inventory = FindObjectOfType<Inventory.Inventory>();
            _itemsStorage = FindObjectOfType<ItemsStorage>();
        }

        protected virtual void OnDestroy()
        {
            _interactableProvider?.UnregisterItem(this);
        }

        public bool IsConditionFulfilled(out string message, out Sprite sprite)
        {
            message = string.Empty;
            sprite = null;
            
            switch (interactCondition)
            {
                case InteractConditions.OnAnyConditions:
                {
                    return true;
                }
                case InteractConditions.OnItemInInventory:
                {
                    return CheckOnItemInInventoryCondition(out message, out sprite);
                }
                default:
                {
                    message = "unknown type of condition";
                    return false;
                }
            }
        }

        private bool CheckOnItemInInventoryCondition(out string message, out Sprite sprite)
        {
            sprite = null;
            message = string.Empty;
            
            if (!_inventory.TryGetItemWithIdAndSlot(itemId, out var item, out var slot, isItemSelected))
            {
                if (!_itemsStorage.TryGetItemStorableUnit(itemId, out var unit))
                {
                    message = $"no items that can interact with this object";
                }
                else
                {
                    message = $"you need {unit.unitName}";
                    if (isItemSelected) message += " in your hand";
                    sprite = unit.icon;
                }
                
                return false;
            }
            _selectedItem = new KeyValuePair<int, Item>(slot, item);
            
            return true;
        }
    }
}