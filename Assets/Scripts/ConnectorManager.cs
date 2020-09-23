using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorManager : MonoBehaviour
{
    public bool HasLimiter;
    public bool SuccesfullConnectionMade;
    public bool BadConnectionMade;
    public bool LockPieces;

    public LimiterType TypeOfLimiter;

    public LootLimiterType TypeOfLootLimiter;

    public LootSliceType TypeOfLootSlice;

    public LootLockSliceType TypeOfLootLockSlice;


    public Symbols Rsymbol = Symbols.None;
    public ColorData Rcolor = ColorData.None;

    public Symbols Lsymbol = Symbols.None;
    public ColorData Lcolor = ColorData.None;

    public Symbols ROutersymbol = Symbols.None;
    public ColorData ROutercolor = ColorData.None;

    public Symbols LOutersymbol = Symbols.None;
    public ColorData LOutercolor = ColorData.None;

    public LimiterCellManager ConnectorLimiter;

    public PieceMoveManager LeftPieceParent, RightPieceParent;

    SpriteRenderer ThisSpriteRenderer, FirstChildSpriteRenderer, SecondChildSpriteRenderer;

    Rigidbody2D FirstChildBody, SecondChildBody;

    private void Update()
    {
        if (ConnectorLimiter)
        {
            if (ConnectorLimiter.TypeOfLimiter != LimiterType.None)
            {
                HasLimiter = true;
                TypeOfLimiter = ConnectorLimiter.TypeOfLimiter;
            }

            if (ConnectorLimiter.TypeOfLootLimiter != LootLimiterType.None)
            {
                HasLimiter = true;
                TypeOfLootLimiter = ConnectorLimiter.TypeOfLootLimiter;
            }
            if (ConnectorLimiter.TypeOfLootSlice != LootSliceType.None)
            {
                HasLimiter = true;
                TypeOfLootSlice = ConnectorLimiter.TypeOfLootSlice;
            }

            if (ConnectorLimiter.TypeOfLootLockSlice != LootLockSliceType.None)
            {
                HasLimiter = true;
                TypeOfLootLockSlice = ConnectorLimiter.TypeOfLootLockSlice;
                LockPieces = true;
            }
        }
    }

    public void CheckConnection()
    {
        if (HasLimiter)
        {
            switch (ConnectorLimiter.boolTypeOfSlice)
            {
                case BoolSliceType.NormalLimiter:
                    NormalLimiterSwitch();
                    break;
                case BoolSliceType.LootLimiter:
                    LootLimiterSwitch();
                    break;
                case BoolSliceType.LootSlice:
                    LootSliceSwitch();
                    break;
                case BoolSliceType.LootLockSlice:
                    LootLockSliceSwitch();
                    break;
                default:
                    break;
            }
        }
        else
        {
            //if (Rcolor != Colors.None && Rsymbol != Symbols.None && Lcolor != Colors.None && Lsymbol != Symbols.None)
            //{
            //    BadConnectionMade = true;
            //    SuccesfullConnectionMade = false;
            //}

            if ((Rcolor == Lcolor || Rsymbol == Lsymbol || Rcolor == ColorData.Joker || Lcolor == ColorData.Joker || Rsymbol == Symbols.Joker || Lsymbol == Symbols.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None && Rsymbol != Symbols.None && Rsymbol != Symbols.None)
            {
                //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color or Shape" + " " + transform.name);
                Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position,transform.rotation, transform), 1.5f);
                SuccesfullConnectionMade = true;
                GameManager.Instance.SuccesfullConnectionsMade++;
                BadConnectionMade = false;
            }
        }

        if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
        {
            if ((ROutercolor == LOutercolor || ROutersymbol == LOutersymbol || ROutercolor == ColorData.Joker || LOutercolor == ColorData.Joker || ROutersymbol == Symbols.Joker || LOutersymbol == Symbols.Joker) && ROutercolor != ColorData.None && LOutercolor != ColorData.None && ROutersymbol != Symbols.None && LOutersymbol != Symbols.None)
            {
                //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color or Shape" + " " + transform.name);
                Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                SuccesfullConnectionMade = true;
                GameManager.Instance.SuccesfullConnectionsMade++;
                BadConnectionMade = false;
            }
        }
    }

    public void BadConnectionToggle()
    {
        if (Rcolor != ColorData.None && Rsymbol != Symbols.None && Lcolor != ColorData.None && Lsymbol != Symbols.None)
        {
            BadConnectionMade = true;
            SuccesfullConnectionMade = false;
        }
    }

    public void NormalLimiterSwitch()
    {
        switch (ConnectorLimiter.TypeOfLimiter)
        {
            case LimiterType.GeneralColor:
                if ((Rcolor == Lcolor || Rcolor == ColorData.Joker || Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LimiterType.GeneralSymbol:
                if ((Rsymbol == Lsymbol || Rsymbol == Symbols.Joker || Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LimiterType.Red:
                if ((Rcolor == ColorData.Red && Lcolor == ColorData.Red) || (Rcolor == ColorData.Red && Lcolor == ColorData.Joker) || (Rcolor == ColorData.Joker && Lcolor == ColorData.Red || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LimiterType.Blue:
                if ((Rcolor == ColorData.Blue && Lcolor == ColorData.Blue) || (Rcolor == ColorData.Blue && Lcolor == ColorData.Joker) || (Rcolor == ColorData.Joker && Lcolor == ColorData.Blue || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LimiterType.Yellow:
                if ((Rcolor == ColorData.Yellow && Lcolor == ColorData.Yellow || Rcolor == ColorData.Joker && Lcolor == ColorData.Yellow || Rcolor == ColorData.Yellow && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LimiterType.Pink:
                if ((Rcolor == ColorData.Pink && Lcolor == ColorData.Pink || Rcolor == ColorData.Joker && Lcolor == ColorData.Pink || Rcolor == ColorData.Pink && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LimiterType.Square:
                if ((Rsymbol == Symbols.Square && Lsymbol == Symbols.Square || Rsymbol == Symbols.Square && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Square || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LimiterType.Triangle:
                if ((Rsymbol == Symbols.Triangle && Lsymbol == Symbols.Triangle || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Triangle || Rsymbol == Symbols.Triangle && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LimiterType.Plus:
                if ((Rsymbol == Symbols.Plus && Lsymbol == Symbols.Plus || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Plus || Rsymbol == Symbols.Plus && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LimiterType.Circle:
                if ((Rsymbol == Symbols.Circle && Lsymbol == Symbols.Circle || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Circle || Rsymbol == Symbols.Circle && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            default:
                if (Rcolor != ColorData.None && Rsymbol != Symbols.None && Lcolor != ColorData.None && Lsymbol != Symbols.None)
                {
                    BadConnectionMade = true;
                    SuccesfullConnectionMade = false;
                }
                break;
        }
    }

    public void LootLimiterSwitch()
    {
        switch (ConnectorLimiter.TypeOfLootLimiter)
        {
            case LootLimiterType.LootLimiterGeneralColor:
                if ((Rcolor == Lcolor || Rcolor == ColorData.Joker || Lcolor == ColorData.Joker)&& Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LootLimiterType.LootLimiterGeneralSymbol:
                if ((Rsymbol == Lsymbol || Rsymbol == Symbols.Joker || Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LootLimiterType.LootLimiterSpecificRed:
                if ((Rcolor == ColorData.Red && Lcolor == ColorData.Red || Rcolor == ColorData.Joker && Lcolor == ColorData.Red || Rcolor == ColorData.Red && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LootLimiterType.LootLimiterSpecificBlue:
                if ((Rcolor == ColorData.Blue && Lcolor == ColorData.Blue || Rcolor == ColorData.Joker && Lcolor == ColorData.Blue || Rcolor == ColorData.Blue && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LootLimiterType.LootLimiterSpecificYellow:
                if ((Rcolor == ColorData.Yellow && Lcolor == ColorData.Yellow || Rcolor == ColorData.Joker && Lcolor == ColorData.Yellow || Rcolor == ColorData.Yellow && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LootLimiterType.LootLimiterSpecificPink:
                if ((Rcolor == ColorData.Pink && Lcolor == ColorData.Pink || Rcolor == ColorData.Joker && Lcolor == ColorData.Pink || Rcolor == ColorData.Pink && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LootLimiterType.LootLimiterSpecificSquare:
                if ((Rsymbol == Symbols.Square && Lsymbol == Symbols.Square || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Square || Rsymbol == Symbols.Square && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LootLimiterType.LootLimiterSpecificTriangle:
                if ((Rsymbol == Symbols.Triangle && Lsymbol == Symbols.Triangle || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Triangle || Rsymbol == Symbols.Triangle && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LootLimiterType.LootLimiterSpecificPlus:
                if ((Rsymbol == Symbols.Plus && Lsymbol == Symbols.Plus || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Plus || Rsymbol == Symbols.Plus && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            case LootLimiterType.LootLimiterSpecificCircle:
                if ((Rsymbol == Symbols.Circle && Lsymbol == Symbols.Circle || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Circle || Rsymbol == Symbols.Circle && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    BadConnectionToggle();
                }
                break;
            default:
                if (Rcolor != ColorData.None && Rsymbol != Symbols.None && Lcolor != ColorData.None && Lsymbol != Symbols.None)
                {
                    BadConnectionMade = true;
                    SuccesfullConnectionMade = false;
                }
                break;
        }
    }

    public void LootSliceSwitch()
    {
        switch (ConnectorLimiter.TypeOfLootSlice)
        {
            case LootSliceType.LootGeneralColor:
                if ((Rcolor == Lcolor || Rcolor == ColorData.Joker || Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    lootsliceReset();
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootSliceType.LootGeneralSymbol:
                if ((Rsymbol == Lsymbol || Rsymbol == Symbols.Joker || Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    lootsliceReset();

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootSliceType.LootSpecificRed:
                if ((Rcolor == ColorData.Red && Lcolor == ColorData.Red  || Rcolor == ColorData.Joker && Lcolor == ColorData.Red || Rcolor == ColorData.Red && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    lootsliceReset();

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootSliceType.LootSpecificBlue:
                if ((Rcolor == ColorData.Blue && Lcolor == ColorData.Blue || Rcolor == ColorData.Joker && Lcolor == ColorData.Blue || Rcolor == ColorData.Blue && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    lootsliceReset();

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootSliceType.LootSpecificYellow:
                if ((Rcolor == ColorData.Yellow && Lcolor == ColorData.Yellow || Rcolor == ColorData.Joker && Lcolor == ColorData.Yellow || Rcolor == ColorData.Yellow && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    lootsliceReset();

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootSliceType.LootSpecificPink:
                if ((Rcolor == ColorData.Pink && Lcolor == ColorData.Pink || Rcolor == ColorData.Joker && Lcolor == ColorData.Pink || Rcolor == ColorData.Pink && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    lootsliceReset();

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootSliceType.LootSpecificSquare:
                if ((Rsymbol == Symbols.Square && Lsymbol == Symbols.Square || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Square || Rsymbol == Symbols.Square && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    lootsliceReset();

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootSliceType.LootSpecificTriangle:
                if ((Rsymbol == Symbols.Triangle && Lsymbol == Symbols.Triangle || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Triangle || Rsymbol == Symbols.Triangle && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    lootsliceReset();

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootSliceType.LootSpecificPlus:
                if ((Rsymbol == Symbols.Plus && Lsymbol == Symbols.Plus || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Plus || Rsymbol == Symbols.Plus && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    lootsliceReset();

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootSliceType.LootSpecificCircle:
                if ((Rsymbol == Symbols.Circle && Lsymbol == Symbols.Circle || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Circle || Rsymbol == Symbols.Circle && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    lootsliceReset();

                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            default:
                if (Rcolor != ColorData.None && Rsymbol != Symbols.None && Lcolor != ColorData.None && Lsymbol != Symbols.None)
                {
                    BadConnectionMade = true;
                    SuccesfullConnectionMade = false;
                }
                break;
        }
    }

    public void LootLockSliceSwitch()
    {
        switch (ConnectorLimiter.TypeOfLootLockSlice)
        {
            case LootLockSliceType.LootLockGeneralColor:
                if ((Rcolor == Lcolor || Rcolor == ColorData.Joker || Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    LeftPieceParent.Locked = true;
                    RightPieceParent.Locked = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;

                    lootlocksliceReset();
                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootLockSliceType.LootLockGeneralSymbol:
                if ((Rsymbol == Lsymbol || Rsymbol == Symbols.Joker || Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    LeftPieceParent.Locked = true;
                    RightPieceParent.Locked = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;

                    lootlocksliceReset();

                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootLockSliceType.LootLockSpecificRed:
                if ((Rcolor == ColorData.Red && Lcolor == ColorData.Red || Rcolor == ColorData.Joker && Lcolor == ColorData.Red || Rcolor == ColorData.Red && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    LeftPieceParent.Locked = true;
                    RightPieceParent.Locked = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;

                    lootlocksliceReset();

                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootLockSliceType.LootLockSpecificBlue:
                if ((Rcolor == ColorData.Blue && Lcolor == ColorData.Blue || Rcolor == ColorData.Joker && Lcolor == ColorData.Blue || Rcolor == ColorData.Blue && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    LeftPieceParent.Locked = true;
                    RightPieceParent.Locked = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;

                    lootlocksliceReset();

                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootLockSliceType.LootLockSpecificYellow:
                if ((Rcolor == ColorData.Yellow && Lcolor == ColorData.Yellow || Rcolor == ColorData.Joker && Lcolor == ColorData.Yellow || Rcolor == ColorData.Yellow && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    LeftPieceParent.Locked = true;
                    RightPieceParent.Locked = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;

                    lootlocksliceReset();

                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootLockSliceType.LootLockSpecificPink:
                if ((Rcolor == ColorData.Pink && Lcolor == ColorData.Pink || Rcolor == ColorData.Joker && Lcolor == ColorData.Pink || Rcolor == ColorData.Pink && Lcolor == ColorData.Joker || Rcolor == ColorData.Joker && Lcolor == ColorData.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    LeftPieceParent.Locked = true;
                    RightPieceParent.Locked = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;

                    lootlocksliceReset();

                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootLockSliceType.LootLockSpecificSquare:
                if ((Rsymbol == Symbols.Square && Lsymbol == Symbols.Square || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Square || Rsymbol == Symbols.Square && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    LeftPieceParent.Locked = true;
                    RightPieceParent.Locked = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;

                    lootlocksliceReset();

                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootLockSliceType.LootLockSpecificTriangle:
                if ((Rsymbol == Symbols.Triangle && Lsymbol == Symbols.Triangle || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Triangle || Rsymbol == Symbols.Triangle && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    LeftPieceParent.Locked = true;
                    RightPieceParent.Locked = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;

                    lootlocksliceReset();

                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootLockSliceType.LootLockSpecificPlus:
                if ((Rsymbol == Symbols.Plus && Lsymbol == Symbols.Plus || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Plus || Rsymbol == Symbols.Plus && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    LeftPieceParent.Locked = true;
                    RightPieceParent.Locked = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;

                    lootlocksliceReset();

                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            case LootLockSliceType.LootLockSpecificCircle:
                if ((Rsymbol == Symbols.Circle && Lsymbol == Symbols.Circle || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Circle || Rsymbol == Symbols.Circle && Lsymbol == Symbols.Joker || Rsymbol == Symbols.Joker && Lsymbol == Symbols.Joker) && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                {
                    //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                    Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
                    SuccesfullConnectionMade = true;
                    LeftPieceParent.Locked = true;
                    RightPieceParent.Locked = true;
                    GameManager.Instance.SuccesfullConnectionsMade++;
                    BadConnectionMade = false;

                    lootlocksliceReset();

                }
                else
                {
                    CheckForNormalConnectionSlices();
                }
                break;
            default:
                if (Rcolor != ColorData.None && Rsymbol != Symbols.None && Lcolor != ColorData.None && Lsymbol != Symbols.None)
                {
                    BadConnectionMade = true;
                    SuccesfullConnectionMade = false;
                }
                break;
        }
    }

    public void lootsliceReset()
    {
        if (ConnectorLimiter.transform.childCount > 0)
        {
            ThisSpriteRenderer = ConnectorLimiter.transform.GetChild(0).GetComponent<SpriteRenderer>();

            FirstChildSpriteRenderer = ConnectorLimiter.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();

            FirstChildBody = ConnectorLimiter.transform.GetChild(0).transform.GetChild(0).GetComponent<Rigidbody2D>();
        }

        ConnectorLimiter.TypeOfLootSlice = LootSliceType.None;
        ConnectorLimiter.boolTypeOfSlice = BoolSliceType.None;
        ConnectorLimiter.IsFull = false;
        //Debug.Log("4");

        HasLimiter = false;
        TypeOfLootSlice = LootSliceType.None;
        ThisSpriteRenderer.color = new Color(ThisSpriteRenderer.color.r, ThisSpriteRenderer.color.g, ThisSpriteRenderer.color.b, 0.3f);
        FirstChildSpriteRenderer.color = new Color(FirstChildSpriteRenderer.color.r, FirstChildSpriteRenderer.color.g, FirstChildSpriteRenderer.color.b, 0.3f);
        FirstChildBody.gravityScale = 1;

        RecieveLoot();
    }

    public void lootlocksliceReset()
    {
        if (ConnectorLimiter.transform.childCount > 0)
        {
            ThisSpriteRenderer = ConnectorLimiter.transform.GetChild(0).GetComponent<SpriteRenderer>();

            FirstChildSpriteRenderer = ConnectorLimiter.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();

            SecondChildSpriteRenderer = ConnectorLimiter.transform.GetChild(0).transform.GetChild(1).GetComponent<SpriteRenderer>();

            SecondChildBody = ConnectorLimiter.transform.GetChild(0).transform.GetChild(1).GetComponent<Rigidbody2D>();
        }

        ConnectorLimiter.TypeOfLootLockSlice = LootLockSliceType.None;
        ConnectorLimiter.boolTypeOfSlice = BoolSliceType.None;
        ConnectorLimiter.IsFull = false;
        //Debug.Log("5");
        HasLimiter = false;
        TypeOfLootLockSlice = LootLockSliceType.None;

        ThisSpriteRenderer.color = new Color(ThisSpriteRenderer.color.r, ThisSpriteRenderer.color.g, ThisSpriteRenderer.color.b, 0.3f);
        FirstChildSpriteRenderer.color = new Color(FirstChildSpriteRenderer.color.r, FirstChildSpriteRenderer.color.g, FirstChildSpriteRenderer.color.b, 0.3f);
        SecondChildSpriteRenderer.color = new Color(SecondChildSpriteRenderer.color.r, SecondChildSpriteRenderer.color.g, SecondChildSpriteRenderer.color.b, 0.3f);

        SecondChildBody.gravityScale = 1;
        RecieveLoot();
    }

    public void CheckForNormalConnectionSlices()
    {
        if ((Rcolor == Lcolor || Rsymbol == Lsymbol || Rcolor == ColorData.Joker || Lcolor == ColorData.Joker || Rsymbol == Symbols.Joker || Lsymbol == Symbols.Joker) && Rcolor != ColorData.None && Lcolor != ColorData.None && Rsymbol != Symbols.None && Rsymbol != Symbols.None)
        {
            //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color or Shape" + " " + transform.name);
            Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform.position, transform.rotation, transform), 1.5f);
            SuccesfullConnectionMade = true;
            GameManager.Instance.SuccesfullConnectionsMade++;
            BadConnectionMade = false;
        }
        else
        {
            BadConnectionToggle();
        }
    }

    public void RecieveLoot()
    {
        //Debug.Log("Recieved Loot");
        int Randomloot = Random.Range(0, GameManager.Instance.LevelSpecificLoot.Count);

        //Debug.Log(Randomloot);
        //Debug.Log(GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].LootForLevel[Randomloot]);

        switch (GameManager.Instance.LevelSpecificLoot[Randomloot])
        {
            case LootType.SmallGold:
                LootManager.Instance.GainGold(LootType.SmallGold);
                break;
            case LootType.BigGold:
                LootManager.Instance.GainGold(LootType.BigGold);
                break;
            case LootType.SmallRuby:
                LootManager.Instance.GainRubie(LootType.SmallRuby);
                break;
            case LootType.BigRuby:
                LootManager.Instance.GainRubie(LootType.BigRuby);
                break;
            case LootType.MagicItem:
                LootManager.Instance.GainMagicalItems();
                break;
            default:
                break;
        }

        GameManager.Instance.ThePlayer.SaveDate();
        GameManager.Instance.LevelSpecificLoot.RemoveAt(Randomloot);
    }
}
