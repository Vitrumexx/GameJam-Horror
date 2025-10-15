using System;
using UnityEngine;

namespace _Project.Scripts.Features.Items
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Item : MonoBehaviour
    {
        public string id = string.Empty;
        public bool IsDropped { get; set; } = true;
        public bool isPickupable = true;
        
        private Collider _collider;
        private Rigidbody _rigidbody;
        private ItemsRegistrator _itemsRegistrator;
        private Inventory.Inventory _inventory;
        
        public bool IsSelected { get; private set; }
        public event Action OnSelected;
        public event Action OnDeselected;

        public void Start()
        {
            if (id == string.Empty) Debug.LogWarning($"No item {gameObject.name} ID specified!");

            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            
            _itemsRegistrator = FindAnyObjectByType<ItemsRegistrator>();
            _itemsRegistrator?.RegisterItem(this);
            
            _inventory = FindAnyObjectByType<Inventory.Inventory>();
        }

        public void Drop(Transform droppedStorage)
        {
            OnDeselected?.Invoke();
            
            IsDropped = true;

            transform.SetParent(droppedStorage);
            
            if (_rigidbody is not null) _rigidbody.isKinematic = false;
            if (_collider is not null) _collider.isTrigger = false;
        }

        public void PickUp(Transform pickUpStorage)
        {
            IsDropped = false;

            transform.SetParent(pickUpStorage);
            transform.position = pickUpStorage.position;
            transform.rotation = pickUpStorage.rotation;
            
            if (_rigidbody is not null) _rigidbody.isKinematic = true;
            if (_collider is not null) _collider.isTrigger = true;
        }

        private void OnDestroy()
        {
            if (_inventory?.TryGetSlotOfItemInInventory(this, out var slot) == true)
            {
                _inventory.DropItem(slot);
            }
            
            _itemsRegistrator?.UnregisterItem(this);
        }

        public void SetIsSelected(bool isSelected)
        {
            IsSelected = isSelected;

            if (IsSelected)
            {
                OnSelected?.Invoke();
            }
            else
            {
                OnDeselected?.Invoke();
            }
        }
    }
}