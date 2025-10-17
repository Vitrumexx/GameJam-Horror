using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Features.Shared
{
    public abstract class MonoStorage<T> : MonoBehaviour where T : StorableUnit
    {
        [SerializeField] private List<T> units;

        public Dictionary<string, T> Units { get; } = new();

        protected virtual void Awake()
        {
            foreach (var item in units)
            {
                if (!Units.TryAdd(item.id, item))
                {
                    Debug.LogWarning($"Duplicate item id {item.id}!");
                }
            }
        }
    }
}