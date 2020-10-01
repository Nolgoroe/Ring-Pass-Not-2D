using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeManager : MonoBehaviour
{
    public GameObject ForgeItemPrefab, ForgeMaterialCellPrefab;

    public Transform ItemParent;

    //public Dropdown SortingDropdown;

    //public List<CraftingMaterials> AllEquipments;

    //public List<CraftingMaterials> SortedItems;

    //string SortingString;

    GameObject go;

    public List<ForgeItem> ForgeItems;

    void Start()
    {

        for (int i = 0; i < GameManager.Instance.GameItems.Length; i++)
        {
            go = Instantiate(ForgeItemPrefab, ItemParent);

            ForgedItemCell FIC = go.GetComponent<ForgeItem>().TheItem.GetComponent<ForgedItemCell>();

            ForgeItem FI = go.GetComponent<ForgeItem>();

            //FIcell.Full = true;

            FIC.EquipmentToCreate = GameManager.Instance.GameItems[i];

            go.name = FIC.EquipmentToCreate.name;

            FI.ForgeButton.onClick.AddListener(delegate { ForgeItem(FIC.EquipmentToCreate); });

            ForgeItems.Add(go.GetComponent<ForgeItem>());
        }
    }



    public void CheckIfCanCraftItem()
    {
        foreach (ForgeItem item in ForgeItems)
        {
            ForgedItemCell FIC = item.TheItem.GetComponent<ForgedItemCell>();

            for (int i = 0; i < FIC.EquipmentToCreate.MaterialsForCrafting.Count; i++)
            {
                bool HasMaterial = false;

                for (int k = 0; k < GameManager.Instance.ThePlayer.CraftingMatsInInventory.Count; k++)
                {
                    if (FIC.EquipmentToCreate.MaterialsForCrafting[i].Material == GameManager.Instance.ThePlayer.CraftingMatsInInventory[k].Material)
                    {
                        HasMaterial = true;
                        if (GameManager.Instance.ThePlayer.CraftingMatsInInventory[k].Amount < FIC.EquipmentToCreate.MaterialsForCrafting[i].Amount)
                        {
                            item.ForgeButton.interactable = false;
                            break;
                        }
                        else
                        {
                            item.ForgeButton.interactable = true;
                        }
                    }
                }

                if (!HasMaterial)
                {
                    item.ForgeButton.interactable = false;
                    break;
                }
            }
        }
    }


    public void ForgeItem(Equipment TheItem)
    {
        GameManager.Instance.ThePlayer.EquipmentInInventory.Add(TheItem);

        for (int i = 0; i < GameManager.Instance.ThePlayer.CraftingMatsInInventory.Count; i++)
        {
            for (int k = 0; k < TheItem.MaterialsForCrafting.Count; k++)
            {
                if (GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Material == TheItem.MaterialsForCrafting[k].Material)
                {
                    GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Amount -= TheItem.MaterialsForCrafting[k].Amount;

                    if (GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Amount <= 0)
                    {
                        GameManager.Instance.ThePlayer.CraftingMatsInInventory.Remove(GameManager.Instance.ThePlayer.CraftingMatsInInventory[i]);
                    }
                }
            }
        }


        RefreshForge();
        GameManager.Instance.MaterialBagManagerScript.RefreshMaterialBag();
    }


    public void RefreshForge()
    {
        for (int i = 0; i < ForgeItems.Count; i++)
        {
            ForgedItemCell FIC = ForgeItems[i].TheItem.GetComponent<ForgedItemCell>();

            for (int k = 0; k < ForgeItems[i].MaterialsForItem.Count; k++)
            {
                bool HasMaterial = false;

                for (int r = 0; r < GameManager.Instance.ThePlayer.CraftingMatsInInventory.Count; r++)
                {
                    if (ForgeItems[i].MaterialsForItem[k].CraftingMatInCell == GameManager.Instance.ThePlayer.CraftingMatsInInventory[r].Material)
                    {
                        HasMaterial = true;
                        ForgeItems[i].MaterialsForItem[k].CraftingMatCountText.text = GameManager.Instance.ThePlayer.CraftingMatsInInventory[r].Amount.ToString() + " / " + FIC.EquipmentToCreate.MaterialsForCrafting[k].Amount;


                        if (GameManager.Instance.ThePlayer.CraftingMatsInInventory[r].Amount >= FIC.EquipmentToCreate.MaterialsForCrafting[k].Amount)
                        {
                            ForgeItems[i].MaterialsForItem[k].CraftingMatCountText.color = Color.green;
                            ForgeItems[i].MaterialsForItem[k].HasEnough = true;
                        }
                        else
                        {
                            ForgeItems[i].MaterialsForItem[k].CraftingMatCountText.color = Color.red;
                            ForgeItems[i].MaterialsForItem[k].HasEnough = false;
                        }
                    }
                }

                if (!HasMaterial)
                {
                    ForgeItems[i].MaterialsForItem[k].CraftingMatCountText.text = 0 + " / " + FIC.EquipmentToCreate.MaterialsForCrafting[k].Amount;
                    ForgeItems[i].MaterialsForItem[k].CraftingMatCountText.color = Color.red;
                    ForgeItems[i].MaterialsForItem[k].HasEnough = false;
                }
            }
        }


        CheckIfCanCraftItem();
    }
}
