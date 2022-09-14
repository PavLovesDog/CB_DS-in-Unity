using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;

        public int currentWeaponDamage = 25;

        private void Awake()
        {
            //on awake, assign the collider
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false; // collider itself can be enabled and disabled, but the GO itself cannot. this is important
        }

        // Functions to Enable & Disable damage collider
        // Turns the collider of weapon ON, to register damage
        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        // Turns the collider of weapon OFF, so no damage is dealt
        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Player") // hit player
            {
                //store player collision in variable
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();


                if(playerStats != null) // if there IS a hit with player
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }
                
            if (collision.tag == "Enemy") // hit enemy
            {
                //store Enemy collision variable
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();


                if (enemyStats != null) // if there IS a hit with enemy
                {
                    if(!enemyStats.isDead)
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }
        }

    }


}