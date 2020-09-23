using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EquipmentSlot : MonoBehaviour
{
    public bool Usable;

    public bool Full;

    public EquipmentSlotType TypeOfSlot;

    public float TimeLeftTillNextUse;

    public int TimesLeftToUseInMatch;

    public int TimesLeftToUseBeforeCountdown;

    public Equipment TheItem;


    private void Start()
    {
        if(TheItem != null)
        {
            Full = true;
            Usable = true;

            if (TheItem.HasTimeCooldown)
            {
                //TimeLeftTillNextUse = TheItem.CoolDownTimeHours;
                TimesLeftToUseBeforeCountdown = TheItem.UsesBeforeTimeCountdown;
            }
            else
            {
                TimesLeftToUseInMatch = TheItem.UsesInMatch;
            }
        }
    }

    private void Update()
    {
        string dtH = DateTime.Now.ToLocalTime().ToString("HH");
        string dtM = DateTime.Now.ToLocalTime().ToString("mm");
        string dtS = DateTime.Now.ToLocalTime().ToString("ss");
        //Debug.Log(dtH);
        //Debug.Log(dtM);
        //Debug.Log(Convert.ToInt64(dtH) * 60 + Convert.ToInt64(dtM));
        //Debug.Log(DateTime.Now.ToShortDateString());
        //TimeSpan now = new TimeSpan(Convert.ToInt16(dtH), Convert.ToInt16(dtM), Convert.ToInt16(dtS));
        //Debug.Log(now);

        //TimeSpan SavedTime = new TimeSpan(8, 34, 15);
        DateTime current = DateTime.Now;

        DateTime Next = new DateTime(2020, 9, 24, 8, 34, 15);
        //Debug.Log(SavedTime);

        TimeSpan TotalTime = Next - current;

        Debug.Log(TotalTime);
    }
    public void DecreaseNumberOfUsesInMatch(Button PressedPowerUp)
    {
        TimesLeftToUseInMatch--;

        if(TimesLeftToUseInMatch == 0)
        {
            PressedPowerUp.interactable = false;
            Usable = false;
        }
    }

    public void TimerTillNextPowerUpUse(Button PressedPowerUp)
    {
        TimesLeftToUseBeforeCountdown--;

        if(TimesLeftToUseBeforeCountdown == 0)
        {
            PressedPowerUp.interactable = false;
            Usable = false;
            GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.Add(this);

            PlayerPrefs.SetInt("ItemsWithCooldownCount", GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.Count);

            for (int i = 0; i < GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown.Count; i++)
            {
                PlayerPrefs.SetInt("ItemID" + i, GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown[i].TheItem.ID);

                PlayerPrefs.SetString("DatePowerUpUsed" + i, DateTime.Now.ToShortDateString());
            }
        }
    }
}
