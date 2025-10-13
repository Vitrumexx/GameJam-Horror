using UnityEngine;

namespace _Project.Scripts.Features.Items
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Item : MonoBehaviour
    {
        public string id = string.Empty;
        public bool IsDropped { get; set; } = true;
        
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
            _collider.enabled = true;
            _rigidbody.useGravity = true;
        }

        public void PickUp(Transform pickUpStorage)
        {
            IsDropped = false;

            transform.SetParent(pickUpStorage);
            _collider.enabled = false;
            _rigidbody.useGravity = false;
        }
    }
}