using System.Linq;
using _Project.Scripts.Features.Shared;
using UnityEngine;

namespace _Project.Scripts.Features.Items
{
    public class ItemsRegistrator : UnitRegistrator<Item>
    {
        public bool TryGetNearestDroppedItem(Vector3 position, out Item nearest, float distance)
        {
            var items = GetUnitsInDistanceToPosition(position, distance);
            
            items.RemoveAll(x => !x.IsDropped);
            
            items.Sort((x, y) => 
                Vector3.Distance(position, x.gameObject.transform.position)
                    .CompareTo(Vector3.Distance(position, y.gameObject.transform.position)));
            
            nearest = items.FirstOrDefault();
            return nearest is not null;
        }
    }
}