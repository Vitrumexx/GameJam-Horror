using UnityEngine;

namespace _Project.Scripts.Features.Items.InteractableItems
{
    [RequireComponent(typeof(Item))]
    public abstract class InteractableItem : MonoBehaviour
    {
        protected Item Item;

        protected virtual void Start()
        {
            Item = GetComponent<Item>();
        }
        
        public abstract void Interact();
    }
}