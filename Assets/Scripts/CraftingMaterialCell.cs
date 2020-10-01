using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMaterialCell : MonoBehaviour
{
    public bool Full;

    public CraftingMaterials CraftingMatInCell;

    public Text CraftingMatNameText, CraftingMatCountText;

    public ItemType TypeOfItem;

    public SubTypeOfItem SubTypeOfItem;

    public int CraftingMatCount;

    private void Start()
    {

        TypeOfItem = CraftingMatInCell.TypeOfItem;
        SubTypeOfItem = CraftingMatInCell.SubTypeOfItem;

        for (int i = 0; i < GameManager.Instance.ThePlayer.CraftingMatsInInventory.Count; i++)
        {
            if(CraftingMatInCell == GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Material)
            {
                CraftingMatCount = GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Amount;
            }
        }    

        CraftingMatCountText.text = "Item Count: " + CraftingMatCount.ToString();
    }
}

