using UnityEngine;

namespace cowsins
{
    public class PlayerMultipliers : MonoBehaviour
    {
        [HideInInspector] public float damageMultiplier, healMultiplier;

        private void Start()
        {
            damageMultiplier = 1;
            healMultiplier = 1;
        }

    }
}