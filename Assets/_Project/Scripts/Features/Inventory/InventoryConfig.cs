using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Features.Inventory
{
    public class InventoryConfig : ScriptableObject
    {
        public bool isPickUpClipExist = false;
        public string pickUpClipId = "pickup";
        [MinMaxSlider(0,10,true)]public int inventoryCapacity = 1;
        public KeyCode pickUpItemKey = KeyCode.E;
        public KeyCode dropItemKey = KeyCode.Q;
    }
}