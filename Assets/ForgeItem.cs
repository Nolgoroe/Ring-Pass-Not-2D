using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgeItem : MonoBehaviour
{
    public GameObject TheItem, MaterialsParent;

    public Button ForgeButton;

    public List<ForgeMaterialCell> MaterialsForItem;

    GameObject go;

    ForgeMaterialCell FMC;

    ForgedItemCell ItemToCraft;

    bool CanCraft;

    private void Start()
    {
        ItemToCraft = TheItem.GetComponent<ForgedItemCell>();

        for (int i = 0; i < ItemToCraft.EquipmentToCreate.MaterialsForCrafting.Count; i++)
        {
            bool HasMaterial = false;

            go = Instantiate(GameManager.Instance.ForgeManagerScript.ForgeMaterialCellPrefab, MaterialsParent.transform);

            FMC = go.GetComponent<ForgeMaterialCell>();

            FMC.CraftingMatInCell = ItemToCraft.EquipmentToCreate.MaterialsForCrafting[i].Material;

            for (int k = 0; k < GameManager.Instance.ThePlayer.CraftingMatsInInventory.Count; k++)
            {
                if (ItemToCraft.EquipmentToCreate.MaterialsForCrafting[i].Material == GameManager.Instance.ThePlayer.CraftingMatsInInventory[k].Material)
                {
                    HasMaterial = true;
                    FMC.CraftingMatCountText.text = (GameManager.Instance.ThePlayer.CraftingMatsInInventory[k].Amount.ToString() + " / " + ItemToCraft.EquipmentToCreate.MaterialsForCrafting[i].Amount);

                    if(GameManager.Instance.ThePlayer.CraftingMatsInInventory[k].Amount >= ItemToCraft.EquipmentToCreate.MaterialsForCrafting[i].Amount)
                    {
                        //FMC.CraftingMatCountText.color = Color.green;
                        FMC.HasEnough = true;
                    }
                    else
                    {
                        //FMC.CraftingMatCountText.color = Color.red;
                        FMC.CountBG.color = Color.red;
                        FMC.HasEnough = false;
                    }
                }
            }

            if (!HasMaterial)
            {
                FMC.CraftingMatCountText.text = 0 + " / " + ItemToCraft.EquipmentToCreate.MaterialsForCrafting[i].Amount;
                //FMC.CraftingMatCountText.color = Color.red;
                FMC.CountBG.color = Color.red;
                FMC.HasEnough = false;
            }

            MaterialsForItem.Add(FMC);
        }
    }
}
