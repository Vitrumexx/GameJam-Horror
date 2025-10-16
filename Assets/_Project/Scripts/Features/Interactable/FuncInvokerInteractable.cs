using UnityEngine.Events;

namespace _Project.Scripts.Features.Interactable
{
    public class FuncInvokerInteractable : Interactable
    {
        public UnityEvent onInteract;
        
        protected override void Interact()
        {
            onInteract?.Invoke();
        }
    }
}