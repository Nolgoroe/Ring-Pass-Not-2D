using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum EquipmentSlotType
{
    Head,
    Body,
    Finger,
    MainHand,
    OffHand
}

public enum TypeOfequipment
{
    None,
    Staff,
    Ring,
    Hat,
    Chestplate
}

[Serializable]
public class ItemAmount
{
    public CraftingMaterials Material;
    public int Amount;

    public ItemAmount(CraftingMaterials material, int Amount)
    {
        Material = material;
        this.Amount = Amount;
    }
}

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Create Equipment")]
public class Equipment : ScriptableObject
{

    public int ID;

    public EquipmentSlotType SlotForEquipment;

    public double CoolDownTimeHours;

    public int UsesInMatch;

    public int UsesBeforeTimeCountdown;

    public int UsesBeforeDestruction;

    //public Sprite SpriteOfEquipment;

    public  List<PowerUpChooseItemTypes> PowerUpToGive;

    public ColorData ColorForPowerUp;

    public Symbols SymbolForPowerUp;

    public bool HasTimeCooldown;

    public TypeOfequipment TheTypeOfEquipment;

    public List<ItemAmount> MaterialsForCrafting;

    //public Sprite LookWhenEquipped;

    public string ItemSpriteInventory;


    public string ItemSpritePathWhenEquipped;

}
