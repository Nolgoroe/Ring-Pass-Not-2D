using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public List<int> PositionsInPlayerPrefs;

    public static EquipmentManager Instance;
    private void Start()
    {
        Instance = this;

        if (PlayerPrefs.HasKey("CountOfPositionsInPlayerPrefs"))
        {
            int count = PlayerPrefs.GetInt("CountOfPositionsInPlayerPrefs");

            for (int i = 0; i < count; i++)
            {
                PositionsInPlayerPrefs.Add(PlayerPrefs.GetInt("PositionNumber" + i));
            }
        }
    }
    private void Update()
    {
        if (PlayerPrefs.HasKey("ItemsWithCooldownCount"))
        {
            //Debug.Log("Checking");
            CheckCooldownsDone();
        }
    }

    public void DecreaseNumberOfUsesInMatch(Button PressedPowerUp, EquipmentSlot SlotToWorkOn)
    {
        SlotToWorkOn.TimesLeftToUseInMatch--;

        if (SlotToWorkOn.TimesLeftToUseInMatch == 0)
        {
            SlotToWorkOn.TimesLeftToUseInMatch = SlotToWorkOn.TheItem.UsesInMatch;
            PressedPowerUp.interactable = false;
            SlotToWorkOn.Usable = false;
        }
    }

    public void TimerTillNextPowerUpUse(Button PressedPowerUp, EquipmentSlot SlotToWorkOn)
    {
        SlotToWorkOn.TimesLeftToUseBeforeCountdown--;
        SlotToWorkOn.TimeLeftTillNextUse = SlotToWorkOn.TheItem.CoolDownTimeHours;
        if (SlotToWorkOn.TimesLeftToUseBeforeCountdown == 0)
        {
            PressedPowerUp.interactable = false;
            PressedPowerUp.transform.GetChild(0).GetComponent<Text>().text = "Cooldown";
            SlotToWorkOn.Usable = false;

            if (!GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.Contains(SlotToWorkOn.TheItem))
            {
                GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.Add(SlotToWorkOn.TheItem);

                PositionsInPlayerPrefs.Add(SlotToWorkOn.TheItem.ID);

                PlayerPrefs.SetInt("CountOfPositionsInPlayerPrefs", PositionsInPlayerPrefs.Count);

                for (int i = 0; i < PositionsInPlayerPrefs.Count; i++)
                {
                    PlayerPrefs.SetInt("PositionNumber" + i, PositionsInPlayerPrefs[i]);
                }

                PlayerPrefs.SetInt("ItemsWithCooldownCount", GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.Count);

                for (int i = 0; i < GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.Count; i++)
                {
                    if (SlotToWorkOn.TheItem.ID == GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown[i].ID)
                    {
                        PlayerPrefs.SetInt("ItemID" + SlotToWorkOn.TheItem.ID, SlotToWorkOn.TheItem.ID);

                        //PlayerPrefs.SetString("DateTimePowerUpUsed" + SlotToWorkOn.TheItem.ID, DateTime.Now.ToString());

                        PlayerPrefs.SetString("NextTimePowerUpAvailable" + SlotToWorkOn.TheItem.ID, DateTime.Now.AddMinutes(SlotToWorkOn.TheItem.CoolDownTimeHours).ToString());
                    }
                }
            }
        }
    }

    public void CheckCooldownsDone()
    {
        if (PlayerPrefs.GetInt("ItemsWithCooldownCount") > 0)
        {
            int count = PlayerPrefs.GetInt("ItemsWithCooldownCount");

            for (int i = 0; i < count; i++)
            {
                DateTime CurrentTime = DateTime.Now.ToLocalTime();

                Debug.Log(PlayerPrefs.GetString("NextTimePowerUpAvailable" + PositionsInPlayerPrefs[i]));

                DateTime NextTimeAvailable = DateTime.Parse(PlayerPrefs.GetString("NextTimePowerUpAvailable" + PositionsInPlayerPrefs[i]));

                TimeSpan TotalTime = NextTimeAvailable - CurrentTime;

                if (TotalTime <= TimeSpan.Zero)
                {
                    for (int k = 0; k < GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.Count; k++)
                    {
                        if (PlayerPrefs.GetInt("ItemID" + PositionsInPlayerPrefs[i]) == GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown[k].ID)
                        {
                            //Debug.Log(GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown[k].ID);

                            for (int p = 0; p < GameManager.Instance.ThePlayer.SlotsForEquipment.Length; p++) 
                            {
                                /////// Will create problems in the future because item might not be equipped on player. Might have to go through Inventory aswell
                                
                                if (GameManager.Instance.ThePlayer.SlotsForEquipment[p].TheItem == GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown[k])
                                {
                                    GameManager.Instance.ThePlayer.SlotsForEquipment[p].TimesLeftToUseBeforeCountdown = GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown[k].UsesBeforeTimeCountdown;
                                }
                            } 

                            PlayerPrefs.DeleteKey("ItemID" + PositionsInPlayerPrefs[i]);

                            PlayerPrefs.DeleteKey("NextTimePowerUpAvailable" + PositionsInPlayerPrefs[i]);

                            PlayerPrefs.DeleteKey("PositionNumber" + PositionsInPlayerPrefs[i]);

                            PositionsInPlayerPrefs.Remove(PositionsInPlayerPrefs[i]);

                            if(PositionsInPlayerPrefs.Count == 0)
                            {
                                PlayerPrefs.DeleteKey("CountOfPositionsInPlayerPrefs");
                            }

                            GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.RemoveAt(k);

                            PlayerPrefs.SetInt("ItemsWithCooldownCount", GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.Count);

                            return;
                        }
                        Debug.Log("Cooldown Done " + PlayerPrefs.GetInt("ItemID" + PositionsInPlayerPrefs[i]));
                    }
                }
            }
        }

        if (GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.Count == 0)
        {
            PlayerPrefs.DeleteKey("ItemID");
            PlayerPrefs.DeleteKey("ItemsWithCooldownCount");
            PlayerPrefs.DeleteKey("NextTimePowerUpAvailable");
        }
    }
}
