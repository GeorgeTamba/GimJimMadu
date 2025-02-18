/// <summary>
/// This script belongs to cowsins™ as a part of the cowsins´ FPS Engine. All rights reserved. 
/// </summary>
using UnityEngine;
using UnityEngine.UI;
namespace cowsins
{

    public class PowerUp : Trigger
    {
        [SerializeField] private bool reappears;

        [SerializeField] protected float reappearTime;

        [SerializeField] private Image Timer;

        [HideInInspector] public bool used;

        protected float timer = 0;

        private void Update()
        {
            if (!reappears && used)
            {
                Destroy(this.gameObject);
            }
            if (!used && Timer != null)
                Timer.gameObject.SetActive(false);
            else
            {
                if (Timer != null)
                {
                    Timer.gameObject.SetActive(true);
                }
                timer -= Time.deltaTime;

            }
            if (Timer == null) return;
            if (Timer.gameObject.activeSelf == true)
            {
                Timer.fillAmount = (reappearTime - timer) / reappearTime;
            }

            if (timer <= 0 && Timer != null) used = false;
        }
        public override void TriggerStay(Collider other)
        {
            if (!used)
            {
                Interact(other.GetComponent<PlayerMultipliers>());
            }
        }
        public virtual void Interact(PlayerMultipliers player)
        {
            // Override this
        }
    }
}