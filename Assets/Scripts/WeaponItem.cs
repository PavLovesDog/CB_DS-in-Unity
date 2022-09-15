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

        [Header("Idle Animations")]
        public string Right_Arm_Idle_01;
        public string Left_Arm_Idle_01;

        [Header("On Handed Attack Animations")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Heavy_Attack_1;

    }
    
}
