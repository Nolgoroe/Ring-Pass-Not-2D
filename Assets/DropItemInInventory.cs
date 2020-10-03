using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DropItemInInventory : MonoBehaviour/*, IDropHandler*/, IPointerDownHandler
{
    //public GameObject ParentForAllInventoryItems;

    //public void OnDrop(PointerEventData eventData)
    //{
    //    Debug.Log(gameObject.name);

    //    if (WardrobeDragHandler.CellToMove)
    //    {
    //        if (WardrobeDragHandler.CellToMove.OriginalParent.GetComponent<EquipmentSlot>())
    //        {
    //            EquipmentSlot TheItem = WardrobeDragHandler.CellToMove.OriginalParent.GetComponent<EquipmentSlot>();

    //            WardrobeDragHandler.CellToMove.transform.SetParent(ParentForAllInventoryItems.transform);
    //            WardrobeDragHandler.CellToMove.OriginalParent = WardrobeDragHandler.CellToMove.transform.parent;

    //            WardrobeManager.Instance.AllEquipments.Add(TheItem.TheItem);

    //            GameManager.Instance.ThePlayer.EquipmentInInventory.Add(TheItem.TheItem);

    //            for (int i = 0; i < GameManager.Instance.ThePlayer.PowerUpsFromItems.Count; i++)
    //            {
    //                for (int k = 0; k < TheItem.TheItem.PowerUpToGive.Length; k++)
    //                {
    //                    if (GameManager.Instance.ThePlayer.PowerUpsFromItems[i] == TheItem.TheItem.PowerUpToGive[k])
    //                    {
    //                        GameManager.Instance.ThePlayer.PowerUpsFromItems.Remove(TheItem.TheItem.PowerUpToGive[k]);
    //                    }

    //                }
    //            }
    //            TheItem.Full = false;
    //            //TheItem.Usable = false;
    //            TheItem.TimesLeftToUseBeforeCountdown = 0;
    //            TheItem.TimeLeftTillNextUse = 0;
    //            TheItem.TimesLeftToUseInMatch = 0;
    //            TheItem = null;


    //            //WardrobeDragHandler.CellToMove.transform.position = gameObject.transform.position;
    //        }
    //        else
    //        {
    //            WardrobeDragHandler.CellToMove.transform.SetParent(WardrobeDragHandler.CellToMove.OriginalParent);
    //            WardrobeDragHandler.CellToMove.transform.position = WardrobeDragHandler.CellToMove.transform.parent.position;
    //        }
    //    }
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<EquipmentSlot>())
        {
            EquipmentSlot EQS = eventData.pointerCurrentRaycast.gameObject.GetComponent<EquipmentSlot>();

            if (EQS.Full)
            {
                for (int i = 0; i < GameManager.Instance.ThePlayer.PowerUpsFromItems.Count; i++)
                {
                    for (int k = 0; k < EQS.TheItem.PowerUpToGive.Length; k++)
                    {
                        if (GameManager.Instance.ThePlayer.PowerUpsFromItems[i] == EQS.TheItem.PowerUpToGive[k])
                        {
                            GameManager.Instance.ThePlayer.PowerUpsFromItems.Remove(EQS.TheItem.PowerUpToGive[k]);
                        }

                    }
                }

                EQS.DestructionButtons.Clear();
                EQS.ItemSprite = EQS.GetComponent<Image>();
                EQS.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                EQS.Full = false;
                EQS.TimesLeftToUseBeforeCountdown = 0;
                EQS.TimeLeftTillNextUse = 0;
                EQS.TimesLeftToUseInMatch = 0;
                EQS.TimesLeftToUseBeforeDestruction = 0;
                EQS.OriginalCellFromInventory.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                EQS.OriginalCellFromInventory.EquippedOnPlayer = false;
                EQS.OriginalCellFromInventory.Equipped.SetActive(false);

                WardrobeManager.Instance.EquippedItems.Remove(EQS);
                WardrobeManager.Instance.AllEquipments.Add(EQS.TheItem);
                GameManager.Instance.ThePlayer.EquipmentInInventory.Add(EQS.TheItem);
                GameManager.Instance.ThePlayer.EquippedItems.Remove(EQS);

                EQS.OriginalCellFromInventory = null;
                EQS.TheItem = null;
            }
        }
    }
}
