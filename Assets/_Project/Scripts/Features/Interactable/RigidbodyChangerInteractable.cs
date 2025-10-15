using UnityEngine;

namespace _Project.Scripts.Features.Interactable
{
    public class RigidbodyChangerInteractable : Interactable
    {
        [Header("Changes on interact")]
        public bool isKinematic = false;

        public Rigidbody[] rigidbodies;
        
        protected override void Interact()
        {
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = isKinematic;
            }
        }
    }
}