using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;

        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        public WeaponItem unarmedWeapon;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2]; // give us 2 slots to work with
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];

        public int currentRightWeaponIndex = -1;
        public int currentLeftWeaponIndex = -1;

        //make weapon inventory
        public List<WeaponItem> weaponsInventory;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            // start off unarmed
            ////declare weapons
            rightWeapon = unarmedWeapon;
            leftWeapon = unarmedWeapon;
            //rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            //leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];

            //// load weapons AFter they are declared which is which
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);

        }

        // Function to switch weapons in right hand weapon slot
        public void ChangeRightWeapon()
        {
            // some Youtube dudes code: patxor
            currentRightWeaponIndex = currentRightWeaponIndex + 1;

            if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = -1;
                rightWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            }
            else if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            #region Sebastions Code
            //// increment index
            //currentRightWeaponIndex = currentRightWeaponIndex + 1;
            //
            //// slot 1
            //if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
            //{
            //    //load next weapon
            //    rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            //    weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            //}
            //else if(currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
            //{
            //    //if no weapon in slot, go to next slot
            //    currentRightWeaponIndex++;
            //}
            ////slot 2
            //else if(currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
            //{
            //    rightWeapon= weaponsInRightHandSlots[currentRightWeaponIndex];
            //    weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            //}
            //else
            //{
            //    currentRightWeaponIndex++;
            //}
            //
            //// unarmed slot
            //if(currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            //{
            //    currentRightWeaponIndex = -1;
            //    rightWeapon = unarmedWeapon;
            //    weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            //}
            #endregion
        }

        // Function to switch weapons in left hand weapon slot
        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
            }
            else if (weaponsInLeftHandSlots[currentLeftWeaponIndex] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            // below was hard coded for 2 inventory slots. above can dynamically add more weapons on player 
            #region Sebastions Old Code
            //// increment index
            //currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            //
            //// slot 1
            //if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
            //{
            //    //load next weapon
            //    leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            //    weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            //}
            //else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
            //{
            //    //if no weapon in slot, go to next slot
            //    currentLeftWeaponIndex++;
            //}
            ////slot 2
            //else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
            //{
            //    leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            //    weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            //}
            //else
            //{
            //    currentLeftWeaponIndex++;
            //}
            //
            //// unarmed slot
            //if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            //{
            //    currentLeftWeaponIndex = -1;
            //    leftWeapon = unarmedWeapon;
            //    weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            //}
            #endregion
        }
    }
    
}
