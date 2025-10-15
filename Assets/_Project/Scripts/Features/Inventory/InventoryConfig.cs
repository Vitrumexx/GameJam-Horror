using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Features.Inventory
{
    public class InventoryConfig : ScriptableObject
    {
        public bool isPickUpClipExist = false;
        public string pickUpClipId = "pickup";
        [PropertyRange(0,10)]public int inventoryCapacity = 1;
        public KeyCode pickUpItemKey = KeyCode.E;
        public KeyCode dropItemKey = KeyCode.Q;
        public float pickUpDistance = 5f;
    }
}