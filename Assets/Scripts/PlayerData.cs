using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int Gold, Rubies, MagicalItems, MaxLevelReached;

    public EquipmentSlot[] SlotsForEquipment;

    public List<PowerUpChooseItemTypes> PowerUpsFromItems;

    public List<Equipment> EquipmentWithTimeCooldown;

    public List<Equipment> EquipmentInInventory;

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
                    if(PlayerPrefs.GetInt("ItemID" + EquipmentManager.Instance.PositionsInPlayerPrefs[i]) == GameManager.Instance.GameItems[k].ID)
                    {
                        EquipmentWithTimeCooldown.Add(GameManager.Instance.GameItems[k]);
                    }
                }
            }
        }
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
}
