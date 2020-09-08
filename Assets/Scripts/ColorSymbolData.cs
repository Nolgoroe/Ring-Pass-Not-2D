using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Colors
{
    Red,
    Pink,
    Blue,
    Yellow,
    None
}
public enum Symbols
{
    Circle,
    Plus,
    Triangle,
    Square,
    None
}

public class ColorSymbolData : MonoBehaviour
{
    public SpriteRenderer ThisRenderer;
    public bool RightSide;
    public bool LeftSide;

    public Colors PieceColor;


    public Symbols PieceSymbol;

    public void ChooseColorAndSprtie(int Symbol)
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();

        ThisRenderer = GetComponent<SpriteRenderer>();

        PieceColor = (Colors)Random.Range(0, 4);
        PieceSymbol = (Symbols)Symbol;

        switch (PieceColor)
        {
            case Colors.Red:
                props.SetColor("_Color", Color.red);
                break;
            case Colors.Pink:
                props.SetColor("_Color", Color.magenta);
                break;
            case Colors.Blue:
                props.SetColor("_Color", Color.blue);
                break;
            case Colors.Yellow:
                props.SetColor("_Color", Color.yellow);
                break;
            default:
                break;
        }

        if (RightSide)
        {
            props.SetTexture("_MainTex", GameManager.Instance.RightShapesSprites[(int)PieceSymbol].texture);
            ThisRenderer.sprite = GameManager.Instance.RightShapesSprites[(int)PieceSymbol];
        }

        if (LeftSide)
        {
            props.SetTexture("_MainTex", GameManager.Instance.LeftShapesSprites[(int)PieceSymbol].texture);
            ThisRenderer.sprite = GameManager.Instance.LeftShapesSprites[(int)PieceSymbol];
        }

        ThisRenderer.SetPropertyBlock(props);
    }
}
