using _Project.Scripts.Features.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Features.Items.Weapon
{
    [RequireComponent(typeof(Item), typeof(Rigidbody), typeof(Collider))]
    public abstract class WeaponItem : MonoBehaviour
    {
        protected static string OverlayTag = "WeaponItem";
        public KeyCode attackKey = KeyCode.Mouse0;

        [InfoBox("Оставь пустым, чтобы выбрать все")]
        public string[] damagableTags;

        protected Item _item;
        protected PlayerOverlay _playerOverlay;
        protected Rigidbody _rigidbody;
        protected Inventory.Inventory _inventory;

        private void Awake()
        {
            OverlayTag += GetHashCode().ToString();
        }

        protected virtual void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerOverlay = FindObjectOfType<PlayerOverlay>();
            _item = GetComponent<Item>();
            _inventory = FindObjectOfType<Inventory.Inventory>();

            if (_item is null) return;

            _item.OnSelected += ShowWeaponHint;
            _item.OnDeselected += HideWeaponHint;
            _item.OnDrop += HideWeaponHint;
        }

        private void Update()
        {
            HandleAttack();
        }

        protected virtual void OnDestroy()
        {
            if (_item is null) return;

            _item.OnSelected -= ShowWeaponHint;
            _item.OnDeselected -= HideWeaponHint;
            _item.OnDrop -= HideWeaponHint;
        }

        protected abstract void Attack();

        private void HandleAttack()
        {
            if (!Input.GetKeyDown(attackKey)) return;
            
            Attack();
        }

        private void ShowWeaponHint()
        {
            var message = $"Press \"{attackKey}\" to attack.";

            _playerOverlay.UpdateData(OverlayTag, message);
        }

        private void HideWeaponHint()
        {
            _playerOverlay.RemoveData(OverlayTag);
        }
    }
}