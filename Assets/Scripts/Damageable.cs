using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private UnityEvent<string, WeaponType, int> damageEvent;
        public void Damage(string name, WeaponType type, int amount)
        {
            damageEvent.Invoke(name, type, amount);
        }
    }
}