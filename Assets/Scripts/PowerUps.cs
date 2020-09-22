﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpChooseItemTypes
{
    Joker,
    Switch,
    TileBomb,
    SliceBomb,
    ColorTransform,
    ShapeTransform
}

public class PowerUps : MonoBehaviour
{
    public GameObject Clip;

    public int TimesClickedDeal;

    public bool GotPowerUp = false;
    public bool LoseGame = false;

    public int SlotsInClip = 4;
    public bool UsingPowerUp;

    public PowerUpChooseItemTypes PowerUpType;

    public Colors ColorForColorTransformPowerUp;
    public Symbols SymbolForShapeTransformPowerUp;

    public bool HasTargetForPowerUp = false;

    public void Init()
    {
        TimesClickedDeal = 0;
        Clip = GameObject.FindGameObjectWithTag("Clip").gameObject;
    }

    public void UsingPowerUpToggle(int TypeOfPowerUp)
    {
        UsingPowerUp = true;

        switch (TypeOfPowerUp)
        {
            case 0:
                PowerUpType = PowerUpChooseItemTypes.Joker;
                break;
            case 1:
                PowerUpType = PowerUpChooseItemTypes.Switch;
                break;
            case 2:
                PowerUpType = PowerUpChooseItemTypes.TileBomb;
                break;
            case 3:
                PowerUpType = PowerUpChooseItemTypes.SliceBomb;
                break;
            case 4:
                PowerUpType = PowerUpChooseItemTypes.ColorTransform;
                break;
            case 5:
                PowerUpType = PowerUpChooseItemTypes.ShapeTransform;
                break;
            default:
                break;
        }
    }

    public void ChooseColorForColorTransformPowerUp(int Color)
    {
        switch (Color)
        {
            case 0:
                ColorForColorTransformPowerUp = Colors.Red;
                break;
            case 1:
                ColorForColorTransformPowerUp = Colors.Pink;
                break;
            case 2:
                ColorForColorTransformPowerUp = Colors.Blue;
                break;
            case 3:
                ColorForColorTransformPowerUp = Colors.Yellow;
                break;
            default:
                break;
        }
    }

    public void ChooseSymbolForShapeTransformPowerUp(int Shape)
    {
        switch (Shape)
        {
            case 0:
                SymbolForShapeTransformPowerUp = Symbols.Circle;
                break;
            case 1:
                SymbolForShapeTransformPowerUp = Symbols.Plus;
                break;
            case 2:
                SymbolForShapeTransformPowerUp = Symbols.Triangle;
                break;
            case 3:
                SymbolForShapeTransformPowerUp = Symbols.Square;
                break;
            default:
                break;
        }
    }

    public void DealCards() //// Put on Deal Button in scene
    {
        if (!LoseGame)
        {
            if (!GotPowerUp)
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

                GotPowerUp = false;
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
            if (TimesClickedDeal == 0 || GotPowerUp)
            {
                UiManager.Instance.SlotsFullMessage();
                return;
            }

            for (int i = 0; i < SlotsInClip; i++)
            {
                Clip.transform.GetChild(i).gameObject.SetActive(true);
            }

            GotPowerUp = true;
            ResuffleParts();
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

        if (!GotPowerUp)
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
        UsingPowerUp = false;
    }

    public void SliceBomb(PieceMoveManager PieceToBomb)
    {
        if (PieceToBomb.PartOfBoard)
        {
            CellInfo CellParent = PieceToBomb.transform.parent.GetComponent<CellInfo>();

            CellParent.Full = false;

            CellParent.Rconnect.Lsymbol = Symbols.None;
            CellParent.Rconnect.Lcolor = Colors.None;


            CellParent.Lconnect.Rsymbol = Symbols.None;
            CellParent.Lconnect.Rcolor = Colors.None;


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

            GameManager.Instance.FullCellCounter--;
            Destroy(PieceToBomb.gameObject);
            UsingPowerUp = false;
        }
    }

    public void SwitchPieceSides(PieceMoveManager PieceToSwtich)
    {
        if (PieceToSwtich.PartOfBoard && !PieceToSwtich.Locked)
        {
            Colors TempColor = PieceToSwtich.LeftSidePiece.transform.GetChild(0).GetComponent<ColorSymbolData>().PieceColor;
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

            CellInfo PieceParent =  PieceToSwtich.transform.parent.GetComponent<CellInfo>();

            TempColor = PieceParent.Rconnect.Lcolor;
            TempSymbol = PieceParent.Rconnect.Lsymbol;

            PieceParent.Rconnect.Lcolor = PieceParent.Lconnect.Rcolor;
            PieceParent.Rconnect.Lsymbol = PieceParent.Lconnect.Rsymbol;

            PieceParent.Lconnect.Rcolor = TempColor;
            PieceParent.Lconnect.Rsymbol = TempSymbol;

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

            //PieceParent.Rconnect.CheckConnection();
            //PieceParent.Lconnect.CheckConnection();


            if (GameManager.Instance.FullCellCounter == GameManager.Instance.CellsNeeedToFinish)
            {
                if (UiManager.Instance.Commit)
                {
                    UiManager.Instance.Commit.interactable = true;
                }
            }

            UsingPowerUp = false;
        }
    }

    public void Joker(PieceMoveManager PieceToJoker)
    {
        if (PieceToJoker.PartOfBoard && !PieceToJoker.Locked)
        {
            PieceToJoker.Rcolor = Colors.Joker;
            PieceToJoker.Rsymbol = Symbols.Joker;

            PieceToJoker.Lcolor = Colors.Joker;
            PieceToJoker.Lsymbol = Symbols.Joker;

            ////// Change Connectors Now
            CellInfo PieceParent = PieceToJoker.transform.parent.GetComponent<CellInfo>();

            PieceParent.Rconnect.Lcolor = Colors.Joker;
            PieceParent.Rconnect.Lsymbol = Symbols.Joker;

            PieceParent.Lconnect.Rcolor = Colors.Joker;
            PieceParent.Lconnect.Rsymbol = Symbols.Joker;

            //if (!PieceParent.Rconnect.SuccesfullConnectionMade)
            //{
            //    PieceParent.Rconnect.CheckConnection();
            //}

            //if (PieceParent.Lconnect.SuccesfullConnectionMade)
            //{
            //    PieceParent.Lconnect.CheckConnection();
            //}

            if (GameManager.Instance.FullCellCounter == GameManager.Instance.CellsNeeedToFinish)
            {
                if (UiManager.Instance.Commit)
                {
                    UiManager.Instance.Commit.interactable = true;
                }
            }

            UsingPowerUp = false;
        }
    }

    public void ColorTransform(ColorSymbolData PieceToTransform)
    {
        PieceMoveManager pieceParent = PieceToTransform.transform.parent.parent.GetComponent<PieceMoveManager>();

        if (pieceParent.PartOfBoard && !pieceParent.Locked)
        {
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

            //CellParent.Rconnect.CheckConnection();
            //CellParent.Lconnect.CheckConnection();


            if (GameManager.Instance.FullCellCounter == GameManager.Instance.CellsNeeedToFinish)
            {
                if (UiManager.Instance.Commit)
                {
                    UiManager.Instance.Commit.interactable = true;
                }
            }

            UsingPowerUp = false;
        }
    }

    public void SymbolTransform(ColorSymbolData PieceToTransform)
    {
        PieceMoveManager pieceParent = PieceToTransform.transform.parent.parent.GetComponent<PieceMoveManager>();

        if (pieceParent.PartOfBoard && !pieceParent.Locked)
        {
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

            //CellParent.Rconnect.CheckConnection();
            //CellParent.Lconnect.CheckConnection();


            if (GameManager.Instance.FullCellCounter == GameManager.Instance.CellsNeeedToFinish)
            {
                if (UiManager.Instance.Commit)
                {
                    UiManager.Instance.Commit.interactable = true;
                }
            }

            UsingPowerUp = false;
        }
    }

    public void UsePowerUp(GameObject ToUsePowerUp)
    {
        //Debug.Log(ToUsePowerUp.name);
        switch (PowerUpType)
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
}
