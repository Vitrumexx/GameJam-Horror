using UnityEngine;

namespace _Project.Scripts.Features.Damagables
{
    [RequireComponent(typeof(Collider))]
    public abstract class Damagable : MonoBehaviour
    {
        public string damagableTag;

        public abstract void OnDamage();
    }
}