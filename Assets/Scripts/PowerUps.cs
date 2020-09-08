using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUps : MonoBehaviour
{
    public GameObject Clip;

    public int TimesClickedDeal;

    public bool GotPowerUp = false;
    public bool LoseGame = false;

    public int SlotsInClip = 4;

    public void Init()
    {
        TimesClickedDeal = 0;
        Clip = GameObject.FindGameObjectWithTag("Clip").gameObject;
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
}
