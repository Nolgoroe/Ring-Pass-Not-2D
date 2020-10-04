using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WardrobeManager : MonoBehaviour
{
    public static WardrobeManager Instance;
    
    public GameObject EquipemntCellPrefab;

    public Transform EquipmentCellParent;

    public Dropdown SortingDropdown;

    public List<Equipment> AllEquipments;

    public List<Equipment> SortedItems;

    public List<EquipmentSlot> SortedEquippedItems;

    public List<EquipmentSlot> EquippedItems;

    string SortingString;


    private void Start()
    {
        Instance = this;
    }
    public void OpenWardrobe()
    {
        foreach (Transform cell in EquipmentCellParent)
        {
            if (!cell.GetComponent<EquipmentCell>().EquippedOnPlayer)
            {
                Destroy(cell.gameObject);
            }
        }

        AllEquipments.Clear();

        for (int i = 0; i < GameManager.Instance.ThePlayer.EquipmentInInventory.Count; i++)
        {
            GameObject go = Instantiate(EquipemntCellPrefab, EquipmentCellParent);

            EquipmentCell Eqcell = go.GetComponent<EquipmentCell>();
            Eqcell.Full = true;

            Eqcell.ItemInCell = GameManager.Instance.ThePlayer.EquipmentInInventory[i];

            //Eqcell.ItemName.text = GameManager.Instance.ThePlayer.EquipmentInInventory[i].name;

            Eqcell.TheTypeOfItem = GameManager.Instance.ThePlayer.EquipmentInInventory[i].TheTypeOfEquipment;

            Eqcell.ItemSprite.sprite = Resources.Load<Sprite>(GameManager.Instance.ThePlayer.EquipmentInInventory[i].ItemSpriteInventory);

            //if (GameManager.Instance.ThePlayer.EquipmentInInventory[i].HasTimeCooldown)
            //{
            //    //Eqcell.ItemUsesPerDay.text = "Uses Per Day: " + GameManager.Instance.ThePlayer.EquipmentInInventory[i].UsesBeforeTimeCountdown;
            //}
            //else
            //{
            //    //Eqcell.ItemUsesPerDay.text = "Uses Per Match: " + GameManager.Instance.ThePlayer.EquipmentInInventory[i].UsesInMatch;
            //}

            Equipment TheItem = go.GetComponent<EquipmentCell>().ItemInCell;

            AllEquipments.Add(TheItem);
        }
    }

    public void GetSortSting()
    {
        SortingString = SortingDropdown.options[SortingDropdown.value].text;
        
        if(SortingString == "All")
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
        SortedEquippedItems.Clear();

        foreach (Equipment item in AllEquipments)
        {
            SortedItems.Add(item);
        }

        foreach (EquipmentSlot item in EquippedItems)
        {
            SortedEquippedItems.Add(item);
        }

        RefreshInventory();
    }

    public void SortItemsByType(string Type)
    {
        SortedItems.Clear();
        SortedEquippedItems.Clear();

        foreach (Equipment item in AllEquipments)
        {
            if(item.TheTypeOfEquipment.ToString() == Type)
            {
                SortedItems.Add(item);
            }
        }

        foreach (EquipmentSlot item in EquippedItems)
        {
            if (item.TheItem.TheTypeOfEquipment.ToString() == Type)
            {
                SortedEquippedItems.Add(item);
            }
        }

        RefreshInventory();
    }

    public void RefreshInventory()
    {
        foreach (Transform cell in EquipmentCellParent)
        {
            if (!cell.GetComponent<EquipmentCell>().EquippedOnPlayer)
            {
                Destroy(cell.gameObject);
            }
        }


        for (int i = 0; i < SortedItems.Count; i++)
        {
            GameObject go = Instantiate(EquipemntCellPrefab, EquipmentCellParent);

            EquipmentCell Eqcell = go.GetComponent<EquipmentCell>();

            Eqcell.Full = true;

            Eqcell.ItemInCell = SortedItems[i];

            //Eqcell.ItemName.text = SortedItems[i].name;

            Eqcell.TheTypeOfItem = SortedItems[i].TheTypeOfEquipment;

            Eqcell.ItemSprite.sprite = Resources.Load<Sprite>(SortedItems[i].ItemSpriteInventory);

            if (GameManager.Instance.ThePlayer.EquipmentInInventory[i].HasTimeCooldown)
            {
                //Eqcell.ItemUsesPerDay.text = "Uses Per Day: " + SortedItems[i].UsesBeforeTimeCountdown;
            }
            else
            {
                //Eqcell.ItemUsesPerDay.text = "Uses Per Match: " + SortedItems[i].UsesInMatch;
            }
        }

        if(SortedEquippedItems.Count == 0)
        {
            foreach (EquipmentSlot item in EquippedItems)
            {
                item.OriginalCellFromInventory.gameObject.SetActive(false);
            }

        }
        else
        {
            foreach (EquipmentSlot item in EquippedItems)
            {
                
                if (!SortedEquippedItems.Contains(item))
                {
                    if (item.OriginalCellFromInventory)
                    {
                        item.OriginalCellFromInventory.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (item.OriginalCellFromInventory)
                    {
                        item.OriginalCellFromInventory.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
