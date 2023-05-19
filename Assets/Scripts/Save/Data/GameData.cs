using FN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FM
{
    [System.Serializable]
    public class GameData
    {
        public string currentstage;
        public int gold;
        public int soul;
        public int healthLevel;
        public int staminaLevel;
        public int poiseLevel;

        [Header("Equipment")]
        public int currentRightHandWeaponID;
        public int currentLeftHandWeaponID;

        public int currentHeadGearItemID;
        public int currentArmorGearItemID;
        public int currentLegGearItemID;
        public int currentHandGearItemID;

        [Header("Inventory")]
        public int weaponsRSlots;
        public int weaponsLSlots;
        public int[] weaponsRSlotItems;
        public int[] weaponsLSlotItems;

        public List<int> weaponsInventorySlotItems;

        public GameData(string currentstage)
        {
            this.currentstage = currentstage;
            this.gold = 0;
            this.soul = 0;
            this.healthLevel = 1;
            this.staminaLevel = 1;
            this.poiseLevel = 1;
            this.weaponsRSlots = 2;
            this.weaponsLSlots = 2;
            this.weaponsRSlotItems = new int[this.weaponsRSlots];
            this.weaponsLSlotItems = new int[this.weaponsLSlots];
            this.weaponsRSlotItems[0] = 111; // Set the default weapon ID for the first slot
            this.weaponsLSlotItems[0] = -1;
            WeaponItem weapon = ItemDataBase.Instance.GetWeaponItemByID(this.weaponsRSlotItems[0]);
        }

    }

}
