using _Project.Scripts.Features.Shared;
using UnityEngine;

namespace _Project.Scripts.Features.Items
{
    public class ItemStorableUnit : StorableUnit
    {
        public GameObject itemPrefab;
        public Sprite icon;
        public ItemWeight itemWeight = ItemWeight.Light;
        public string soundId;
        
        public enum ItemWeight
        {
            Heavy = 0,
            Light = 1
        }
    }
}