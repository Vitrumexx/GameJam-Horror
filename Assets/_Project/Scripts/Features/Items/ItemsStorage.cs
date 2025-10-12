using _Project.Scripts.Features.Shared;
using UnityEngine;

namespace _Project.Scripts.Features.Items
{
    public class ItemsStorage : MonoStorage<ItemStorableUnit>
    {
        public bool TryGetSoundStorableUnit(string itemId, out ItemStorableUnit storableUnit)
        {
            if (!Units.TryGetValue(itemId, out storableUnit))
            {
                Debug.LogError($"Item ID {itemId} not found!");
                return false;
            }
            
            return true;
        }
    }
}