using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LimiterType
{
    GeneralColor,
    GeneralSymbol,
    Red,
    Blue,
    Yellow,
    Pink,
    Square,
    Triangle,
    Plus,
    Circle,
    None
}

public class LimiterCellManager : MonoBehaviour
{
    public bool IsFull;

    public LimiterType TypeOfLimiter;
}
