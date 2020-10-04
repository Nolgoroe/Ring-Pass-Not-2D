using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System;


public class PlayerData : MonoBehaviour
{
    public int Gold, Rubies, MagicalItems, MaxLevelReached;

    public EquipmentSlot[] SlotsForEquipment;

    public List<PowerUpChooseItemTypes> PowerUpsFromItems;

    public List<Equipment> EquipmentWithTimeCooldown;

    public List<Equipment> EquipmentInInventory;

    public List<ItemAmount> CraftingMatsInInventory;

    public List<Equipment> EquippedItems;

    JsonData MatsInInventorySave;

    JsonData MatsInInventoryLoad;
    string MaterialsInInventoryLoadString;

    JsonData EquipmentInInventorySave;
    JsonData EquippedEquipmentInInventorySave;

    JsonData EquipmentInventoryLoad;
    string EquipmentInInventoryLoadString;

    JsonData EquippedEquipmentInInventoryLoad;
    string EquippedEquipmentInInventoryLoadString;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            Gold = PlayerPrefs.GetInt("Gold");
        }

        if (PlayerPrefs.HasKey("Rubies"))
        {
            Rubies = PlayerPrefs.GetInt("Rubies");
        }

        if (PlayerPrefs.HasKey("MagicalItems"))
        {
            MagicalItems = PlayerPrefs.GetInt("MagicalItems");
        }

        if (PlayerPrefs.HasKey("MaxLevelReached"))
        {
            MaxLevelReached = PlayerPrefs.GetInt("MaxLevelReached");
        }

        if (PlayerPrefs.HasKey("ItemsWithCooldownCount"))
        {
            int count = PlayerPrefs.GetInt("ItemsWithCooldownCount");

            for (int i = 0; i < count; i++)
            {
                for (int k = 0; k < GameManager.Instance.GameItems.Length; k++)
                {
                    if (PlayerPrefs.GetInt("ItemID" + EquipmentManager.Instance.PositionsInPlayerPrefs[i]) == GameManager.Instance.GameItems[k].ID)
                    {
                        EquipmentWithTimeCooldown.Add(GameManager.Instance.GameItems[k]);
                    }
                }
            }
        }


        LoadMatsInInventory();
    }

    public void SaveDate()
    {
        PlayerPrefs.SetInt("Gold", Gold);
        PlayerPrefs.SetInt("Rubies", Rubies);
        PlayerPrefs.SetInt("MagicalItems", MagicalItems);
        PlayerPrefs.SetInt("MaxLevelReached", MaxLevelReached);
    }


    [ContextMenu("Reset Player Prefs")]
    public void ResetPlayerPref()
    {
        PlayerPrefs.DeleteAll();
    }

    [ContextMenu("Reset Player Equipment")]
    public void ResetEquipmentData()
    {
        PlayerPrefs.DeleteKey("ItemsWithCooldownCount");
        PlayerPrefs.DeleteKey("ItemID");
        PlayerPrefs.DeleteKey("NextTimePowerUpAvailable");
    }


    [ContextMenu("Save mats in inventroy")]
    public void SaveMatsInInventory()
    {
        MatsInInventorySave = JsonMapper.ToJson(CraftingMatsInInventory);

        File.WriteAllText(Application.dataPath + "/CraftingMatsInInventory.Json", MatsInInventorySave.ToString());
    }

    public void LoadMatsInInventory()
    {
        MaterialsInInventoryLoadString = File.ReadAllText(Application.dataPath + "/CraftingMatsInInventory.Json");

        MatsInInventoryLoad = JsonMapper.ToObject(MaterialsInInventoryLoadString);

        //CraftingMatsInInventory = JsonMapper.ToObject<List<ItemAmount>>(MaterialsInInventoryLoadString);

        for (int i = 0; i < MatsInInventoryLoad.Count; i++)
        {
            CraftingMaterials Mat = (CraftingMaterials)ScriptableObject.CreateInstance("CraftingMaterials");

            //CraftingMaterials Mat = new CraftingMaterials();
            Mat.name = MatsInInventoryLoad[i]["Material"]["name"].ToString();
            Mat.ID = (int)(MatsInInventoryLoad[i]["Material"]["ID"]);
            Mat.TypeOfItem = (ItemType)(int)(MatsInInventoryLoad[i]["Material"]["TypeOfItem"]);
            Mat.SubTypeOfItem = (SubTypeOfItem)(int)(MatsInInventoryLoad[i]["Material"]["SubTypeOfItem"]);
            Mat.ItemSpritePath = MatsInInventoryLoad[i]["Material"]["ItemSpritePath"].ToString();


            int amount = (int)(MatsInInventoryLoad[i]["Amount"]);

            CraftingMatsInInventory.Add(new ItemAmount(Mat, amount));

        }
    }

    [ContextMenu("Save equipment in inventroy")]
    public void SaveEquipmentInInventory()
    {
        EquipmentInInventorySave = JsonMapper.ToJson(EquipmentInInventory);
        EquippedEquipmentInInventorySave = JsonMapper.ToJson(EquippedItems);

        File.WriteAllText(Application.dataPath + "/EquipmentInInventory.Json", EquipmentInInventorySave.ToString());
        File.WriteAllText(Application.dataPath + "/EquippedEquipmentInInventory.Json", EquippedEquipmentInInventorySave.ToString());
    }


    [ContextMenu("Load equipment in inventroy")]
    public void LoadequipmentInInventory()
    {
        EquipmentInInventoryLoadString = File.ReadAllText(Application.dataPath + "/EquipmentInInventory.Json");
        EquippedEquipmentInInventoryLoadString = File.ReadAllText(Application.dataPath + "/EquippedEquipmentInInventory.Json");

        //EquipmentInInventory = JsonMapper.ToObject<List<Equipment>>(EquipmentInInventoryLoadString);

        EquipmentInventoryLoad = JsonMapper.ToObject(EquipmentInInventoryLoadString);
        JsonData EquippedEquipmentInInventoryLoad = JsonMapper.ToObject(EquippedEquipmentInInventoryLoadString);


        for (int i = 0; i < EquipmentInventoryLoad.Count; i++)
        {
            Equipment item = (Equipment)ScriptableObject.CreateInstance("Equipment");

            for (int k = 0; k < GameManager.Instance.GameItems.Length; k++)
            {
                if (GameManager.Instance.GameItems[k].ID == (int)EquipmentInventoryLoad[i]["ID"])
                {

                    //Equipment item = new Equipment();

                    item.name = EquipmentInventoryLoad[i]["name"].ToString();
                    item.ID = (int)EquipmentInventoryLoad[i]["ID"];
                    item.SlotForEquipment = (EquipmentSlotType)(int)EquipmentInventoryLoad[i]["SlotForEquipment"];
                    item.CoolDownTimeHours = (double)EquipmentInventoryLoad[i]["CoolDownTimeHours"];
                    item.UsesInMatch = (int)EquipmentInventoryLoad[i]["UsesInMatch"];
                    item.UsesBeforeTimeCountdown = (int)EquipmentInventoryLoad[i]["UsesBeforeTimeCountdown"];
                    item.UsesBeforeDestruction = (int)EquipmentInventoryLoad[i]["UsesBeforeDestruction"];

                    item.PowerUpToGive = new List<PowerUpChooseItemTypes>();
                    for (int t = 0; t < EquipmentInventoryLoad[i]["PowerUpToGive"].Count; t++)
                    {
                        item.PowerUpToGive.Add((PowerUpChooseItemTypes)(int)EquipmentInventoryLoad[i]["PowerUpToGive"][t]);
                    }

                    item.ColorForPowerUp = (ColorData)(int)EquipmentInventoryLoad[i]["ColorForPowerUp"];
                    item.SymbolForPowerUp = (Symbols)(int)EquipmentInventoryLoad[i]["SymbolForPowerUp"];
                    item.HasTimeCooldown = (bool)EquipmentInventoryLoad[i]["HasTimeCooldown"];
                    item.TheTypeOfEquipment = (TypeOfequipment)(int)EquipmentInventoryLoad[i]["TheTypeOfEquipment"];

                    item.MaterialsForCrafting = new List<ItemAmount>();

                    for (int r = 0; r < EquipmentInventoryLoad[i]["MaterialsForCrafting"].Count; r++)
                    {
                        CraftingMaterials Mat = (CraftingMaterials)ScriptableObject.CreateInstance("CraftingMaterials");

                        Mat.name = EquipmentInventoryLoad[i]["MaterialsForCrafting"][r]["Material"]["name"].ToString();
                        Mat.ID = (int)(EquipmentInventoryLoad[i]["MaterialsForCrafting"][r]["Material"]["ID"]);
                        Mat.TypeOfItem = (ItemType)(int)(EquipmentInventoryLoad[i]["MaterialsForCrafting"][r]["Material"]["TypeOfItem"]);
                        Mat.SubTypeOfItem = (SubTypeOfItem)(int)(EquipmentInventoryLoad[i]["MaterialsForCrafting"][r]["Material"]["SubTypeOfItem"]);
                        Mat.ItemSpritePath = EquipmentInventoryLoad[i]["MaterialsForCrafting"][r]["Material"]["ItemSpritePath"].ToString();


                        int amount = (int)(EquipmentInventoryLoad[i]["MaterialsForCrafting"][r]["Amount"]);

                        item.MaterialsForCrafting.Add(new ItemAmount(Mat, amount));

                    }

                    item.ItemSpriteInventory = EquipmentInventoryLoad[i]["ItemSpriteInventory"].ToString();
                    item.ItemSpritePathWhenEquipped = EquipmentInventoryLoad[i]["ItemSpritePathWhenEquipped"].ToString();
                }
            }

            EquipmentInInventory.Add(item);
        }


        for (int i = 0; i < EquippedEquipmentInInventoryLoad.Count; i++)
        {
            Equipment item = (Equipment)ScriptableObject.CreateInstance("Equipment");

            for (int k = 0; k < GameManager.Instance.GameItems.Length; k++)
            {
                if (GameManager.Instance.GameItems[k].ID == (int)EquippedEquipmentInInventoryLoad[i]["ID"])
                {

                    //Equipment item = new Equipment();

                    item.name = EquippedEquipmentInInventoryLoad[i]["name"].ToString();
                    item.ID = (int)EquippedEquipmentInInventoryLoad[i]["ID"];
                    item.SlotForEquipment = (EquipmentSlotType)(int)EquippedEquipmentInInventoryLoad[i]["SlotForEquipment"];
                    item.CoolDownTimeHours = (double)EquippedEquipmentInInventoryLoad[i]["CoolDownTimeHours"];
                    item.UsesInMatch = (int)EquippedEquipmentInInventoryLoad[i]["UsesInMatch"];
                    item.UsesBeforeTimeCountdown = (int)EquippedEquipmentInInventoryLoad[i]["UsesBeforeTimeCountdown"];
                    item.UsesBeforeDestruction = (int)EquippedEquipmentInInventoryLoad[i]["UsesBeforeDestruction"];

                    item.PowerUpToGive = new List<PowerUpChooseItemTypes>();
                    for (int t = 0; t < EquippedEquipmentInInventoryLoad[i]["PowerUpToGive"].Count; t++)
                    {
                        item.PowerUpToGive.Add((PowerUpChooseItemTypes)(int)EquippedEquipmentInInventoryLoad[i]["PowerUpToGive"][t]);
                    }

                    item.ColorForPowerUp = (ColorData)(int)EquippedEquipmentInInventoryLoad[i]["ColorForPowerUp"];
                    item.SymbolForPowerUp = (Symbols)(int)EquippedEquipmentInInventoryLoad[i]["SymbolForPowerUp"];
                    item.HasTimeCooldown = (bool)EquippedEquipmentInInventoryLoad[i]["HasTimeCooldown"];
                    item.TheTypeOfEquipment = (TypeOfequipment)(int)EquippedEquipmentInInventoryLoad[i]["TheTypeOfEquipment"];

                    item.MaterialsForCrafting = new List<ItemAmount>();

                    for (int r = 0; r < EquippedEquipmentInInventoryLoad[i]["MaterialsForCrafting"].Count; r++)
                    {
                        CraftingMaterials Mat = (CraftingMaterials)ScriptableObject.CreateInstance("CraftingMaterials");

                        Mat.name = EquippedEquipmentInInventoryLoad[i]["MaterialsForCrafting"][r]["Material"]["name"].ToString();
                        Mat.ID = (int)(EquippedEquipmentInInventoryLoad[i]["MaterialsForCrafting"][r]["Material"]["ID"]);
                        Mat.TypeOfItem = (ItemType)(int)(EquippedEquipmentInInventoryLoad[i]["MaterialsForCrafting"][r]["Material"]["TypeOfItem"]);
                        Mat.SubTypeOfItem = (SubTypeOfItem)(int)(EquippedEquipmentInInventoryLoad[i]["MaterialsForCrafting"][r]["Material"]["SubTypeOfItem"]);
                        Mat.ItemSpritePath = EquippedEquipmentInInventoryLoad[i]["MaterialsForCrafting"][r]["Material"]["ItemSpritePath"].ToString();


                        int amount = (int)(EquippedEquipmentInInventoryLoad[i]["MaterialsForCrafting"][r]["Amount"]);

                        item.MaterialsForCrafting.Add(new ItemAmount(Mat, amount));

                    }

                    item.ItemSpriteInventory = EquippedEquipmentInInventoryLoad[i]["ItemSpriteInventory"].ToString();
                    item.ItemSpritePathWhenEquipped = EquippedEquipmentInInventoryLoad[i]["ItemSpritePathWhenEquipped"].ToString();


                    for (int y = 0; y < SlotsForEquipment.Length; y++)
                    {
                        if (SlotsForEquipment[y].TypeOfSlot == item.SlotForEquipment)
                        {
                            SlotsForEquipment[y].Full = true;
                            SlotsForEquipment[y].TimeLeftTillNextUse = item.CoolDownTimeHours;
                            SlotsForEquipment[y].TimesLeftToUseInMatch = item.UsesInMatch;
                            SlotsForEquipment[y].TimesLeftToUseBeforeCountdown = item.UsesBeforeTimeCountdown;
                            SlotsForEquipment[y].TimesLeftToUseBeforeDestruction = item.UsesBeforeDestruction;
                            SlotsForEquipment[y].TheItem = item;

                            SlotsForEquipment[y].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.ItemSpritePathWhenEquipped);
                            SlotsForEquipment[y].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

                            GameObject go = Instantiate(WardrobeManager.Instance.EquipemntCellPrefab, WardrobeManager.Instance.EquipmentCellParent);

                            EquipmentCell Eqcell = go.GetComponent<EquipmentCell>();
                            Eqcell.Full = true;

                            Eqcell.ItemInCell = item;

                            //Eqcell.ItemName.text = GameManager.Instance.ThePlayer.EquipmentInInventory[i].name;

                            Eqcell.TheTypeOfItem = item.TheTypeOfEquipment;

                            Eqcell.ItemSprite.sprite = Resources.Load<Sprite>(item.ItemSpriteInventory);

                            Eqcell.IsBeingHeld = false;
                            Eqcell.EquippedOnPlayer = true;

                            Eqcell.TimesLeftToUseBeforeDestruction = item.UsesBeforeDestruction;

                            foreach (Transform child in go.transform)
                            {
                                if (child.name == "Equipped")
                                {
                                    Eqcell.Equipped = child.gameObject;
                                }
                            }

                            if (Eqcell.ItemInCell.ID == SlotsForEquipment[y].TheItem.ID)
                            {
                                Eqcell.Equipped.SetActive(true);
                                Eqcell.GetComponent<Image>().color = new Color(1, 1, 1, 0.6f);
                            }

                            SlotsForEquipment[y].OriginalCellFromInventory = Eqcell;

                            EquippedItems.Add(SlotsForEquipment[y].TheItem);
                            WardrobeManager.Instance.EquippedItems.Add(SlotsForEquipment[y]);
                        }
                    }
                }
            }

        }

    }

}
