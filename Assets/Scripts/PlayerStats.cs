using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public int maxStamina;
        public int currentStamina;

        public HealthBar healthBar;
        public StaminaBar staminaBar;

        AnimatorHandler animatorHandler;

        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        void Start()
        {
            maxHealth = SetMaxhealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth); // set value of health bar to characters max health
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina); //TODO Make a StaminaBar Script
            staminaBar.SetCurrentStamina(currentStamina);

        }

        //function to determine actual amount of health, derived from stats
        private int SetMaxhealthFromHealthLevel()
        {
            //TODO Balance and find my OWN formula for this
            // usually some formula is here to determint ACTUAL health amount
            //i.e 40 vigour stat = stat * healthAmount * 0.25 ArmourLevel/MagicItem / debuff. or simply, 40(stat) * 30(multiplier) = 1200 health
            maxHealth = healthLevel * 10;

            return maxHealth;
        }

        //function to determine actual amount of Stamina, derived from stats
        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;

            return maxStamina;
        }

        // function to handle damage taken on character
        public void TakeDamage(int damage)
        {
            // calculate damage taken
            currentHealth -= damage;

            //remove health from bar
            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            //Handle death
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.canRotate = false;
                animatorHandler.PlayTargetAnimation("Death_01", true);
                //TODO HANDLE PLAYER DEATH
            }
        }

        public void TakeStaminadamage(int damage)
        {
            currentStamina -= damage;
            staminaBar.SetCurrentStamina(currentStamina);
            //set bar value
        }
    
    }
    
}
