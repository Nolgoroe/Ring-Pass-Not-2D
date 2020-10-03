using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentCell : MonoBehaviour
{
    public bool Full;

    public Equipment ItemInCell;

    //public Text ItemName, ItemUsesPerDay;

    public TypeOfequipment TheTypeOfItem;

    public Image ItemSprite;

    public bool IsBeingHeld, EquippedOnPlayer;

    public int TimesLeftToUseBeforeDestruction;

    [HideInInspector]
    public Transform OriginalParent;

    Vector3 Mousepos;

    public GameObject Equipped;

    private void Start()
    {
        OriginalParent = transform.parent;

        TimesLeftToUseBeforeDestruction = ItemInCell.UsesBeforeDestruction;
    }
    private void Update()
    {
        if (IsBeingHeld && Full)
        {
            //transform.SetParent(null);
            Mousepos = Input.mousePosition;
            //Mousepos = Camera.main.ScreenToWorldPoint(Mousepos);
            transform.position = new Vector3(Mousepos.x, Mousepos.y, -3);

        }
    }
}
