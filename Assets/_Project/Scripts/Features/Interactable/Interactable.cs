using System.Collections.Generic;
using _Project.Scripts.Features.Items;
using _Project.Scripts.Features.Player;
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
        
        private InteractableProvider _interactableProvider;
        private Inventory.Inventory _inventory;
        private KeyValuePair<int, Item> _selectedItem;

        public enum InteractConditions
        {
            OnAnyConditions = 0,
            OnItemInInventory = 1
        }
        
        public abstract void Interact();

        protected virtual void Start()
        {
            _interactableProvider = FindObjectOfType<InteractableProvider>();
            _interactableProvider?.RegisterItem(this);
            
            _inventory = FindObjectOfType<Inventory.Inventory>();
        }

        protected virtual void OnDestroy()
        {
            _interactableProvider?.UnregisterItem(this);
        }

        public bool IsConditionFulfilled(out string message)
        {
            message = string.Empty;
            
            switch (interactCondition)
            {
                case InteractConditions.OnAnyConditions:
                {
                    return true;
                }
                case InteractConditions.OnItemInInventory:
                {
                    return CheckOnItemInInventoryCondition(out message);
                }
                default:
                {
                    message = "unknown type of condition";
                    return false;
                }
            }
        }

        private bool CheckOnItemInInventoryCondition(out string message)
        {
            if (!_inventory.TryGetItemWithIdAndSlot(itemId, out var item, out var slot, isItemSelected))
            {
                message = "necessary item not in inventory or not selected";
                return false;
            }
            _selectedItem = new KeyValuePair<int, Item>(slot, item);
            
            message = string.Empty;
            return true;
        }
    }
}