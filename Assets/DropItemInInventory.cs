using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DropItemInInventory : MonoBehaviour, IDropHandler
{
    public GameObject ParentForAllInventoryItems;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);

        if (WardrobeDragHandler.CellToMove)
        {
            if (WardrobeDragHandler.CellToMove.OriginalParent.GetComponent<EquipmentSlot>())
            {
                EquipmentSlot TheItem = WardrobeDragHandler.CellToMove.OriginalParent.GetComponent<EquipmentSlot>();

                WardrobeDragHandler.CellToMove.transform.SetParent(ParentForAllInventoryItems.transform);
                WardrobeDragHandler.CellToMove.OriginalParent = WardrobeDragHandler.CellToMove.transform.parent;

                WardrobeManager.Instance.AllEquipments.Add(TheItem.TheItem);

                GameManager.Instance.ThePlayer.EquipmentInInventory.Add(TheItem.TheItem);
                GameManager.Instance.ThePlayer.PowerUpsFromItems.Remove(TheItem.TheItem.PowerUpToGive);
                TheItem.Full = false;
                //TheItem.Usable = false;
                TheItem.TimesLeftToUseBeforeCountdown = 0;
                TheItem.TimeLeftTillNextUse = 0;
                TheItem.TimesLeftToUseInMatch = 0;
                TheItem = null;


                //WardrobeDragHandler.CellToMove.transform.position = gameObject.transform.position;
            }
            else
            {
                WardrobeDragHandler.CellToMove.transform.SetParent(WardrobeDragHandler.CellToMove.OriginalParent);
                WardrobeDragHandler.CellToMove.transform.position = WardrobeDragHandler.CellToMove.transform.parent.position;
            }
        }
    }
}
