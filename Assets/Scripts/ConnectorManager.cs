using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorManager : MonoBehaviour
{
    public bool HasLimiter;
    public bool SuccesfullConnectionMade;
    public bool BadConnectionMade;

    public LimiterType TypeOfLimiter;
    public Symbols Rsymbol = Symbols.None;
    public Colors Rcolor = Colors.None;

    public Symbols Lsymbol = Symbols.None;
    public Colors Lcolor = Colors.None;

    public LimiterCellManager ConnectorLimiter;

    private void Update()
    {
        if(ConnectorLimiter.TypeOfLimiter != LimiterType.None)
        {
            HasLimiter = true;
            TypeOfLimiter = ConnectorLimiter.TypeOfLimiter;
        }
    }

    public void CheckConnection()
    {
        if (HasLimiter)
        {
            switch (ConnectorLimiter.TypeOfLimiter)
            {
                case LimiterType.GeneralColor:
                    if (Rcolor == Lcolor && Rcolor != Colors.None && Lcolor != Colors.None)
                    {
                        //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                        Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
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
                    if (Rsymbol == Lsymbol && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                    {
                        //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                        Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
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
                    if (Rcolor == Colors.Red && Lcolor == Colors.Red && Rcolor != Colors.None && Lcolor != Colors.None)
                    {
                        //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                        Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
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
                    if (Rcolor == Colors.Blue && Lcolor == Colors.Blue && Rcolor != Colors.None && Lcolor != Colors.None)
                    {
                        //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                        Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
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
                    if (Rcolor == Colors.Yellow && Lcolor == Colors.Yellow && Rcolor != Colors.None && Lcolor != Colors.None)
                    {
                        //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                        Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
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
                    if (Rcolor == Colors.Pink && Lcolor == Colors.Pink && Rcolor != Colors.None && Lcolor != Colors.None)
                    {
                        //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color" + " " + transform.name);
                        Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
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
                    if (Rsymbol == Symbols.Square && Lsymbol == Symbols.Square && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                    {
                        //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                        Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
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
                    if (Rsymbol == Symbols.Triangle && Lsymbol == Symbols.Triangle && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                    {
                        //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                        Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
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
                    if (Rsymbol == Symbols.Plus && Lsymbol == Symbols.Plus && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                    {
                        //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                        Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
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
                    if (Rsymbol == Symbols.Circle && Lsymbol == Symbols.Circle && Rsymbol != Symbols.None && Lsymbol != Symbols.None)
                    {
                        //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Shape" + " " + transform.name);
                        Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
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
                    if (Rcolor != Colors.None && Rsymbol != Symbols.None && Lcolor != Colors.None && Lsymbol != Symbols.None)
                    {
                        BadConnectionMade = true;
                        SuccesfullConnectionMade = false;
                    }
                    break;
            }
        }
        else
        {
            if (Rcolor != Colors.None && Rsymbol != Symbols.None && Lcolor != Colors.None && Lsymbol != Symbols.None)
            {
                BadConnectionMade = true;
                SuccesfullConnectionMade = false;
            }

            if ((Rcolor == Lcolor || Rsymbol == Lsymbol) && Rcolor!= Colors.None && Lcolor != Colors.None && Rsymbol !=Symbols.None && Rsymbol != Symbols.None)
            {
                //Debug.Log("Limiter Type: " + ConnectorLimiter.TypeOfLimiter + " " + "Connection Made Color or Shape" + " " + transform.name);
                Destroy(Instantiate(GameManager.Instance.ConnectionVFX.gameObject, transform), 1.5f);
                SuccesfullConnectionMade = true;
                GameManager.Instance.SuccesfullConnectionsMade++;
                BadConnectionMade = false;
            }
        }
    }

    public void BadConnectionToggle()
    {
        if (Rcolor != Colors.None && Rsymbol != Symbols.None && Lcolor != Colors.None && Lsymbol != Symbols.None)
        {
            BadConnectionMade = true;
            SuccesfullConnectionMade = false;
        }
    }
}
