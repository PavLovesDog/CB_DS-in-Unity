using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;

        // drawe a sphere around the interactable item
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        // virtual means that any other class that derives from interactable, can override this void and chqagne it
        public virtual void Interact(PlayerManager playerManager)
        {
            //Called when player interacts
            Debug.Log("You interacted with an object");
        }
    }
}
