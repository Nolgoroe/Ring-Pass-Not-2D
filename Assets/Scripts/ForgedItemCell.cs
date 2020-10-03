using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgedItemCell : MonoBehaviour
{
    public Equipment EquipmentToCreate;

    public Text ItemName, ItemUsesPerDay;

    public TypeOfequipment TheTypeOfItem;

    //public int CraftingMatCount;

    public Image ItemSprite;

    private void Start()
    {
        TheTypeOfItem = EquipmentToCreate.TheTypeOfEquipment;
        ItemName.text = EquipmentToCreate.name;

        if (EquipmentToCreate.HasTimeCooldown)
        {
            ItemUsesPerDay.text = "Uses Per Day: " + EquipmentToCreate.UsesBeforeTimeCountdown;
        }
        else
        {
            ItemUsesPerDay.text = "Uses Per Match: " + EquipmentToCreate.UsesInMatch;
        }
        ItemSprite.sprite = EquipmentToCreate.SpriteOfEquipment;
    }
}

