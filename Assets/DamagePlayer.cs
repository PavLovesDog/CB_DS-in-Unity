using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class DamagePlayer : MonoBehaviour
    {
        private int damage = 25;
        private void OnTriggerEnter(Collider other)
        {
            // get player stats component of object colliding with THIS object
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            // if it is the player (i.e only player has the "PlayerStats" script)
            if(playerStats != null)
            {
                // call the take damage function
                playerStats.TakeDamage(damage);
            }
        }
    }

}
