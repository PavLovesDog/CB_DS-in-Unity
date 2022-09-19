using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB_DarkSouls
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;

        public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
        {
            if(isLeft == false) // RIGHT HAND
            {
                if(weapon.itemIcon != null) // if weapon DOES have an icon
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else // no icon available
                {
                    //disable image, set sprite to null
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
            else // LEFT HAND
            {
                if (weapon.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
        }
    }
    
}

