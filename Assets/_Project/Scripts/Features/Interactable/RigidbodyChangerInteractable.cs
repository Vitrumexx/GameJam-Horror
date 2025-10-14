using UnityEngine;

namespace _Project.Scripts.Features.Interactable
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class RigidbodyChangerInteractable : Interactable
    {
        [Header("Changes on interact")]
        public bool isKinematic = false;

        private Rigidbody _rigidbody;
        
        protected override void Start()
        {
            base.Start();
            
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public override void Interact()
        {
            _rigidbody.isKinematic = isKinematic;
        }
    }
}