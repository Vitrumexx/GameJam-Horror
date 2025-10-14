using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Features.Player;
using _Project.Scripts.Features.Shared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Features.Interactable
{
    public class InteractableProvider : UnitRegistrator<Interactable>
    {
        [Header("Player Info")]
        public Transform player;
        public float interactDistance = 3f;
        
        [Header("Services")]
        public PlayerOverlay playerOverlay;
        
        [Header("Config")]
        public KeyCode interactKey = KeyCode.E;
        [InfoBox("Можно не указывать")] public Sprite warningSprite;
        
        private Interactable _closestInteractable;
        private bool _isOverlayVisible = false;
        
        private void Update()
        {
            SearchForClosestInteractable();
            HandleInteract();
        }

        private void SearchForClosestInteractable()
        {
            _closestInteractable = Units
                .Where(x => x is not null)
                .Where(x => x.isInteractable)
                .Select(x => new { obj = x, dist = Vector3.SqrMagnitude(player.position - x.transform.position) })
                .Where(x => x.dist <= interactDistance * interactDistance)
                .OrderBy(x => x.dist)
                .Select(x => x.obj)
                .FirstOrDefault();

            if (_closestInteractable is null)
            {
                if (!_isOverlayVisible) return;
                
                _isOverlayVisible = false;
                playerOverlay.HideInteractable();
            }
            else
            {
                if (_isOverlayVisible) return;
                
                _isOverlayVisible = true;
                playerOverlay.ShowInteractable(interactKey, warningSprite);
            }
        }

        private void HandleInteract()
        {
            _closestInteractable.Interact();
        }
    }
}