using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB_DarkSouls
{
    public class StaminaBar : MonoBehaviour
    {
        public PlayerStats playerStats;
        public Slider slider;
        public float regenAmount;


        private void Start()
        {
            playerStats = FindObjectOfType<PlayerStats>(); // should only be one..
            slider = GetComponent<Slider>();
        }

        private void Update()
        {
            // slowly increse stamina bar back to top
            RegenStamina();
        }

        public void SetMaxStamina(int maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }

        // function which sets the slider display/visibility to the Stamina amount
        public void SetCurrentStamina(float currentStamina)
        {
            slider.value = currentStamina;
        }

        public void RegenStamina()
        {
            slider.value += regenAmount;
            if(playerStats.currentStamina < playerStats.maxStamina) // clamp at maxStamina
            {
                playerStats.currentStamina += regenAmount;
            }
        }
    }

}

