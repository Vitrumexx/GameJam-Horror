using UnityEngine;

namespace _Project.Scripts.Features.Inventory
{
    public class InventoryConfig : ScriptableObject
    {
        public bool isPickUpClipExist = false;
        public string pickUpClipId = "pickup";
        public int inventoryCapacity = 1;
    }
}