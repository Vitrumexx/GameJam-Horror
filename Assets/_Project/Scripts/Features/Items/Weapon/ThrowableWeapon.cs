using System.Linq;
using UnityEngine;
using _Project.Scripts.Features.Shared;

namespace _Project.Scripts.Features.Items.Weapon
{
    public class ThrowableWeapon : WeaponItem
    {
        public bool destroyOnImpact = false;
        public bool destroyOnLifeTime = false; 
        public float lifetime = 5f;
        public float throwForce = 20f;
        
        public Camera playerCamera;
        
        private bool _isThrown = false;

        protected override void Start()
        {
            base.Start();

            _item.OnPickup += ChangeIsThrownToFalse;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _item.OnPickup -= ChangeIsThrownToFalse;
        }

        private void ChangeIsThrownToFalse()
        {
            _isThrown = false;
        }

        protected override void Attack()
        {
            if (_isThrown) return;
            
            if (!_inventory.TryGetSlotOfItemInInventory(_item, out var slot)) return;
            
            _inventory.DropItem(slot);

            transform.position = playerCamera.transform.position;
            transform.rotation = playerCamera.transform.rotation;
            
            _rigidbody.velocity = playerCamera.transform.forward * throwForce;
            _isThrown = true;

            if (destroyOnLifeTime) Destroy(gameObject, lifetime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isThrown) return;

            if (collision.collider.TryGetComponent(out Damagable damagable) 
                && damagableTags.Contains(damagable.damagableTag))
            {
                damagable.OnDamage();
            }

            if (destroyOnImpact) Destroy(gameObject);
        }
    }
}