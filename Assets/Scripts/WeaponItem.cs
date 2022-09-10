using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    // A class to contain a weapon prefab

    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

    }
    
}
