using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Colors
{
    Red,
    Pink,
    Blue,
    Yellow,
    Joker,
    None
}
public enum Symbols
{
    Circle,
    Plus,
    Triangle,
    Square,
    Joker,
    None
}

public class ColorSymbolData : MonoBehaviour
{
    public SpriteRenderer ThisRenderer;
    public bool RightSide;
    public bool LeftSide;

    public Colors PieceColor;

    public Color[] TheColors;

    public Symbols PieceSymbol;

    public void ChooseColorAndSprtie(int Symbol)
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();

        ThisRenderer = GetComponent<SpriteRenderer>();

        if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
        {
            int Ran = Random.Range(0, (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].SpecificColors.Length));
            PieceColor = GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].SpecificColors[Ran];
        }
        else
        {
            PieceColor = (Colors)Random.Range(0, 4);
        }

        if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
        {
            int Ran = Random.Range(0, (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].SpecificSymbols.Length));
            PieceSymbol = GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].SpecificSymbols[Ran];
        }
        else
        {
            PieceSymbol = (Symbols)Symbol;
        }


        switch (PieceColor)
        {
            case Colors.Red:
                props.SetColor("_Color", TheColors[0]);
                break;
            case Colors.Pink:
                props.SetColor("_Color", TheColors[3]);
                break;
            case Colors.Blue:
                props.SetColor("_Color", TheColors[1]);
                break;
            case Colors.Yellow:
                props.SetColor("_Color", TheColors[2]);
                break;
            default:
                break;
        }

        if (RightSide)
        {
            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                switch (PieceSymbol)
                {
                    case Symbols.Circle:
                        props.SetTexture("_MainTex", GameManager.Instance.RightShapesSprites[0].texture);
                        ThisRenderer.sprite = GameManager.Instance.RightShapesSprites[0];
                        break;
                    case Symbols.Plus:
                        props.SetTexture("_MainTex", GameManager.Instance.RightShapesSprites[1].texture);
                        ThisRenderer.sprite = GameManager.Instance.RightShapesSprites[1];
                        break;
                    case Symbols.Triangle:
                        props.SetTexture("_MainTex", GameManager.Instance.RightShapesSprites[2].texture);
                        ThisRenderer.sprite = GameManager.Instance.RightShapesSprites[2];
                        break;
                    case Symbols.Square:
                        props.SetTexture("_MainTex", GameManager.Instance.RightShapesSprites[3].texture);
                        ThisRenderer.sprite = GameManager.Instance.RightShapesSprites[3];
                        break;
                    case Symbols.None:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                props.SetTexture("_MainTex", GameManager.Instance.RightShapesSprites[(int)PieceSymbol].texture);
                ThisRenderer.sprite = GameManager.Instance.RightShapesSprites[(int)PieceSymbol];
            }
        }

        if (LeftSide)
        {
            if (GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].DoubleRing)
            {
                switch (PieceSymbol)
                {
                    case Symbols.Circle:
                        props.SetTexture("_MainTex", GameManager.Instance.LeftShapesSprites[0].texture);
                        ThisRenderer.sprite = GameManager.Instance.LeftShapesSprites[0];
                        break;
                    case Symbols.Plus:
                        props.SetTexture("_MainTex", GameManager.Instance.LeftShapesSprites[1].texture);
                        ThisRenderer.sprite = GameManager.Instance.LeftShapesSprites[1];
                        break;
                    case Symbols.Triangle:
                        props.SetTexture("_MainTex", GameManager.Instance.LeftShapesSprites[2].texture);
                        ThisRenderer.sprite = GameManager.Instance.LeftShapesSprites[2];
                        break;
                    case Symbols.Square:
                        props.SetTexture("_MainTex", GameManager.Instance.LeftShapesSprites[3].texture);
                        ThisRenderer.sprite = GameManager.Instance.LeftShapesSprites[3];
                        break;
                    case Symbols.None:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                props.SetTexture("_MainTex", GameManager.Instance.LeftShapesSprites[(int)PieceSymbol].texture);
                ThisRenderer.sprite = GameManager.Instance.LeftShapesSprites[(int)PieceSymbol];
            }
        }
        ThisRenderer.SetPropertyBlock(props);
    }


    public void ChooseColorAndSpritePowerUp(Colors pieceColor, Symbols pieceSymbol)
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();

        ThisRenderer = GetComponent<SpriteRenderer>();

        switch (pieceColor)
        {
            case Colors.Red:
                props.SetColor("_Color", TheColors[0]);
                break;
            case Colors.Pink:
                props.SetColor("_Color", TheColors[3]);
                break;
            case Colors.Blue:
                props.SetColor("_Color", TheColors[1]);
                break;
            case Colors.Yellow:
                props.SetColor("_Color", TheColors[2]);
                break;
            default:
                break;
        }

        if (RightSide)
        {
            switch (pieceSymbol)
            {
                case Symbols.Circle:
                    props.SetTexture("_MainTex", GameManager.Instance.RightShapesSprites[0].texture);
                    ThisRenderer.sprite = GameManager.Instance.RightShapesSprites[0];
                    break;
                case Symbols.Plus:
                    props.SetTexture("_MainTex", GameManager.Instance.RightShapesSprites[1].texture);
                    ThisRenderer.sprite = GameManager.Instance.RightShapesSprites[1];
                    break;
                case Symbols.Triangle:
                    props.SetTexture("_MainTex", GameManager.Instance.RightShapesSprites[2].texture);
                    ThisRenderer.sprite = GameManager.Instance.RightShapesSprites[2];
                    break;
                case Symbols.Square:
                    props.SetTexture("_MainTex", GameManager.Instance.RightShapesSprites[3].texture);
                    ThisRenderer.sprite = GameManager.Instance.RightShapesSprites[3];
                    break;
                case Symbols.None:
                    break;
                default:
                    break;
            }
        }

        if (LeftSide)
        {
            switch (PieceSymbol)
            {
                case Symbols.Circle:
                    props.SetTexture("_MainTex", GameManager.Instance.LeftShapesSprites[0].texture);
                    ThisRenderer.sprite = GameManager.Instance.LeftShapesSprites[0];
                    break;
                case Symbols.Plus:
                    props.SetTexture("_MainTex", GameManager.Instance.LeftShapesSprites[1].texture);
                    ThisRenderer.sprite = GameManager.Instance.LeftShapesSprites[1];
                    break;
                case Symbols.Triangle:
                    props.SetTexture("_MainTex", GameManager.Instance.LeftShapesSprites[2].texture);
                    ThisRenderer.sprite = GameManager.Instance.LeftShapesSprites[2];
                    break;
                case Symbols.Square:
                    props.SetTexture("_MainTex", GameManager.Instance.LeftShapesSprites[3].texture);
                    ThisRenderer.sprite = GameManager.Instance.LeftShapesSprites[3];
                    break;
                case Symbols.None:
                    break;
                default:
                    break;
            }
        }
        ThisRenderer.SetPropertyBlock(props);
    }
}
