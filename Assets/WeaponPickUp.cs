using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB_DarkSouls
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        //this ovveride void, will overtake the virtual void in Interactable
        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            // Pick up the item weapon & add to item inventoryt
            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            AnimatorHandler animatorHandler;
            //use player manager to get the inventory & locomotion
            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            playerLocomotion.rigidbody.velocity = Vector3.zero; // stop movement while picking up item!
            animatorHandler.PlayTargetAnimation("Pick_Up_Item", true); // plays animation of looting item
            playerInventory.weaponsInventory.Add(weapon); // add to inventory
            //Handle UI Pop ups
            playerManager.IteminteractableUIGameObject.GetComponentInChildren<TMP_Text>().text = weapon.itemName; // set ppop up item name to the weapon item
            playerManager.IteminteractableUIGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture; // set icon
            playerManager.IteminteractableUIGameObject.SetActive(true); // activate it!
            Destroy(gameObject);
        }
    }
}
