using System;
using _Project.Scripts.Features.Player;
using UnityEngine;

namespace _Project.Scripts.Features.Items
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Item : MonoBehaviour
    {
        [Header("Base config")]
        public string id = string.Empty;
        public bool IsDropped { get; set; } = true;
        public bool isPickupable = true;
        public float localScaleModifierOnPickUp = 0.5f;
        
        [Header("Message config")]
        [TextArea]public string messageOnPickup = string.Empty;
        public bool isOnlyOnFirstPickup = false;
        
        private Collider _collider;
        private Rigidbody _rigidbody;
        private ItemsRegistrator _itemsRegistrator;
        private Inventory.Inventory _inventory;
        private Vector3 _localScale;
        private ItemsStorage _itemsStorage;
        private ItemStorableUnit _itemStorableUnit;
        private PlayerNotifier _playerNotifier;
        private bool _isNotifiedOnPickup = false;
        
        public bool IsSelected { get; private set; }
        public event Action OnSelected;
        public event Action OnDeselected;
        public event Action OnPickup;
        public event Action OnDrop;

        public void Start()
        {
            if (id == string.Empty) Debug.LogWarning($"No item {gameObject.name} ID specified!");
            
            _localScale = transform.localScale;
            
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            
            _itemsRegistrator = FindAnyObjectByType<ItemsRegistrator>();
            _itemsRegistrator?.RegisterItem(this);
            
            _inventory = FindAnyObjectByType<Inventory.Inventory>();
            _itemsStorage = FindAnyObjectByType<ItemsStorage>();
            _itemsStorage.TryGetItemStorableUnit(id, out _itemStorableUnit);
            
            _playerNotifier = FindAnyObjectByType<PlayerNotifier>();
        }

        public void Drop(Transform droppedStorage)
        {
            OnDrop?.Invoke();
            
            IsDropped = true;

            transform.SetParent(droppedStorage);
            
            if (_rigidbody is not null) _rigidbody.isKinematic = false;
            if (_collider is not null) _collider.isTrigger = false;
            transform.localScale = _localScale;
        }

        public void PickUp(Transform pickUpStorage)
        {
            OnPickup?.Invoke();
            IsDropped = false;

            transform.SetParent(pickUpStorage, true);
            
            if (_itemStorableUnit is not null)
            {
                transform.localRotation = _itemStorableUnit.itemPrefab.transform.rotation;
                transform.localPosition = _itemStorableUnit.itemPrefab.transform.position;
            }
            transform.localScale = _localScale * localScaleModifierOnPickUp;
            
            if (_rigidbody is not null) _rigidbody.isKinematic = true;
            if (_collider is not null) _collider.isTrigger = true;
            
            SendMessageOnPickUp();
        }

        private void SendMessageOnPickUp()
        {
            if (messageOnPickup == string.Empty) return;
            if (isOnlyOnFirstPickup && _isNotifiedOnPickup) return;
            
            _playerNotifier?.NotifyPlayer(messageOnPickup);
            _isNotifiedOnPickup = true;
        }

        private void OnDestroy()
        {
            // if (_inventory?.TryGetSlotOfItemInInventory(this, out var slot) == true)
            // {
            //     _inventory.DropItem(slot);
            // }
            
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