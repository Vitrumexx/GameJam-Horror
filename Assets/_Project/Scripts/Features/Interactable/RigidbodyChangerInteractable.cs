using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace _Project.Scripts.Features.Interactable
{
    public class RigidbodyChangerInteractable : Interactable
    {
        [Header("Changes on interact")]
        public bool isKinematic = false;

        private readonly HashSet<Rigidbody> _rigidbodies = new();

        protected override void Start()
        {
            base.Start();
            
            _rigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());

            var selfComponent = GetComponent<Rigidbody>();
            if (selfComponent is not null) _rigidbodies.Add(selfComponent);
        }

        protected override void Interact()
        {
            foreach (var rb in _rigidbodies.Where(x => x is not null))
            {
                rb.isKinematic = isKinematic;
            }
        }
    }
}