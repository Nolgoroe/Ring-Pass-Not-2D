using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int Gold, Rubies, MagicalItems, MaxLevelReached;


    private void Start()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            Gold = PlayerPrefs.GetInt("Gold");
        }

        if (PlayerPrefs.HasKey("Rubies"))
        {
            Rubies = PlayerPrefs.GetInt("Rubies");
        }

        if (PlayerPrefs.HasKey("MagicalItems"))
        {
            MagicalItems = PlayerPrefs.GetInt("MagicalItems");
        }

        if (PlayerPrefs.HasKey("MaxLevelReached"))
        {
            MaxLevelReached = PlayerPrefs.GetInt("MaxLevelReached");
        }
    }

    public void SaveDate()
    {
        PlayerPrefs.SetInt("Gold", Gold);
        PlayerPrefs.SetInt("Rubies", Rubies);
        PlayerPrefs.SetInt("MagicalItems", MagicalItems);
        PlayerPrefs.SetInt("MaxLevelReached", MaxLevelReached);
    }


    [ContextMenu("Reset Player Prefs")]
    public void ResetPlayerPref()
    {
        PlayerPrefs.DeleteAll();
    }
}
