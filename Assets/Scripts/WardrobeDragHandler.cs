using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WardrobeDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static EquipmentCell CellToMove;
    Vector3 Mousepos;
    Touch touch;
    //public List<Collider2D> InteractedColliders;

    Transform MainCavas;
    private void Start()
    {
        MainCavas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.Wardrobe.activeInHierarchy)
        {
            //if (CellToMove == null)
            //{
            //if (eventData.pointerCurrentRaycast.gameObject.CompareTag("EquipmentCell"))
            //{
            //    CellToMove = eventData.pointerCurrentRaycast.gameObject.GetComponent<EquipmentCell>();

            CellToMove = gameObject.GetComponent<EquipmentCell>();

            Mousepos = Input.mousePosition;
            transform.position = Mousepos;
            //Debug.Log(Input.mousePosition);

            CellToMove.transform.SetParent(MainCavas);

            gameObject.GetComponent<Image>().raycastTarget = false;
            //}
            //}
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.GetComponent<Collider2D>().enabled = true;
        Mousepos = Input.mousePosition;
        transform.position = Mousepos;
        //Debug.Log(Input.mousePosition);

        if (CellToMove != null)
        {
            CellToMove.IsBeingHeld = true;

            //CellToMove.Full = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        if(transform.parent != CellToMove.OriginalParent)
        {
            CellToMove.transform.SetParent(CellToMove.OriginalParent);
            CellToMove.transform.position = CellToMove.OriginalParent.transform.position;
        }


        gameObject.GetComponent<Image>().raycastTarget = true;

        CellToMove.IsBeingHeld = false;

        CellToMove = null;

    }
}
