using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB_DarkSouls
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        // function which sets the slider display/visibility to the health amount
        public void SetCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }
    }
    
}
