using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Features.Shared
{
    public abstract class UnitRegistrator<T> : MonoBehaviour where T : MonoBehaviour
    {
        public HashSet<T> Units { get; private set; } = new();

        public void RegisterItem(T unit)
        {
            if (Units.Add(unit)) return;
            
            Debug.LogError($"Duplicate item {unit.gameObject.name}!");
        }

        public void UnregisterItem(T unit)
        {
            if (Units.Remove(unit)) return;
            
            Debug.LogError($"Not found {unit.gameObject.name}!");
        }

        public bool TryGetNearest(Vector3 position, out T nearest, out float distance)
        {
            nearest = null;
            distance = -1f;
            
            foreach (var unit in Units)
            {
                var currentDistance = Vector3.Distance(position, unit.transform.position);
                if (!(currentDistance < distance)) continue;
                
                nearest = unit;
                distance = currentDistance;
            }
            
            return nearest != null;
        }
    }
}