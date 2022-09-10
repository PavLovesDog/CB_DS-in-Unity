using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        private void Awake()
        {
            //search player model for weapon holder slots
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if(weaponSlot.isLeftHandSlot)
                {
                    // if left hand weapon slot variable is ticked on weapon, weapon will be assigned to left hand
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    //same as above, but for right hand
                    rightHandSlot = weaponSlot;
                }
            }
        }

        // function to load models onto character, dependent on if Left hand item or not
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if(isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
            }
        }
    }
    
}
