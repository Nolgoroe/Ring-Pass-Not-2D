using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EquipmentSlot : MonoBehaviour,IDropHandler
{
    //public bool Usable;

    public bool Full;

    public EquipmentSlotType TypeOfSlot;

    public double TimeLeftTillNextUse;

    public int TimesLeftToUseInMatch;

    public int TimesLeftToUseBeforeCountdown;

    public int TimesLeftToUseBeforeDestruction;

    public Equipment TheItem;

    //public List<Button> DestructionButtons;

    //public Image ItemSprite;

    public EquipmentCell OriginalCellFromInventory;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);

        if (WardrobeDragHandler.CellToMove)
        {
            if (Full == false && TypeOfSlot == WardrobeDragHandler.CellToMove.ItemInCell.SlotForEquipment)
            {
                //WardrobeDragHandler.CellToMove.transform.SetParent(gameObject.transform);
                //WardrobeDragHandler.CellToMove.transform.position = gameObject.transform.position;


                //WardrobeDragHandler.CellToMove.OriginalParent = WardrobeDragHandler.CellToMove.transform.parent;


                Full = true;
                TheItem = WardrobeDragHandler.CellToMove.ItemInCell;

                OriginalCellFromInventory = WardrobeDragHandler.CellToMove;

                //ItemSprite.sprite = Resources.Load <Sprite>(TheItem.ItemSpritePathWhenEquipped);
                //ItemSprite.color = new Color(1, 1, 1, 1);

                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(TheItem.ItemSpritePathWhenEquipped);
                gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

                WardrobeDragHandler.CellToMove.GetComponent<Image>().color = new Color(1, 1, 1, 0.6f);
                WardrobeDragHandler.CellToMove.EquippedOnPlayer = true;
                WardrobeDragHandler.CellToMove.Equipped.SetActive(true);

                WardrobeDragHandler.CellToMove.transform.SetParent(WardrobeDragHandler.CellToMove.OriginalParent);
                WardrobeDragHandler.CellToMove.transform.position = WardrobeDragHandler.CellToMove.transform.parent.position;

                //Usable = true;

                if (TheItem.HasTimeCooldown)
                {
                    TimesLeftToUseBeforeCountdown = TheItem.UsesBeforeTimeCountdown;
                }
                else
                {
                    TimesLeftToUseInMatch = TheItem.UsesInMatch;
                }

                TimesLeftToUseBeforeDestruction = TheItem.UsesBeforeDestruction;

                GameManager.Instance.ThePlayer.PowerUpsFromItems.AddRange(TheItem.PowerUpToGive);
                GameManager.Instance.ThePlayer.EquippedItems.Add(TheItem);
                WardrobeManager.Instance.EquippedItems.Add(this);

                WardrobeManager.Instance.AllEquipments.Remove(TheItem); /// ADD to on drop inventory

                GameManager.Instance.ThePlayer.EquipmentInInventory.Remove(TheItem); /// ADD to on drop inventory
            }
            else
            {
                WardrobeDragHandler.CellToMove.transform.SetParent(WardrobeDragHandler.CellToMove.OriginalParent);
                WardrobeDragHandler.CellToMove.transform.position = WardrobeDragHandler.CellToMove.transform.parent.position;
            }
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
