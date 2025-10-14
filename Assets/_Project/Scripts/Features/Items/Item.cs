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

        public void Start()
        {
            if (id == string.Empty) Debug.LogWarning($"No item {gameObject.name} ID specified!");

            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _itemsRegistrator = FindAnyObjectByType<ItemsRegistrator>();
            
            _itemsRegistrator?.RegisterItem(this);
        }

        public void Drop(Transform droppedStorage)
        {
            IsDropped = true;

            transform.SetParent(droppedStorage);
            _rigidbody.isKinematic = false;
            _collider.isTrigger = false;
        }

        public void PickUp(Transform pickUpStorage)
        {
            IsDropped = false;

            transform.SetParent(pickUpStorage);
            transform.position = pickUpStorage.position;
            transform.rotation = pickUpStorage.rotation;
            _rigidbody.isKinematic = true;
            _collider.isTrigger = true;
        }

        private void OnDestroy()
        {
            _itemsRegistrator?.UnregisterItem(this);
        }
    }
}