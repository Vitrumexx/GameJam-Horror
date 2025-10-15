using System;
using _Project.Scripts.Features.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Features.Items.Weapon
{
    [RequireComponent(typeof(Item))]
    public class WeaponItem : MonoBehaviour
    {
        public KeyCode attackButton = KeyCode.Mouse0;
        [InfoBox("Оставь пустым, чтобы выбрать все")]
        public string[] damagableTags;
        
        private Item _item;
        private PlayerOverlay _playerOverlay;

        private static string OverlayTag = "WeaponItem";

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

        private void ShowWeaponHint()
        {
            var message = $"Press \"{attackButton}\" to attack.";
            
            _playerOverlay.UpdateData(OverlayTag, message);
        }

        private void HideWeaponHint()
        {
            _playerOverlay.RemoveData(OverlayTag);
        }

        private void OnDestroy()
        {
            if (_item is null) return;

            _item.OnSelected -= ShowWeaponHint;
            _item.OnDeselected -= HideWeaponHint;
        }
    }
}