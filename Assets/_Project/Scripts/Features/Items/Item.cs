using _Project.Scripts.Features.Shared;
using UnityEngine;

namespace _Project.Scripts.Features.Items
{
    [RequireComponent(typeof(Rigidbody))]
    public class Item : MonoBehaviour
    {
        public string id;
        public Rigidbody Rigidbody { get; private set; }
        public Collider Collider { get; private set; }

        public void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
        }
    }
}