using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaterialBagManager : MonoBehaviour
{
    //public static MaterialBagManager Instance;

    public GameObject MaterialCellPrefab;

    public Transform BasicMaterialParent, MagicalItemParent, TextileParent;

    //public Dropdown SortingDropdown;

    //public List<CraftingMaterials> AllEquipments;

    //public List<CraftingMaterials> SortedItems;

    //string SortingString;

    GameObject go;

    public void RefreshMaterialBag()
    {
        foreach (Transform child in BasicMaterialParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in MagicalItemParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in TextileParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < GameManager.Instance.ThePlayer.CraftingMatsInInventory.Count; i++)
        {
            switch (GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Material.TypeOfItem)
            {
                case ItemType.None:
                    break;
                case ItemType.BasicMaterial:
                    go = Instantiate(MaterialCellPrefab, BasicMaterialParent);
                    break;
                case ItemType.MagicalMaterial:
                    go = Instantiate(MaterialCellPrefab, MagicalItemParent);
                    break;
                case ItemType.Textile:
                    go = Instantiate(MaterialCellPrefab, TextileParent);
                    break;
                default:
                    break;
            }

            CraftingMaterialCell CMcell = go.GetComponent<CraftingMaterialCell>();

            CMcell.Full = true;

            CMcell.CraftingMatInCell = GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Material;

            //CMcell.CraftingMatNameText.text = GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Material.name;

            CMcell.TypeOfItem = GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Material.TypeOfItem;

            CMcell.SubTypeOfItem = GameManager.Instance.ThePlayer.CraftingMatsInInventory[i].Material.SubTypeOfItem;

        }

    }
}

