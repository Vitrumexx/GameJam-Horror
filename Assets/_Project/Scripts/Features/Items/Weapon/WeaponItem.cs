using _Project.Scripts.Features.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Features.Items.Weapon
{
    [RequireComponent(typeof(Item))]
    public abstract class WeaponItem : MonoBehaviour
    {
        private static string OverlayTag = "WeaponItem";
        public KeyCode attackKey = KeyCode.Mouse0;

        [InfoBox("Оставь пустым, чтобы выбрать все")]
        public string[] damagableTags;

        private Item _item;
        private PlayerOverlay _playerOverlay;

        private void Awake()
        {
            OverlayTag += GetHashCode().ToString();
        }

        private void Start()
        {
            _playerOverlay = FindObjectOfType<PlayerOverlay>();
            _item = GetComponent<Item>();

            if (_item is null) return;

            _item.OnSelected += ShowWeaponHint;
            _item.OnDeselected += HideWeaponHint;
        }

        private void Update()
        {
            HandleAttack();
        }

        private void OnDestroy()
        {
            if (_item is null) return;

            _item.OnSelected -= ShowWeaponHint;
            _item.OnDeselected -= HideWeaponHint;
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