using _Project.Scripts.Features.Shared;
using UnityEngine;

namespace _Project.Scripts.Features.Items
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Item : MonoBehaviour
    {
        public string id = string.Empty;
        private Rigidbody _rigidbody;
        private Collider _collider;
        public bool IsDropped { get; set; } = true;

        public void Start()
        {
            if (id == string.Empty)
            {
                Debug.LogWarning($"No item {gameObject.name} ID specified!");
            }
            
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
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