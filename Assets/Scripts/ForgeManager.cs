using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    string SortingString;

    public Dropdown SortingDropdown;

    public List<Equipment> SortedItems; ///// Used For Sorting

    public List<Equipment> AllEquipments; ///// Used For Sorting

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

            AllEquipments.Add(FIC.EquipmentToCreate);
        }
    }


    public void CheckIfCanCraftItem()
    {
        foreach (ForgeItem item in ForgeItems)
        {
            ForgedItemCell FIC = item.TheItem.GetComponent<ForgedItemCell>();

            for (int i = 0; i < FIC.EquipmentToCreate.MaterialsForCrafting.Count; i++)
            {
                bool HasEnough = false;

                for (int k = 0; k < GameManager.Instance.ThePlayer.CraftingMatsInInventory.Count; k++)
                {
                    if (FIC.EquipmentToCreate.MaterialsForCrafting[i].Material.ID == GameManager.Instance.ThePlayer.CraftingMatsInInventory[k].Material.ID)
                    {
                        if (GameManager.Instance.ThePlayer.CraftingMatsInInventory[k].Amount < FIC.EquipmentToCreate.MaterialsForCrafting[i].Amount)
                        {
                            item.ForgeButton.interactable = false;
                            item.ForgeButton.transform.GetChild(0).GetComponent<Text>().color = Color.grey;
                            break;
                        }
                        else
                        {
                            HasEnough = true;
                            item.ForgeButton.interactable = true;
                            item.ForgeButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                            break;
                        }
                    }
                }

                if (!HasEnough)
                {
                    item.ForgeButton.interactable = false;
                    item.ForgeButton.transform.GetChild(0).GetComponent<Text>().color = Color.grey;
                    break;
                }
            }
        }
    }


    public void ForgeItem(Equipment TheItem)
    {
        GameManager.Instance.ThePlayer.EquipmentInInventory.Add(TheItem);

        foreach (ForgeItem item in ForgeItems)
        {
            if(item.TheItem.GetComponent<ForgedItemCell>().EquipmentToCreate.ID == TheItem.ID)
            {
                Destroy(item.gameObject);

                ForgeItems.Remove(item);
                AllEquipments.Remove(TheItem);
                break;
            }
        }

        for (int i = 0; i < GameManager.Instance.ThePlayer.CraftingMatsInInventory.Count; i++)
        {
            for (int k = 0; k < TheItem.MaterialsForCrafting.Count; k++)
            {
                if (GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Material.ID == TheItem.MaterialsForCrafting[k].Material.ID)
                {
                    GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Amount -= TheItem.MaterialsForCrafting[k].Amount;

                    if (GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Amount <= 0)
                    {
                        GameManager.Instance.ThePlayer.CraftingMatsInInventory.Remove(GameManager.Instance.ThePlayer.CraftingMatsInInventory[i]);
                    }
                }
            }
        }

        GameManager.Instance.ThePlayer.SaveMatsInInventory();

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
                    if (ForgeItems[i].MaterialsForItem[k].CraftingMatInCell.ID == GameManager.Instance.ThePlayer.CraftingMatsInInventory[r].Material.ID)
                    {
                        HasMaterial = true;
                        ForgeItems[i].MaterialsForItem[k].CraftingMatCountText.text = GameManager.Instance.ThePlayer.CraftingMatsInInventory[r].Amount.ToString() + " / " + FIC.EquipmentToCreate.MaterialsForCrafting[k].Amount;


                        if (GameManager.Instance.ThePlayer.CraftingMatsInInventory[r].Amount >= FIC.EquipmentToCreate.MaterialsForCrafting[k].Amount)
                        {
                            //ForgeItems[i].MaterialsForItem[k].CraftingMatCountText.color = Color.black;
                            ForgeItems[i].MaterialsForItem[k].HasEnough = true;
                        }
                        else
                        {
                            //ForgeItems[i].MaterialsForItem[k].CraftingMatCountText.color = Color.red;
                            ForgeItems[i].MaterialsForItem[k].CountBG.color = Color.red;
                            ForgeItems[i].MaterialsForItem[k].HasEnough = false;
                        }
                    }
                }

                if (!HasMaterial)
                {
                    ForgeItems[i].MaterialsForItem[k].CraftingMatCountText.text = 0 + " / " + FIC.EquipmentToCreate.MaterialsForCrafting[k].Amount;
                    //ForgeItems[i].MaterialsForItem[k].CraftingMatCountText.color = Color.red;
                    ForgeItems[i].MaterialsForItem[k].CountBG.color = Color.red;
                    ForgeItems[i].MaterialsForItem[k].HasEnough = false;
                }
            }
        }


        CheckIfCanCraftItem();
    }

    public void GetSortSting()
    {
        SortingString = SortingDropdown.options[SortingDropdown.value].text;

        if (SortingString == "All")
        {
            SortItemsAll();
        }
        else
        {
            SortItemsByType(SortingString);
        }
    }

    public void SortItemsAll()
    {
        SortedItems.Clear();

        foreach (Equipment item in AllEquipments)
        {
            SortedItems.Add(item);
        }

        SortForge();
    }

    public void SortItemsByType(string Type)
    {
        SortedItems.Clear();

        foreach (Equipment item in AllEquipments)
        {
            if (item.TheTypeOfEquipment.ToString() == Type)
            {
                SortedItems.Add(item);
            }
        }

        SortForge();
    }

    public void SortForge()
    {
        foreach (Transform cell in ItemParent)
        {
            Destroy(cell.gameObject);
        }

        ForgeItems.Clear();

        for (int i = 0; i < SortedItems.Count; i++)
        {
            GameObject go = Instantiate(ForgeItemPrefab, ItemParent);

            ForgedItemCell FIC = go.GetComponent<ForgeItem>().TheItem.GetComponent<ForgedItemCell>();

            ForgeItem FI = go.GetComponent<ForgeItem>();

            FIC.EquipmentToCreate = SortedItems[i];

            go.name = FIC.EquipmentToCreate.name;

            FI.ForgeButton.onClick.AddListener(delegate { ForgeItem(FIC.EquipmentToCreate); });

            ForgeItems.Add(go.GetComponent<ForgeItem>());
        }

        CheckIfCanCraftItem();
    }

    public void SummonAfterDestruction(Equipment Item)
    {
        GameObject go = Instantiate(ForgeItemPrefab, ItemParent);

        ForgedItemCell FIC = go.GetComponent<ForgeItem>().TheItem.GetComponent<ForgedItemCell>();

        ForgeItem FI = go.GetComponent<ForgeItem>();

        FIC.EquipmentToCreate = Item;

        go.name = FIC.EquipmentToCreate.name;

        FI.ForgeButton.onClick.AddListener(delegate { ForgeItem(FIC.EquipmentToCreate); });

        ForgeItems.Add(FI);
        AllEquipments.Add(FIC.EquipmentToCreate);

        CheckIfCanCraftItem();
    }
}
