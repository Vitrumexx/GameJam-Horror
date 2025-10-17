using UnityEngine.Events;

namespace _Project.Scripts.Features.Damagables
{
    public class BaseDamage : Damagable
    {
        public UnityEvent onDamage;
        
        public override void OnDamage()
        {
            onDamage?.Invoke();
        }
    }
}