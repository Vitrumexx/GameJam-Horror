using UnityEngine;

namespace _Project.Scripts.Features.Shared
{
    [RequireComponent(typeof(Collider))]
    public abstract class Damagable : MonoBehaviour
    {
        public string damagableTag;

        public abstract void OnDamage();
    }
}