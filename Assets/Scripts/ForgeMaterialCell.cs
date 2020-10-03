using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ForgeMaterialCell : MonoBehaviour
{
    public CraftingMaterials CraftingMatInCell;

    public Text CraftingMatNameText, CraftingMatCountText;

    public ItemType TypeOfItem;

    public SubTypeOfItem SubTypeOfItem;

    public bool HasEnough;

    public Image ItemSprite;

    public Image CountBG;

    void Start()
    {
        TypeOfItem = CraftingMatInCell.TypeOfItem;
        SubTypeOfItem = CraftingMatInCell.SubTypeOfItem;

        CraftingMatNameText.text = CraftingMatInCell.name;
        ItemSprite.sprite = CraftingMatInCell.ItemSprite;
    }
}
