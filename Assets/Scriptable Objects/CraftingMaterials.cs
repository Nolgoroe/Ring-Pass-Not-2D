using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    None,
    BasicMaterial,
    MagicalMaterial,
    Textile
}

public enum SubTypeOfItem
{
    None,
    Wood,
    Stone,
    FireShard,
    PurpleFlower,
    Feather
}

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Create Item")]
public class CraftingMaterials : ScriptableObject
{
    public int ID;
    public ItemType TypeOfItem;
    public SubTypeOfItem SubTypeOfItem;
    //public Sprite ItemSprite;

    public string ItemSpritePath;
}
