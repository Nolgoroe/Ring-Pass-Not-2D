using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInfo : MonoBehaviour
{
    public bool Full;

    public bool IsOuterRing;
    public ConnectorManager Rconnect, Lconnect, OuterRightConnect, OuterLeftConnect;
}
