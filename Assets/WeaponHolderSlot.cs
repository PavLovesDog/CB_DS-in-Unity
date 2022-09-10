using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOveride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public GameObject currentWeaponModel;

        //Function to Set an equiped weapon inactive
        public void UnloadWeapon()
        {
            if(currentWeaponModel != null)
            {
                //render it un-seeable in scene
                currentWeaponModel.SetActive(false);
            }
        }

        // function to "unequip" weapon from character model
        // this function destroys the weapon model GameObject
        public void UnloadWeaponAndDestroy()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        // Function to "Equip" a new weapon model passed in
        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            // if weapon item pass is null
            if(weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            //When unloading a new model, we need to destroy the old one
            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if(model != null)
            {
                //if there is a 'Partent' object
                if(parentOveride != null)
                {
                    // use its transform
                    model.transform.parent = parentOveride;
                }
                else
                {
                    // the transform of this model is equal to the transform of THIS script
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }
    }
    
}
