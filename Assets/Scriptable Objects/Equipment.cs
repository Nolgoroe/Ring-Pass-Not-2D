using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Create Equipment")]
public class Equipment : ScriptableObject
{

    public int ID;

    public EquipmentSlotType SlotForEquipment;

    public float CoolDownTimeHours;

    public int UsesInMatch;

    public int UsesBeforeTimeCountdown;

    public int UsesBeforeDestruction;

    public Sprite SpriteOfEquipment;

    public PowerUpChooseItemTypes PowerUpToGive;

    public ColorData ColorForPowerUp;

    public Symbols SymbolForPowerUp;

    public bool HasTimeCooldown;

    public TypeOfequipment TheTypeOfEquipment;

}
