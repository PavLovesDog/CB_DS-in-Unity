using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class EnemyStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth = 10;
        public int currentHealth;
        public bool isDead;

        Animator animator;
        //Collider collider;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            //collider = GetComponentInChildren<Collider>();
        }

        private void Start()
        {
            isDead = false;
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;

            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            animator.Play("Damage_01");

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death_01");
                isDead = true;
                //collider.enabled = false; // stop colliding ? no more hits
                //HANDLE ENEMY DEATH
            }
        }
    }
    
}
