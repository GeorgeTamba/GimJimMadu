using UnityEngine;
namespace cowsins
{
    public class HurtTrigger : Trigger
    {
        [SerializeField] private float damage;
        public override void TriggerEnter(Collider other)
        {
            other.GetComponent<PlayerStats>().Damage(damage, false);
        }
    }
}