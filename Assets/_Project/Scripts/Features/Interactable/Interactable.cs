using UnityEngine;

namespace _Project.Scripts.Features.Interactable
{
    public abstract class Interactable : MonoBehaviour
    {
        public bool isInteractable = true;

        private InteractableProvider _interactableProvider;
        
        public abstract void Interact();

        protected virtual void Start()
        {
            _interactableProvider = FindObjectOfType<InteractableProvider>();
            _interactableProvider?.RegisterItem(this);
        }

        protected virtual void OnDestroy()
        {
            _interactableProvider?.UnregisterItem(this);
        }
    }
}