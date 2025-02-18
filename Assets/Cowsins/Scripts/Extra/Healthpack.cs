/// <summary>
/// This script belongs to cowsins™ as a part of the cowsins´ FPS Engine. All rights reserved. 
/// </summary>
using UnityEngine;
namespace cowsins
{
    public class Healthpack : PowerUp
    {
        [Tooltip("Amount of health to be restored")] [Range(.1f, 1000), SerializeField] private float healAmount;
        public override void Interact(PlayerMultipliers player)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats.MaxShield == 0 && playerStats.health == playerStats.MaxHealth || playerStats.MaxShield != 0 && playerStats.shield == playerStats.MaxShield) return;
            used = true;
            timer = reappearTime;
            playerStats.Heal(healAmount);
        }

    }
}