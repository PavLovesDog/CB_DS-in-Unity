using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB_DarkSouls
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;
        public float regenAmount;

        private void Start()
        {
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
        public void SetCurrentStamina(int currentStamina)
        {
            slider.value = currentStamina;
        }

        public void RegenStamina()
        {
            slider.value += regenAmount;
        }
    }

}

