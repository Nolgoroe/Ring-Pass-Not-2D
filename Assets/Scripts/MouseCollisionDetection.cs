using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCollisionDetection : MonoBehaviour
{
    public static MouseCollisionDetection Instance;

    public PieceMoveManager PieceToMove;

    public GameObject ObjectToUsePowerup;

    public List<Collider2D> InteractedColliders;
    public bool EnteredBoard;
    bool DoOnce;
    //bool SkipLevel;
    Vector3 Mousepos;

    Touch touch;

    Vector3 startTouchPos, endTouchPos;
    public float LengthTouchLimiter = 1f;
    public float TimerCouter = 0f;

    public LayerMask PowerUpsLayer;

    bool NumReduced = false;

    [HideInInspector]
    public bool BombedSlice;

    private void Start()
    {
        Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Power Ups Layer") && !GameManager.Instance.PowerUpManager.HasTargetForPowerUp)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Power Ups Layer"))
            {
                //Debug.Log(collision.gameObject.name);
                ObjectToUsePowerup = collision.gameObject;

                for (int i = 0; i < ObjectToUsePowerup.GetComponent<SelectPowerUpType>().PowerUpsToUse.Length; i++)
                {
                    if (GameManager.Instance.PowerUpManager.PowerUpInUse == ObjectToUsePowerup.GetComponent<SelectPowerUpType>().PowerUpsToUse[i])
                    {
                        //Debug.Log("WHAT THE FUCK");
                        GameManager.Instance.PowerUpManager.HasTargetForPowerUp = true;
                        GameManager.Instance.PowerUpManager.UsePowerUp(ObjectToUsePowerup);
                        return;
                    }
                }
            }
        }

        if (PieceToMove == null && !GameManager.Instance.PowerUpManager.UsingPowerUp)
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
                        //Debug.Log("Piece Locked");
                    }
                }
            }
        }

        if (!collision.CompareTag("Movable Daddy") && !collision.CompareTag("Board") && !GameManager.Instance.PowerUpManager.UsingPowerUp)
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
        //if(Input.touchCount >= 3 && !SkipLevel)
        //{
        //    SkipLevel = true;
        //    GameManager.Instance.StartNextLevel();
        //    return;
        //}
        if (GameManager.Instance.SceneBoard != null)
        {
            if (GameManager.Instance.PowerUpManager.UsingPowerUp)
            {
                transform.gameObject.layer = LayerMask.NameToLayer("Power Ups Layer");

                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        Mousepos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                        transform.position = Mousepos;
                        transform.GetComponent<Collider2D>().enabled = true;
                    }

                    if (touch.phase == TouchPhase.Moved)
                    {
                        transform.GetComponent<Collider2D>().enabled = true;
                        Mousepos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                        transform.position = Mousepos;
                    }

                    if (touch.phase == TouchPhase.Ended && (ObjectToUsePowerup != null || BombedSlice))
                    {
                        BombedSlice = false;
                        ObjectToUsePowerup = null;
                        GameManager.Instance.PowerUpManager.UsingPowerUp = false;
                        GameManager.Instance.PowerUpManager.ColorForColorTransformPowerUp = ColorData.None;
                        GameManager.Instance.PowerUpManager.SymbolForShapeTransformPowerUp = Symbols.None;
                        GameManager.Instance.PowerUpManager.PowerUpInUse = PowerUpChooseItemTypes.None;
                        GameManager.Instance.PowerUpManager.PowerUpButton = null;
                        return;
                    }

                }
                else
                {
                    if (PieceToMove != null)
                    {
                        PieceToMove.IsBeingHeld = false;
                        PieceToMove = null;
                    }
                }
            }
        }

        if (!UiManager.Instance.OptionsOpen && !GameManager.Instance.PowerUpManager.UsingPowerUp && !GameManager.Instance.Wardrobe.activeInHierarchy)
        {
            //Debug.Log("In here");
            //Debug.Log("Not using PowerUp");
            transform.gameObject.layer = LayerMask.NameToLayer("Board Collision Detector");

            if (Input.touchCount > 0 && Input.touchCount < 2)
            {
                //Debug.Log("Touch count " + Input.touchCount);
                TimerCouter += Time.deltaTime;
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    DoOnce = true;
                    NumReduced = false;
                    Mousepos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                    transform.position = Mousepos;
                    transform.GetComponent<Collider2D>().enabled = true;
                    startTouchPos = Mousepos;
                }

                if (TimerCouter >= LengthTouchLimiter)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        transform.GetComponent<Collider2D>().enabled = true;
                        Mousepos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                        transform.position = Mousepos;


                        if (PieceToMove != null)
                        {
                            //Debug.Log("Can Pick Up");

                            PieceToMove.IsBeingHeld = true;

                            if (PieceToMove.PartOfBoard)
                            {
                                if (DoOnce)
                                {
                                    DoOnce = false;

                                    // Debug.Log("Once");
                                    GameManager.Instance.FullCellCounter--;
                                    NumReduced = true;
                                    if (PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.SuccesfullConnectionMade == true)
                                    {
                                        GameManager.Instance.SuccesfullConnectionsMade--;
                                    }

                                    if (PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.SuccesfullConnectionMade == true)
                                    {
                                        GameManager.Instance.SuccesfullConnectionsMade--;
                                    }

                                    if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect != null)
                                    {
                                        if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.SuccesfullConnectionMade == true)
                                        {
                                            GameManager.Instance.SuccesfullConnectionsMade--;
                                        }
                                    }

                                    if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect != null)
                                    {
                                        if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.SuccesfullConnectionMade == true)
                                        {
                                            GameManager.Instance.SuccesfullConnectionsMade--;
                                        }
                                    }

                                }

                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lsymbol = Symbols.None;
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lcolor = ColorData.None;

                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rsymbol = Symbols.None;
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rcolor = ColorData.None;


                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.BadConnectionMade = false;
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.BadConnectionMade = false;

                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.SuccesfullConnectionMade = false;
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.SuccesfullConnectionMade = false;

                                if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect != null)
                                {
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.ROutersymbol = Symbols.None;
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.ROutercolor = ColorData.None;

                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.LOutersymbol = Symbols.None;
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.LOutercolor = ColorData.None;

                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.SuccesfullConnectionMade = false;
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.SuccesfullConnectionMade = false;

                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.BadConnectionMade = false;
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.BadConnectionMade = false;

                                }

                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Full = false;
                                //Debug.Log("NOT full");
                                //InteractedColliders.Clear();
                            }
                        }
                    }


                }

                if (touch.phase == TouchPhase.Ended)
                {
                    TimerCouter = 0;

                    endTouchPos = Mousepos;
                    //Debug.Log(Vector3.Magnitude(startTouchPos - endTouchPos));
                    if (Vector3.Magnitude(startTouchPos - endTouchPos) > 0.1f)
                    {
                        //SkipLevel = false;
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
                                    if (InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Full == false)
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

                                        if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
                                        {
                                            if (InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().IsOuterRing)
                                            {
                                                PieceToMove.transform.localScale = new Vector3(1.44f, 1.44f, 1.44f);
                                            }
                                            else
                                            {
                                                PieceToMove.transform.localScale = new Vector3(1.04f, 1.06f, 1.04f);
                                            }
                                        }


                                        InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Rconnect.Lsymbol = PieceToMove.Rsymbol;
                                        InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Rconnect.Lcolor = PieceToMove.Rcolor;

                                        InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Rconnect.LeftPieceParent = PieceToMove;

                                        InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Lconnect.Rsymbol = PieceToMove.Lsymbol;
                                        InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Lconnect.Rcolor = PieceToMove.Lcolor;

                                        InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Lconnect.RightPieceParent = PieceToMove;


                                        if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect != null)
                                        {
                                            InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().OuterRightConnect.ROutersymbol = PieceToMove.Rsymbol;
                                            InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().OuterRightConnect.ROutercolor = PieceToMove.Rcolor;

                                            InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().OuterLeftConnect.LOutersymbol = PieceToMove.Lsymbol;
                                            InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().OuterLeftConnect.LOutercolor = PieceToMove.Lcolor;

                                            InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().OuterLeftConnect.CheckConnection();
                                            InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().OuterRightConnect.CheckConnection();
                                        }

                                        InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Lconnect.CheckConnection();
                                        InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Rconnect.CheckConnection();

                                        InteractedColliders[InteractedColliders.Count - 1].GetComponent<CellInfo>().Full = true;
                                        GameManager.Instance.FullCellCounter++;
                                        //Debug.Log(InteractedColliders[InteractedColliders.Count - 1].name);
                                        //Debug.Log("full");
                                        PieceToMove = null;
                                    }
                                    else
                                    {
                                        PieceToMove.transform.position = PieceToMove.startingpos;
                                        PieceToMove.transform.parent = PieceToMove.OriginalParent;
                                        PieceToMove.transform.rotation = PieceToMove.startingRotation;

                                        if (PieceToMove.PartOfBoard)
                                        {
                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lsymbol = PieceToMove.Rsymbol;
                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lcolor = PieceToMove.Rcolor;

                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.LeftPieceParent = PieceToMove;

                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rsymbol = PieceToMove.Lsymbol;
                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rcolor = PieceToMove.Lcolor;

                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.RightPieceParent = PieceToMove;


                                            if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect != null)
                                            {
                                                PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.ROutersymbol = PieceToMove.Rsymbol;
                                                PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.ROutercolor = PieceToMove.Rcolor;

                                                PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.LOutersymbol = PieceToMove.Lsymbol;
                                                PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.LOutercolor = PieceToMove.Lcolor;

                                                PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.CheckConnection();
                                                PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.CheckConnection();
                                            }

                                            //Debug.Log("HERE?!");
                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.CheckConnection();
                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.CheckConnection();
                                            if (NumReduced)
                                            {
                                                GameManager.Instance.FullCellCounter++;
                                                //Debug.Log("here3");
                                            }
                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().Full = true;

                                            //Debug.Log("full");

                                        }

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
                                        //Debug.Log("full");
                                        if (NumReduced)
                                        {
                                            GameManager.Instance.FullCellCounter++;
                                        }
                                        //Debug.Log("here4");
                                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.CheckConnection();
                                        PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.CheckConnection();

                                        if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect != null)
                                        {
                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.CheckConnection();
                                            PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.CheckConnection();
                                        }

                                    }
                                    //Debug.Log("Found Nothing 2");

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
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lsymbol = PieceToMove.Rsymbol;
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lcolor = PieceToMove.Rcolor;

                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rsymbol = PieceToMove.Lsymbol;
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rcolor = PieceToMove.Lcolor;

                                    if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect != null)
                                    {
                                        PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.ROutersymbol = PieceToMove.Rsymbol;
                                        PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.ROutercolor = PieceToMove.Rcolor;

                                        PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.LOutersymbol = PieceToMove.Lsymbol;
                                        PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.LOutercolor = PieceToMove.Lcolor;
                                    }


                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().Full = true;
                                    //Debug.Log("full");
                                    if (NumReduced)
                                    {
                                        GameManager.Instance.FullCellCounter++;
                                    }
                                    //Debug.Log("here1");
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.CheckConnection();
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.CheckConnection();

                                    if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect != null)
                                    {
                                        PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.CheckConnection();
                                        PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.CheckConnection();
                                    }
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

                        if (GameManager.Instance.FullCellCounter == GameManager.Instance.CellsNeeedToFinish)
                        {
                            if (UiManager.Instance.Commit)
                            {
                                UiManager.Instance.Commit.interactable = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (Input.touchCount == 0)
                {
                    TimerCouter = 0;
                    endTouchPos = Mousepos;
                    //Debug.Log(Vector3.Magnitude(startTouchPos - endTouchPos));
                    if (Vector3.Magnitude(startTouchPos - endTouchPos) > 1)
                    {
                        //PieceToMove.IsBeingHeld = false;
                        //PieceToMove = null;

                        if (PieceToMove != null)
                        {
                            if (PieceToMove.PartOfBoard)
                            {
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lsymbol = PieceToMove.Rsymbol;
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.Lcolor = PieceToMove.Rcolor;

                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rsymbol = PieceToMove.Lsymbol;
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.Rcolor = PieceToMove.Lcolor;

                                if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect != null)
                                {
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.ROutersymbol = PieceToMove.Rsymbol;
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.ROutercolor = PieceToMove.Rcolor;

                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.LOutersymbol = PieceToMove.Lsymbol;
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.LOutercolor = PieceToMove.Lcolor;
                                }


                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Full = true;
                                //Debug.Log("full");

                                if (NumReduced)
                                {
                                    GameManager.Instance.FullCellCounter++;
                                }
                                //Debug.Log("here1");
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Lconnect.CheckConnection();
                                PieceToMove.OriginalParent.GetComponent<CellInfo>().Rconnect.CheckConnection();

                                if (PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect != null)
                                {
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterLeftConnect.CheckConnection();
                                    PieceToMove.OriginalParent.GetComponent<CellInfo>().OuterRightConnect.CheckConnection();
                                }
                            }
                            //Debug.Log("Found Nothing 1");
                            //ThisRenderer.sortingOrder = 0;
                            PieceToMove.transform.position = PieceToMove.startingpos;
                            PieceToMove.transform.parent = PieceToMove.OriginalParent;
                            PieceToMove.transform.rotation = PieceToMove.startingRotation;
                            PieceToMove.IsBeingHeld = false;
                            PieceToMove = null;
                        }

                        InteractedColliders.Clear();
                    }
                    else
                    {
                        if (PieceToMove != null)
                        {

                        }
                    }
                }
            }
        }
    }
}

