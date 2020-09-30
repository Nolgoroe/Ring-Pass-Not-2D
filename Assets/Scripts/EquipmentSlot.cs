using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class EquipmentSlot : MonoBehaviour,IDropHandler
{
    //public bool Usable;

    public bool Full;

    public EquipmentSlotType TypeOfSlot;

    public float TimeLeftTillNextUse;

    public int TimesLeftToUseInMatch;

    public int TimesLeftToUseBeforeCountdown;

    public Equipment TheItem;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);


        if (Full == false && TypeOfSlot == WardrobeDragHandler.CellToMove.ItemInCell.SlotForEquipment)
        {
            WardrobeDragHandler.CellToMove.transform.SetParent(gameObject.transform);
            WardrobeDragHandler.CellToMove.transform.position = gameObject.transform.position;


            WardrobeDragHandler.CellToMove.OriginalParent = WardrobeDragHandler.CellToMove.transform.parent;


            Full = true;
            TheItem = WardrobeDragHandler.CellToMove.ItemInCell;
            //Usable = true;

            if (TheItem.HasTimeCooldown)
            {
                TimesLeftToUseBeforeCountdown = TheItem.UsesBeforeTimeCountdown;
            }
            else
            {
                TimesLeftToUseInMatch = TheItem.UsesInMatch;
            }

            GameManager.Instance.ThePlayer.PowerUpsFromItems.Add(TheItem.PowerUpToGive);

            WardrobeManager.Instance.AllEquipments.Remove(TheItem);

            GameManager.Instance.ThePlayer.EquipmentInInventory.Remove(TheItem);
        }
        else
        {
            WardrobeDragHandler.CellToMove.transform.SetParent(WardrobeDragHandler.CellToMove.OriginalParent);
            WardrobeDragHandler.CellToMove.transform.position = WardrobeDragHandler.CellToMove.transform.parent.position;
        }
    }

    //private void Start()
    //{
    //    if(TheItem != null)
    //    {
    //        Full = true;
    //        Usable = true;

    //        if (TheItem.HasTimeCooldown)
    //        {
    //            //TimeLeftTillNextUse = TheItem.CoolDownTimeHours;
    //            TimesLeftToUseBeforeCountdown = TheItem.UsesBeforeTimeCountdown;
    //        }
    //        else
    //        {
    //            TimesLeftToUseInMatch = TheItem.UsesInMatch;
    //        }
    //    }
    //}


}
