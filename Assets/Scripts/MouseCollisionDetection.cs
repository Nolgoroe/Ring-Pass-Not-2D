using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCollisionDetection : MonoBehaviour
{
    public PieceMoveManager PieceToMove;
    public List<Collider2D> InteractedColliders;
    public bool EnteredBoard;
    bool DoOnce;
    bool SkipLevel;
    Vector3 Mousepos;

    Touch touch;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(PieceToMove == null)
        {
            if (collision.CompareTag("Movable Daddy"))
            {
                if (Input.touchCount > 0)
                {
                    //Debug.Log("What?");
                    PieceToMove = collision.gameObject.GetComponent<PieceMoveManager>();
                    if (PieceToMove.Locked)
                    {
                        PieceToMove = null;
                        Debug.Log("Piece Locked");
                    }
                }
            }
        }

        if (!collision.CompareTag("Movable Daddy") && !collision.CompareTag("Board"))
        {
            InteractedColliders.Add(collision);
        }

        if (collision.CompareTag("Board"))
        {
            EnteredBoard = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Board"))
        {
            EnteredBoard = false;
        }
    }

    //private void OnMouseDown()
    //{
    //    if (Input.GetMouseButtonDown(0) && PieceToMove!= null)
    //    {
    //        //Debug.Log("Can Pick Up");

    //        PieceToMove.IsBeingHeld = true;

    //        if (InteractedColliders.Count > 0)
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
    //        if (PieceToMove != null)
    //        {
    //            if (InteractedColliders[0].GetComponent<CellInfo>().Full == false)
    //            {
    //                InteractedColliders[0].GetComponent<CellInfo>().Full = true;

    //                if (PieceToMove.OriginalParent.CompareTag("MiniClipDaddy"))
    //                {
    //                    GameManager.Instance.FillClipPiece(PieceToMove.OriginalParent.gameObject);
    //                }

    //                PieceToMove.transform.SetParent(InteractedColliders[0].transform);
    //                PieceToMove.transform.position = InteractedColliders[0].transform.position;
    //                PieceToMove.transform.rotation = InteractedColliders[0].transform.rotation;

    //                for (int i = 0; i < InteractedColliders.Count; i++)
    //                {
    //                    if (InteractedColliders[i] != InteractedColliders[0])
    //                    {
    //                        InteractedColliders.Remove(InteractedColliders[i]);
    //                    }
    //                }

    //                PieceToMove.IsBeingHeld = false;
    //                PieceToMove.OriginalParent = PieceToMove.transform.parent;
    //                PieceToMove.startingpos = PieceToMove.transform.position;
    //                PieceToMove.startingRotation = PieceToMove.transform.rotation;

    //                InteractedColliders[0].GetComponent<CellInfo>().Rconnect.Lsymbol = PieceToMove.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;
    //                InteractedColliders[0].GetComponent<CellInfo>().Rconnect.Lcolor = PieceToMove.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;

    //                InteractedColliders[0].GetComponent<CellInfo>().Lconnect.Rsymbol = PieceToMove.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;
    //                InteractedColliders[0].GetComponent<CellInfo>().Lconnect.Rcolor = PieceToMove.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;

    //                InteractedColliders[0].GetComponent<CellInfo>().Lconnect.CheckConnection();
    //                InteractedColliders[0].GetComponent<CellInfo>().Rconnect.CheckConnection();
    //            }
    //            else
    //            {
    //                Debug.Log("Found Nothing 1");
    //                //ThisRenderer.sortingOrder = 0;
    //                PieceToMove.transform.position = PieceToMove.startingpos;
    //                PieceToMove.transform.parent = PieceToMove.OriginalParent;
    //                PieceToMove.transform.rotation = PieceToMove.startingRotation;
    //                PieceToMove.IsBeingHeld = false;
    //            }
    //        }

    //    }
    //    else
    //    {
    //        if(PieceToMove != null)
    //        {
    //            Debug.Log("Found Nothing 1");
    //            //ThisRenderer.sortingOrder = 0;
    //            PieceToMove.transform.position = PieceToMove.startingpos;
    //            PieceToMove.transform.parent = PieceToMove.OriginalParent;
    //            PieceToMove.transform.rotation = PieceToMove.startingRotation;
    //            PieceToMove.IsBeingHeld = false;
    //        }
    //    }
    //}

    //private void Update()
    //{
    //    Mousepos = Input.mousePosition;
    //    Mousepos = Camera.main.ScreenToWorldPoint(Mousepos);
    //    transform.position = new Vector3(Mousepos.x, Mousepos.y, 0);

    //}

    private void Update()
    {
        if(Input.touchCount >= 3 && !SkipLevel)
        {
            SkipLevel = true;
            GameManager.Instance.StartNextLevel();
            return;
        }

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                DoOnce = true;
                Mousepos = Camera.main.ScreenToWorldPoint(new Vector2(touch.position.x, touch.position.y));
                transform.position = Mousepos;
                transform.GetComponent<Collider2D>().enabled = true;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                transform.GetComponent<Collider2D>().enabled = true;
                Mousepos = Camera.main.ScreenToWorldPoint(new Vector2(touch.position.x, touch.position.y));
                transform.position = Mousepos;

                if (PieceToMove != null)
                {
                    //Debug.Log("Can Pick Up");

                    PieceToMove.IsBeingHeld = true;

                    if (PieceToMove.PartOfBoard)
                    {
                        if (DoOnce)
                        {
                            GameManager.Instance.FullCellCounter--;

                            if (PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.SuccesfullConnectionMade == true)
                            {
                                GameManager.Instance.SuccesfullConnectionsMade--;
                            }

                            if (PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.SuccesfullConnectionMade == true)
                            {
                                GameManager.Instance.SuccesfullConnectionsMade--;
                            }
                            DoOnce = false;
                        }

                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lsymbol = Symbols.None;
                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lcolor = Colors.None;

                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rsymbol = Symbols.None;
                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rcolor = Colors.None;

                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.BadConnectionMade = false;
                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.BadConnectionMade = false;

                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.SuccesfullConnectionMade = false;
                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.SuccesfullConnectionMade = false;

                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Full = false;

                        //InteractedColliders.Clear();
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                SkipLevel = false;
                //Debug.Log("Touch Up");
                if (EnteredBoard)
                {
                    //Debug.Log("In Board");
                    if (PieceToMove != null)
                    {
                        //Debug.Log("Has Piece");
                        if (InteractedColliders.Count != 0)
                        {
                            //Debug.Log("InteractedColliders Not Empty");
                            if (InteractedColliders[InteractedColliders.Count -1].GetComponent<CellInfo>().Full == false)
                            {
                                if (PieceToMove.OriginalParent.CompareTag("MiniClipDaddy"))
                                {
                                    GameManager.Instance.FillClipPiece(PieceToMove.OriginalParent.gameObject);
                                }

                                PieceToMove.transform.SetParent(InteractedColliders[InteractedColliders.Count - 1].transform);
                                PieceToMove.transform.position = InteractedColliders[InteractedColliders.Count - 1].transform.position;
                                PieceToMove.transform.rotation = InteractedColliders[InteractedColliders.Count - 1].transform.rotation;

                                PieceToMove.PartOfBoard = true;
                                PieceToMove.IsBeingHeld = false;

                                PieceToMove.OriginalParent = PieceToMove.transform.parent;
                                PieceToMove.startingpos = PieceToMove.transform.position;
                                PieceToMove.startingRotation = PieceToMove.transform.rotation;

                                InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Rconnect.Lsymbol = PieceToMove.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;
                                InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Rconnect.Lcolor = PieceToMove.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;

                                InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Rconnect.LeftPieceParent = PieceToMove;

                                InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Lconnect.Rsymbol = PieceToMove.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;
                                InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Lconnect.Rcolor = PieceToMove.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;

                                InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Lconnect.RightPieceParent = PieceToMove;

                                InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Lconnect.CheckConnection();
                                InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Rconnect.CheckConnection();

                                InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Full = true;
                                GameManager.Instance.FullCellCounter++;
                                PieceToMove = null;
                            }
                            else
                            {
                                PieceToMove.transform.position = PieceToMove.startingpos;
                                PieceToMove.transform.parent = PieceToMove.OriginalParent;
                                PieceToMove.transform.rotation = PieceToMove.startingRotation;
                                PieceToMove.IsBeingHeld = false;
                                PieceToMove = null;
                                return;
                            }
                        }
                        else
                        {
                            if (PieceToMove.PartOfBoard)
                            {
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Full = true;
                                GameManager.Instance.FullCellCounter++;
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.CheckConnection();
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.CheckConnection();
                            }
                            Debug.Log("Found Nothing 2");

                            PieceToMove.transform.position = PieceToMove.startingpos;
                            PieceToMove.transform.parent = PieceToMove.OriginalParent;
                            PieceToMove.transform.rotation = PieceToMove.startingRotation;
                            PieceToMove.IsBeingHeld = false;
                            PieceToMove = null;

                        }
                    }
                    //else
                    //{
                    //    if (PieceToMove.PartOfBoard)
                    //    {
                    //        PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lsymbol = PieceToMove.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;
                    //        PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lcolor = PieceToMove.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;

                    //        PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rsymbol = PieceToMove.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;
                    //        PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rcolor = PieceToMove.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;

                    //        PieceToMove.transform.position = PieceToMove.startingpos;
                    //        PieceToMove.transform.parent = PieceToMove.OriginalParent;
                    //        PieceToMove.transform.rotation = PieceToMove.startingRotation;
                    //        PieceToMove.IsBeingHeld = false;
                    //        PieceToMove.OriginalParent.GetComponent<CellInfo>().Full = true;
                    //        GameManager.Instance.FullCellCounter++;

                    //        PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.CheckConnection();
                    //        PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.CheckConnection();

                    //        PieceToMove = null;
                    //    }
                    //}
                }
                else
                {
                    if (PieceToMove != null)
                    {
                        if (PieceToMove.PartOfBoard)
                        {
                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lsymbol = PieceToMove.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;
                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lcolor = PieceToMove.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;

                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rsymbol = PieceToMove.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;
                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rcolor = PieceToMove.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;

                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Full = true;
                            GameManager.Instance.FullCellCounter++;

                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.CheckConnection();
                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.CheckConnection();
                        }
                        //Debug.Log("Found Nothing 1");
                        //ThisRenderer.sortingOrder = 0;
                        PieceToMove.transform.position = PieceToMove.startingpos;
                        PieceToMove.transform.parent = PieceToMove.OriginalParent;
                        PieceToMove.transform.rotation = PieceToMove.startingRotation;
                        PieceToMove.IsBeingHeld = false;
                        PieceToMove = null;
                    }


                }

                DoOnce = false;
                transform.GetComponent<Collider2D>().enabled = false;
                InteractedColliders.Clear();

                if (GameManager.Instance.FullCellCounter == 8)
                {
                    UiManager.Instance.Commit.interactable = true;
                }
            }
        }
    }
}

