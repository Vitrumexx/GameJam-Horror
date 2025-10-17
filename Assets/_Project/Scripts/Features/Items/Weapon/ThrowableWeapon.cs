using System.Linq;
using _Project.Scripts.Features.Damagables;
using _Project.Scripts.Features.Player;
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
        public bool isTurnOffTheDangerZoneOnZeroVelocity = true;
        
        private FirstPersonController _fpsController;
        private bool _isThrown = false;
        private bool _isDangerZoneActive = false;

        protected override void Start()
        {
            base.Start();
            
            _fpsController = FindObjectOfType<FirstPersonController>();

            _item.OnPickup += ChangeIsThrownToFalse;
        }

        protected override void Update()
        {
            base.Update();
            
            if (!_isThrown) return;

            if (!isTurnOffTheDangerZoneOnZeroVelocity) return;

            if (_rigidbody.velocity.magnitude < 0.1f) _isDangerZoneActive = false;
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

            transform.position = _fpsController.playerCamera.transform.position;
            transform.rotation = _fpsController.playerCamera.transform.rotation;
            
            _rigidbody.velocity = _fpsController.playerCamera.transform.forward * throwForce;
            
            _isThrown = true;
            _isDangerZoneActive = true;

            if (destroyOnLifeTime) Destroy(gameObject, lifetime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isThrown || !_isDangerZoneActive) return;

            if (collision.collider.TryGetComponent(out Damagable damagable))
            {
                if (damagableTags.Length > 0 && !damagableTags.Contains(damagable.damagableTag)) return;
                
                damagable.OnDamage();
            }

            if (destroyOnImpact) Destroy(gameObject);
        }
    }
}