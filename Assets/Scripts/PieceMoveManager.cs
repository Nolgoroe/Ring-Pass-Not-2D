﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceMoveManager : MonoBehaviour
{
    Vector3 Mousepos;

    public bool IsBeingHeld;
    public bool PartOfBoard;

    //public bool EnteredBoard;
    [HideInInspector]
    public Vector3 startingpos;

    [HideInInspector]
    public Quaternion startingRotation;

    [HideInInspector]
    public Transform OriginalParent;

    //public Transform Parent;

    SpriteRenderer ThisRenderer;

    public GameObject LeftSidePiece;
    public GameObject RightSidePiece;

    //public List<Collider2D> InteractedColliders;
    private void Start()
    {
        ThisRenderer = GetComponent<SpriteRenderer>();
        OriginalParent = transform.parent;
        startingpos = transform.position;
        startingRotation = transform.rotation;
    }
    //private void OnMouseDown()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Mousepos = Input.mousePosition;
    //        Mousepos = Camera.main.ScreenToWorldPoint(Mousepos);

    //        IsBeingHeld = true;

    //        if(InteractedColliders.Count > 0)
    //        {
    //            InteractedColliders[0].GetComponent<CellInfo>().Full = false;
    //            InteractedColliders.Clear();
    //        }
    //    }

    //}

    //private void OnMouseUp()
    //{
    //    if (EnteredBoard)
    //    {
    //        if (InteractedColliders[0].GetComponent<CellInfo>().Full == false)
    //        {
    //            InteractedColliders[0].GetComponent<CellInfo>().Full = true;
    //            //transform.SetParent(InteractedColliders[0].transform);
    //            transform.position = InteractedColliders[0].transform.position;
    //            transform.rotation = InteractedColliders[0].transform.rotation;
    //            for (int i = 0; i < InteractedColliders.Count; i++)
    //            {
    //                if(InteractedColliders[i] != InteractedColliders[0])
    //                {
    //                    InteractedColliders.Remove(InteractedColliders[i]);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("Found Nothing 2");
    //            transform.position = startingpos;
    //            transform.parent = OriginalParent;
    //            transform.rotation = startingRotation;
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Found Nothing 1");
    //        //ThisRenderer.sortingOrder = 0;
    //        transform.position = startingpos;
    //        transform.parent = OriginalParent;
    //        transform.rotation = startingRotation;
    //    }
    //    IsBeingHeld = false;
    //}

    public void Update()
    {
        if (IsBeingHeld)
        {
            transform.parent = null;
            //ThisRenderer.sortingOrder = 10;
            Mousepos = Input.mousePosition;
            Mousepos = Camera.main.ScreenToWorldPoint(Mousepos);
            transform.position = new Vector3(Mousepos.x, Mousepos.y, -3);

            float angle = Mathf.Atan2(GameManager.Instance.PieceTargetLookAt.position.y - transform.position.y, GameManager.Instance.PieceTargetLookAt.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            //Debug.Log(angle);
        }
    }
}