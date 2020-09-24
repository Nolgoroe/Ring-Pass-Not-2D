using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


}
