using System.Collections;
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
        public float timeOffset = 0f;

        private readonly HashSet<Rigidbody> _rigidbodies = new();
        private Coroutine _changeRoutine;

        protected override void Start()
        {
            base.Start();
            
            _rigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());

            var selfComponent = GetComponent<Rigidbody>();
            if (selfComponent is not null) _rigidbodies.Add(selfComponent);
        }

        protected override void Interact()
        {
            if (_changeRoutine != null) return;

            _changeRoutine = StartCoroutine(ChangeRigidbodies());
        }

        protected virtual IEnumerator ChangeRigidbodies()
        {
            foreach (var rb in _rigidbodies.Where(rb => rb != null))
            {
                if (destroyCancellationToken.IsCancellationRequested) yield break;
                
                rb.isKinematic = isKinematic;
                
                var t = 0f;
                while (t < timeOffset)
                {
                    if (destroyCancellationToken.IsCancellationRequested) yield break;

                    t += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}