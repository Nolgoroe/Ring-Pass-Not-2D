using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerUpChooseItemTypes
{
    Joker,
    Switch,
    TileBomb,
    SliceBomb,
    ColorTransform,
    ShapeTransform,
    Reshuffle,
    None
}

public class PowerUps : MonoBehaviour
{
    public GameObject Clip;

    public int TimesClickedDeal;

    public bool Reshuffled = false;
    public bool LoseGame = false;

    public int SlotsInClip = 4;
    public bool UsingPowerUp;

    public PowerUpChooseItemTypes PowerUpInUse;

    public ColorData ColorForColorTransformPowerUp;
    public Symbols SymbolForShapeTransformPowerUp;

    public Button PowerUpButton;

    public bool HasTargetForPowerUp = false;

    public void Init()
    {
        TimesClickedDeal = 0;
        Clip = GameObject.FindGameObjectWithTag("Clip").gameObject;
    }

    public void UsingPowerUpToggle(int TypeOfPowerUp)
    {
        UsingPowerUp = true;

        PowerUpInUse = (PowerUpChooseItemTypes)TypeOfPowerUp;
    }

    public void FillPowerUpButton(Button ThePowerUpButton)
    {
        PowerUpButton = ThePowerUpButton;
    }
    public void ChooseColorForColorTransformPowerUp(int Color)
    {
        ColorForColorTransformPowerUp = (ColorData)Color;
        //switch (Color)
        //{
        //    case 0:
        //        ColorForColorTransformPowerUp = ColorData.Red;
        //        break;
        //    case 1:
        //        ColorForColorTransformPowerUp = ColorData.Pink;
        //        break;
        //    case 2:
        //        ColorForColorTransformPowerUp = ColorData.Blue;
        //        break;
        //    case 3:
        //        ColorForColorTransformPowerUp = ColorData.Yellow;
        //        break;
        //    default:
        //        break;
        //}
    }

    public void ChooseSymbolForShapeTransformPowerUp(int Shape)
    {
        SymbolForShapeTransformPowerUp = (Symbols)Shape;
        //switch (Shape)
        //{
        //    case 0:
        //        SymbolForShapeTransformPowerUp = Symbols.Circle;
        //        break;
        //    case 1:
        //        SymbolForShapeTransformPowerUp = Symbols.Plus;
        //        break;
        //    case 2:
        //        SymbolForShapeTransformPowerUp = Symbols.Triangle;
        //        break;
        //    case 3:
        //        SymbolForShapeTransformPowerUp = Symbols.Square;
        //        break;
        //    default:
        //        break;
        //}
    }

    public void DealCards() //// Put on Deal Button in scene
    {
        if (!LoseGame)
        {
            if (!Reshuffled)
            {
                if (TimesClickedDeal == 0)
                {
                    Clip.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    Clip.transform.GetChild(TimesClickedDeal).gameObject.SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < TimesClickedDeal + 1; i++)
                {
                    Clip.transform.GetChild(i).gameObject.SetActive(false);
                }

                Reshuffled = false;
            }

            TimesClickedDeal++;

            ResuffleParts();

            if (TimesClickedDeal == 4)
            {
                LoseGame = true;
                UiManager.Instance.YouLoseMessage();
            }
        }
    }

    public void RefillClip() /// Put on refill Power Up button in scene
    {
        if (!LoseGame)
        {
            if (TimesClickedDeal == 0 || Reshuffled)
            {
                UiManager.Instance.SlotsFullMessage();
                UsingPowerUp = false;
                PowerUpInUse = PowerUpChooseItemTypes.None;
                PowerUpButton = null;
                return;
            }

            for (int i = 0; i < SlotsInClip; i++)
            {
                Clip.transform.GetChild(i).gameObject.SetActive(true);
            }

            UsingPowerUp = false;
            Reshuffled = true;
            ResuffleParts();

            DoAfterSuccessfullPowerUp();
            PowerUpInUse = PowerUpChooseItemTypes.None;
            PowerUpButton = null;
        }
    }

    public void ResuffleParts()
    {
        for (int i = 0; i < SlotsInClip; i++)
        {
            if (Clip.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                Destroy(Clip.transform.GetChild(i).GetChild(0).gameObject);
            }
        }

        if (!Reshuffled)
        {
            for (int i = 0; i < SlotsInClip - TimesClickedDeal; i++)
            {
                GameManager.Instance.FillClipPiece(Clip.transform.GetChild(i + TimesClickedDeal).gameObject);
            }
        }
        else
        {
            for (int i = 0; i < SlotsInClip; i++)
            {
                GameManager.Instance.FillClipPiece(Clip.transform.GetChild(i).gameObject);
            }
        }
    }

    public void TileBomb(LimiterPiece TileToBomb)
    {
        Debug.Log("Used Tile Bomb Power Up");

        TileToBomb.ParentLimiterCell.TypeOfLimiter = LimiterType.None;
        TileToBomb.ParentLimiterCell.TypeOfLootLimiter = LootLimiterType.None;
        TileToBomb.ParentLimiterCell.TypeOfLootLockSlice = LootLockSliceType.None;
        TileToBomb.ParentLimiterCell.TypeOfLootSlice = LootSliceType.None;
        TileToBomb.ParentLimiterCell.boolTypeOfSlice = BoolSliceType.None;
        TileToBomb.ParentLimiterCell.IsFull = false;

        for (int i = 0; i < GameManager.Instance.Connectors.Count; i++)
        {
            if(TileToBomb.ParentLimiterCell == GameManager.Instance.Connectors[i].ConnectorLimiter)
            {
                GameManager.Instance.Connectors[i].HasLimiter = false;
                GameManager.Instance.Connectors[i].LockPieces = false;
                GameManager.Instance.Connectors[i].TypeOfLimiter = LimiterType.None;
                GameManager.Instance.Connectors[i].TypeOfLootLockSlice = LootLockSliceType.None;
                GameManager.Instance.Connectors[i].TypeOfLootSlice = LootSliceType.None;
                GameManager.Instance.Connectors[i].TypeOfLootLimiter = LootLimiterType.None;


                if (GameManager.Instance.Connectors[i].LeftPieceParent != null && GameManager.Instance.Connectors[i].RightPieceParent != null)
                {
                    if (GameManager.Instance.Connectors[i].LeftPieceParent.Locked && GameManager.Instance.Connectors[i].RightPieceParent.Locked)
                    {
                        GameManager.Instance.Connectors[i].LeftPieceParent.Locked = false;
                        GameManager.Instance.Connectors[i].RightPieceParent.Locked = false;

                        foreach (Transform child in GameManager.Instance.Connectors[i].LeftPieceParent.transform)
                        {
                            if (child.CompareTag("Lock"))
                            {
                                child.gameObject.SetActive(false);
                            }
                        }

                        foreach (Transform child in GameManager.Instance.Connectors[i].RightPieceParent.transform)
                        {
                            if (child.CompareTag("Lock"))
                            {
                                child.gameObject.SetActive(false);
                            }
                        }

                    }
                }
            }
        }

        if (GameManager.Instance.FullCellCounter == GameManager.Instance.CellsNeeedToFinish)
        {
            if (UiManager.Instance.Commit)
            {
                UiManager.Instance.Commit.interactable = true;
            }
        }

        Destroy(TileToBomb.gameObject);

        DoAfterSuccessfullPowerUp();
        //UsingPowerUp = false;
    }

    public void SliceBomb(PieceMoveManager PieceToBomb)
    {
        if (PieceToBomb.PartOfBoard)
        {
            Debug.Log("Used Slice Bomb Power Up");

            CellInfo CellParent = PieceToBomb.transform.parent.GetComponent<CellInfo>();

            CellParent.Full = false;
            //Debug.Log("2");

            CellParent.Rconnect.Lsymbol = Symbols.None;
            CellParent.Rconnect.Lcolor = ColorData.None;


            CellParent.Lconnect.Rsymbol = Symbols.None;
            CellParent.Lconnect.Rcolor = ColorData.None;

            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                CellParent.OuterRightConnect.ROutercolor = ColorData.None;
                CellParent.OuterRightConnect.ROutersymbol = Symbols.None;

                CellParent.OuterLeftConnect.LOutercolor = ColorData.None;
                CellParent.OuterLeftConnect.LOutersymbol = Symbols.None;

            }

            if (CellParent.Rconnect.LockPieces)
            {
                CellParent.Rconnect.LeftPieceParent.Locked = false;
                CellParent.Rconnect.RightPieceParent.Locked = false;


                foreach (Transform child in CellParent.Rconnect.LeftPieceParent.transform)
                {
                    if (child.CompareTag("Lock"))
                    {
                        child.gameObject.SetActive(false);
                    }
                }

                foreach (Transform child in CellParent.Rconnect.RightPieceParent.transform)
                {
                    if (child.CompareTag("Lock"))
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }


            if (CellParent.Lconnect.LockPieces)
            {
                CellParent.Lconnect.LeftPieceParent.Locked = false;
                CellParent.Lconnect.RightPieceParent.Locked = false;

                foreach (Transform child in CellParent.Lconnect.LeftPieceParent.transform)
                {
                    if (child.CompareTag("Lock"))
                    {
                        child.gameObject.SetActive(false);
                    }
                }

                foreach (Transform child in CellParent.Lconnect.RightPieceParent.transform)
                {
                    if (child.CompareTag("Lock"))
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }


            CellParent.Lconnect.RightPieceParent = null;

            CellParent.Rconnect.LeftPieceParent = null;

            if (GameManager.Instance.SuccesfullConnectionsMade == GameManager.Instance.ConnectionsNeededToFinishLevel)
            {
                UiManager.Instance.Commit.interactable = false;
            }

            if (CellParent.Rconnect.SuccesfullConnectionMade)
            {
                CellParent.Rconnect.SuccesfullConnectionMade = false;
                GameManager.Instance.SuccesfullConnectionsMade--;
            }

            if (CellParent.Lconnect.SuccesfullConnectionMade)
            {
                CellParent.Lconnect.SuccesfullConnectionMade = false;
                GameManager.Instance.SuccesfullConnectionsMade--;
            }

            if (CellParent.Rconnect.BadConnectionMade)
            {
                CellParent.Rconnect.BadConnectionMade = false;
            }

            if (CellParent.Lconnect.BadConnectionMade)
            {
                CellParent.Lconnect.BadConnectionMade = false;
            }


            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                if (CellParent.OuterRightConnect.SuccesfullConnectionMade)
                {
                    CellParent.OuterRightConnect.SuccesfullConnectionMade = false;
                    GameManager.Instance.SuccesfullConnectionsMade--;
                }

                if (CellParent.OuterLeftConnect.SuccesfullConnectionMade)
                {
                    CellParent.OuterLeftConnect.SuccesfullConnectionMade = false;
                    GameManager.Instance.SuccesfullConnectionsMade--;
                }

                if (CellParent.OuterRightConnect.BadConnectionMade)
                {
                    CellParent.OuterRightConnect.BadConnectionMade = false;
                }

                if (CellParent.OuterLeftConnect.BadConnectionMade)
                {
                    CellParent.OuterLeftConnect.BadConnectionMade = false;
                }
            }

            MouseCollisionDetection.Instance.ObjectToUsePowerup = null;
            MouseCollisionDetection.Instance.BombedSlice = true;
           GameManager.Instance.FullCellCounter--;
            Destroy(PieceToBomb.gameObject);
            //UsingPowerUp = false;
            DoAfterSuccessfullPowerUp();
        }

    }

    public void SwitchPieceSides(PieceMoveManager PieceToSwtich)
    {
        if (PieceToSwtich.PartOfBoard && !PieceToSwtich.Locked && (PieceToSwtich.Rsymbol != PieceToSwtich.Lsymbol && PieceToSwtich.Rcolor != PieceToSwtich.Lcolor))
        {
            Debug.Log("Used Switch Power Up");

            CellInfo PieceParent = PieceToSwtich.transform.parent.GetComponent<CellInfo>();


            ColorData TempColor = PieceToSwtich.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;
            Symbols TempSymbol = PieceToSwtich.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;


            PieceToSwtich.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor = PieceToSwtich.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;
            PieceToSwtich.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol = PieceToSwtich.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;

            PieceToSwtich.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor = TempColor;
            PieceToSwtich.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol = TempSymbol;


            PieceToSwtich.Rsymbol = PieceToSwtich.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;
            PieceToSwtich.Rcolor = PieceToSwtich.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;

            PieceToSwtich.Lsymbol = PieceToSwtich.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol;
            PieceToSwtich.Lcolor = PieceToSwtich.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;

            PieceToSwtich.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().ChooseColorAndSpritePowerUp(PieceToSwtich.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor, PieceToSwtich.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol);
            PieceToSwtich.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().ChooseColorAndSpritePowerUp(PieceToSwtich.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor, PieceToSwtich.RightSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceSymbol);


            ////// Change Connectors Now


            TempColor = PieceParent.Rconnect.Lcolor;
            TempSymbol = PieceParent.Rconnect.Lsymbol;

            PieceParent.Rconnect.Lcolor = PieceParent.Lconnect.Rcolor;
            PieceParent.Rconnect.Lsymbol = PieceParent.Lconnect.Rsymbol;

            PieceParent.Lconnect.Rcolor = TempColor;
            PieceParent.Lconnect.Rsymbol = TempSymbol;

            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {

                TempColor = PieceParent.OuterRightConnect.ROutercolor;
                TempSymbol = PieceParent.OuterRightConnect.ROutersymbol;

                PieceParent.OuterRightConnect.ROutercolor = PieceParent.OuterLeftConnect.LOutercolor;
                PieceParent.OuterRightConnect.ROutersymbol = PieceParent.OuterLeftConnect.LOutersymbol;

                PieceParent.OuterLeftConnect.LOutercolor = TempColor;
                PieceParent.OuterLeftConnect.LOutersymbol = TempSymbol;

            }

            if (GameManager.Instance.SuccesfullConnectionsMade == GameManager.Instance.ConnectionsNeededToFinishLevel)
            {
                UiManager.Instance.Commit.interactable = false;
            }

            if (PieceParent.Rconnect.SuccesfullConnectionMade)
            {
                PieceParent.Rconnect.SuccesfullConnectionMade = false;
                GameManager.Instance.SuccesfullConnectionsMade--;
            }

            if (PieceParent.Lconnect.SuccesfullConnectionMade)
            {
                PieceParent.Lconnect.SuccesfullConnectionMade = false;
                GameManager.Instance.SuccesfullConnectionsMade--;
            }

            if (PieceParent.Rconnect.BadConnectionMade)
            {
                PieceParent.Rconnect.BadConnectionMade = false;
            }

            if (PieceParent.Lconnect.BadConnectionMade)
            {
                PieceParent.Lconnect.BadConnectionMade = false;
            }

            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                if (PieceParent.OuterRightConnect.SuccesfullConnectionMade)
                {
                    PieceParent.OuterRightConnect.SuccesfullConnectionMade = false;
                    GameManager.Instance.SuccesfullConnectionsMade--;
                }

                if (PieceParent.OuterLeftConnect.SuccesfullConnectionMade)
                {
                    PieceParent.OuterLeftConnect.SuccesfullConnectionMade = false;
                    GameManager.Instance.SuccesfullConnectionsMade--;
                }

                if (PieceParent.OuterRightConnect.BadConnectionMade)
                {
                    PieceParent.OuterRightConnect.BadConnectionMade = false;
                }

                if (PieceParent.OuterLeftConnect.BadConnectionMade)
                {
                    PieceParent.OuterLeftConnect.BadConnectionMade = false;
                }

                PieceParent.OuterRightConnect.CheckConnection();
                PieceParent.OuterLeftConnect.CheckConnection();
            }
            PieceParent.Rconnect.CheckConnection();
            PieceParent.Lconnect.CheckConnection();


            if (GameManager.Instance.FullCellCounter == GameManager.Instance.CellsNeeedToFinish)
            {
                if (UiManager.Instance.Commit)
                {
                    UiManager.Instance.Commit.interactable = true;
                }
            }

            DoAfterSuccessfullPowerUp();
            // UsingPowerUp = false;
        }
    }

    public void Joker(PieceMoveManager PieceToJoker)
    {
        PowerUpInUse = PowerUpChooseItemTypes.Joker;

        if (PieceToJoker.PartOfBoard && !PieceToJoker.Locked && !PieceToJoker.IsJoker)
        {
            Debug.Log("Used Joker Power Up");

            PieceToJoker.IsJoker = true;

            PieceToJoker.Rcolor = ColorData.Joker;
            PieceToJoker.Rsymbol = Symbols.Joker;

            PieceToJoker.Lcolor = ColorData.Joker;
            PieceToJoker.Lsymbol = Symbols.Joker;

            ////// Change Connectors Now
            CellInfo PieceParent = PieceToJoker.transform.parent.GetComponent<CellInfo>();

            PieceParent.Rconnect.Lcolor = ColorData.Joker;
            PieceParent.Rconnect.Lsymbol = Symbols.Joker;

            PieceParent.Lconnect.Rcolor = ColorData.Joker;
            PieceParent.Lconnect.Rsymbol = Symbols.Joker;

            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                PieceParent.OuterRightConnect.ROutercolor = ColorData.Joker;
                PieceParent.OuterRightConnect.ROutersymbol = Symbols.Joker;

                PieceParent.OuterLeftConnect.LOutercolor = ColorData.Joker;
                PieceParent.OuterLeftConnect.LOutersymbol = Symbols.Joker;
            }

            //if (!PieceParent.Rconnect.SuccesfullConnectionMade)
            //{
            //    PieceParent.Rconnect.CheckConnection();
            //}

            //if (!PieceParent.Lconnect.SuccesfullConnectionMade)
            //{
            //    PieceParent.Lconnect.CheckConnection();
            //}

            if (PieceParent.Rconnect.SuccesfullConnectionMade)
            {
                PieceParent.Rconnect.SuccesfullConnectionMade = false;
                GameManager.Instance.SuccesfullConnectionsMade--;
            }

            if (PieceParent.Lconnect.SuccesfullConnectionMade)
            {
                PieceParent.Lconnect.SuccesfullConnectionMade = false;
                GameManager.Instance.SuccesfullConnectionsMade--;
            }

            if (PieceParent.Rconnect.BadConnectionMade)
            {
                PieceParent.Rconnect.BadConnectionMade = false;
            }

            if (PieceParent.Lconnect.BadConnectionMade)
            {
                PieceParent.Lconnect.BadConnectionMade = false;
            }

            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                if (PieceParent.OuterRightConnect.SuccesfullConnectionMade)
                {
                    PieceParent.OuterRightConnect.SuccesfullConnectionMade = false;
                    GameManager.Instance.SuccesfullConnectionsMade--;
                }

                if (PieceParent.OuterLeftConnect.SuccesfullConnectionMade)
                {
                    PieceParent.OuterLeftConnect.SuccesfullConnectionMade = false;
                    GameManager.Instance.SuccesfullConnectionsMade--;
                }

                if (PieceParent.OuterRightConnect.BadConnectionMade)
                {
                    PieceParent.OuterRightConnect.BadConnectionMade = false;
                }

                if (PieceParent.OuterLeftConnect.BadConnectionMade)
                {
                    PieceParent.OuterLeftConnect.BadConnectionMade = false;
                }

                PieceParent.OuterRightConnect.CheckConnection();
                PieceParent.OuterLeftConnect.CheckConnection();
            }

            PieceParent.Rconnect.CheckConnection();
            PieceParent.Lconnect.CheckConnection();

            if (GameManager.Instance.FullCellCounter == GameManager.Instance.CellsNeeedToFinish)
            {
                if (UiManager.Instance.Commit)
                {
                    UiManager.Instance.Commit.interactable = true;
                }
            }

            DoAfterSuccessfullPowerUp();
            //UsingPowerUp = false;
        }
    }

    public void ColorTransform(ColorSymbolData PieceToTransform)
    {
        PieceMoveManager pieceParent = PieceToTransform.transform.parent.parent.GetComponent<PieceMoveManager>();

        if (pieceParent.PartOfBoard && !pieceParent.Locked && PieceToTransform.PieceColor != ColorForColorTransformPowerUp)
        {
            //Debug.Log("Used Color Transform Power Up");

            PieceToTransform.PieceColor = ColorForColorTransformPowerUp;
            
            PieceToTransform.ChooseColorAndSpritePowerUp(ColorForColorTransformPowerUp, PieceToTransform.PieceSymbol);

            ////// Change Connectors Now
            CellInfo CellParent = PieceToTransform.transform.parent.parent.parent.GetComponent<CellInfo>();

            if (PieceToTransform.RightSide)
            {
                CellParent.Rconnect.Lcolor = ColorForColorTransformPowerUp;
                pieceParent.Rcolor = ColorForColorTransformPowerUp;
            }

            if (PieceToTransform.LeftSide)
            {
                CellParent.Lconnect.Rcolor = ColorForColorTransformPowerUp;
                pieceParent.Lcolor = ColorForColorTransformPowerUp;
            }

            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                if (PieceToTransform.RightSide)
                {
                    CellParent.OuterRightConnect.ROutercolor = ColorForColorTransformPowerUp;
                    pieceParent.Rcolor = ColorForColorTransformPowerUp;
                }

                if (PieceToTransform.LeftSide)
                {
                    CellParent.OuterLeftConnect.LOutercolor = ColorForColorTransformPowerUp;
                    pieceParent.Lcolor = ColorForColorTransformPowerUp;
                }
            }

            if (GameManager.Instance.SuccesfullConnectionsMade == GameManager.Instance.ConnectionsNeededToFinishLevel)
            {
                UiManager.Instance.Commit.interactable = false;
            }

            if (CellParent.Rconnect.SuccesfullConnectionMade)
            {
                CellParent.Rconnect.SuccesfullConnectionMade = false;
                GameManager.Instance.SuccesfullConnectionsMade--;
            }

            if (CellParent.Lconnect.SuccesfullConnectionMade)
            {
                CellParent.Lconnect.SuccesfullConnectionMade = false;
                GameManager.Instance.SuccesfullConnectionsMade--;
            }

            if (CellParent.Rconnect.BadConnectionMade)
            {
                CellParent.Rconnect.BadConnectionMade = false;
            }

            if (CellParent.Lconnect.BadConnectionMade)
            {
                CellParent.Lconnect.BadConnectionMade = false;
            }

            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                if (CellParent.OuterRightConnect.SuccesfullConnectionMade)
                {
                    CellParent.OuterRightConnect.SuccesfullConnectionMade = false;
                    GameManager.Instance.SuccesfullConnectionsMade--;
                }

                if (CellParent.OuterLeftConnect.SuccesfullConnectionMade)
                {
                    CellParent.OuterLeftConnect.SuccesfullConnectionMade = false;
                    GameManager.Instance.SuccesfullConnectionsMade--;
                }

                if (CellParent.OuterRightConnect.BadConnectionMade)
                {
                    CellParent.OuterRightConnect.BadConnectionMade = false;
                }

                if (CellParent.OuterLeftConnect.BadConnectionMade)
                {
                    CellParent.OuterLeftConnect.BadConnectionMade = false;
                }

                CellParent.OuterRightConnect.CheckConnection();
                CellParent.OuterLeftConnect.CheckConnection();
            }

            CellParent.Rconnect.CheckConnection();
            CellParent.Lconnect.CheckConnection();


            if (GameManager.Instance.FullCellCounter == GameManager.Instance.CellsNeeedToFinish)
            {
                if (UiManager.Instance.Commit)
                {
                    UiManager.Instance.Commit.interactable = true;
                }
            }

            DoAfterSuccessfullPowerUp();
            //UsingPowerUp = false;
        }
    }

    public void SymbolTransform(ColorSymbolData PieceToTransform)
    {
        PieceMoveManager pieceParent = PieceToTransform.transform.parent.parent.GetComponent<PieceMoveManager>();

        if (pieceParent.PartOfBoard && !pieceParent.Locked && PieceToTransform.PieceSymbol != SymbolForShapeTransformPowerUp)
        {
            //Debug.Log("Used Symbol Transform Power Up");
            PieceToTransform.PieceSymbol = SymbolForShapeTransformPowerUp;

            PieceToTransform.ChooseColorAndSpritePowerUp(PieceToTransform.PieceColor, SymbolForShapeTransformPowerUp);

            ////// Change Connectors Now
            CellInfo CellParent = PieceToTransform.transform.parent.parent.parent.GetComponent<CellInfo>();

            if (PieceToTransform.RightSide)
            {
                CellParent.Rconnect.Lsymbol = SymbolForShapeTransformPowerUp;
                pieceParent.Rsymbol = SymbolForShapeTransformPowerUp;
            }

            if (PieceToTransform.LeftSide)
            {
                CellParent.Lconnect.Rsymbol = SymbolForShapeTransformPowerUp;
                pieceParent.Lsymbol = SymbolForShapeTransformPowerUp;
            }

            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                if (PieceToTransform.RightSide)
                {
                    CellParent.OuterRightConnect.ROutersymbol = SymbolForShapeTransformPowerUp;
                    pieceParent.Rsymbol = SymbolForShapeTransformPowerUp;
                }

                if (PieceToTransform.LeftSide)
                {
                    CellParent.OuterLeftConnect.LOutersymbol = SymbolForShapeTransformPowerUp;
                    pieceParent.Lsymbol = SymbolForShapeTransformPowerUp;
                }
            }


            if (GameManager.Instance.SuccesfullConnectionsMade == GameManager.Instance.ConnectionsNeededToFinishLevel)
            {
                UiManager.Instance.Commit.interactable = false;
            }

            if (CellParent.Rconnect.SuccesfullConnectionMade)
            {
                CellParent.Rconnect.SuccesfullConnectionMade = false;
                GameManager.Instance.SuccesfullConnectionsMade--;
            }

            if (CellParent.Lconnect.SuccesfullConnectionMade)
            {
                CellParent.Lconnect.SuccesfullConnectionMade = false;
                GameManager.Instance.SuccesfullConnectionsMade--;
            }

            if (CellParent.Rconnect.BadConnectionMade)
            {
                CellParent.Rconnect.BadConnectionMade = false;
            }

            if (CellParent.Lconnect.BadConnectionMade)
            {
                CellParent.Lconnect.BadConnectionMade = false;
            }

            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                if (CellParent.OuterRightConnect.SuccesfullConnectionMade)
                {
                    CellParent.OuterRightConnect.SuccesfullConnectionMade = false;
                    GameManager.Instance.SuccesfullConnectionsMade--;
                }

                if (CellParent.OuterLeftConnect.SuccesfullConnectionMade)
                {
                    CellParent.OuterLeftConnect.SuccesfullConnectionMade = false;
                    GameManager.Instance.SuccesfullConnectionsMade--;
                }

                if (CellParent.OuterRightConnect.BadConnectionMade)
                {
                    CellParent.OuterRightConnect.BadConnectionMade = false;
                }

                if (CellParent.OuterLeftConnect.BadConnectionMade)
                {
                    CellParent.OuterLeftConnect.BadConnectionMade = false;
                }

                CellParent.OuterRightConnect.CheckConnection();
                CellParent.OuterLeftConnect.CheckConnection();

            }

            CellParent.Rconnect.CheckConnection();
            CellParent.Lconnect.CheckConnection();


            if (GameManager.Instance.FullCellCounter == GameManager.Instance.CellsNeeedToFinish)
            {
                if (UiManager.Instance.Commit)
                {
                    UiManager.Instance.Commit.interactable = true;
                }
            }

            DoAfterSuccessfullPowerUp();
            //UsingPowerUp = false;
        }
    }

    public void UsePowerUp(GameObject ToUsePowerUp)
    {
        //Debug.Log(ToUsePowerUp.name);
        switch (PowerUpInUse)
        {
            case PowerUpChooseItemTypes.Joker:
                Joker(ToUsePowerUp.transform.parent.parent.GetComponent<PieceMoveManager>());
                break;
            case PowerUpChooseItemTypes.Switch:
                SwitchPieceSides(ToUsePowerUp.transform.parent.parent.GetComponent<PieceMoveManager>());
                break;
            case PowerUpChooseItemTypes.TileBomb:
                TileBomb(ToUsePowerUp.GetComponent<LimiterPiece>());
                break;
            case PowerUpChooseItemTypes.SliceBomb:
                SliceBomb(ToUsePowerUp.transform.parent.parent.GetComponent<PieceMoveManager>());
                break;
            case PowerUpChooseItemTypes.ColorTransform:
                ColorTransform(ToUsePowerUp.transform.GetComponent<ColorSymbolData>());
                break;
            case PowerUpChooseItemTypes.ShapeTransform:
                SymbolTransform(ToUsePowerUp.transform.GetComponent<ColorSymbolData>());
                break;
            default:
                break;
        }

        StartCoroutine(ResetTargetPowerUp());
    }
    public IEnumerator ResetTargetPowerUp()
    {
        yield return new WaitForEndOfFrame();
        HasTargetForPowerUp = false;
    }


    public void DoAfterSuccessfullPowerUp()
    {
        foreach (EquipmentSlot slot in GameManager.Instance.ThePlayer.SlotsForEquipment)
        {
            if (slot.TheItem != null)
            {
                if (slot.TheItem.HasTimeCooldown)
                {
                    if (slot.TheItem.PowerUpToGive == PowerUpInUse)
                    {
                       EquipmentManager.Instance.TimerTillNextPowerUpUse(PowerUpButton, slot);
                        //Debug.Log("Decreased The Times To Use In Match");
                    }
                }
                else
                {
                    if (slot.TheItem.PowerUpToGive == PowerUpInUse)
                    {
                        EquipmentManager.Instance.DecreaseNumberOfUsesInMatch(PowerUpButton, slot);
                        Debug.Log("Decreased The Times To Use In Match");
                    }
                }
            }
        }
    }
}
