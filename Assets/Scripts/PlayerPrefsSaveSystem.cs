using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSaveSystem : MonoBehaviour
{
    public static PlayerPrefsSaveSystem instance;

    private const string P_nickName = "P_NickName"; 
    private const string P_SFX = "P_SFX"; 
    private const string P_Music = "P_Music"; 
    private const string P_FirstTime = "P_FirstTime"; 

    private void Awake()
    {
        instance = this;
        if(PlayerPrefs.GetInt(P_FirstTime) != 1)
        {
            PlayerPrefs.SetInt(P_SFX, 1);
            PlayerPrefs.SetInt(P_Music, 1);
            PlayerPrefs.SetInt(P_FirstTime, 1);
            AudioManager.playMusic = true;
            AudioManager.playSFX = true;
            LeanTween.delayedCall(2f, () => 
            {
                UIManager.RenderPanel(EnumUI.NickNamePanel);
            });
        }
        else
        {
            int i = PlayerPrefs.GetInt(P_Music);
            if (i == 1)
                AudioManager.playMusic = true;
            else
                AudioManager.playMusic = false;

            i = PlayerPrefs.GetInt(P_SFX);
            if (i == 1)
                AudioManager.playSFX = true;
            else
                AudioManager.playSFX = false;
        }
    }

    public static string GetPlayerName()
    {
        string name = PlayerPrefs.GetString(P_nickName);
        if (name != null || name != "")
            return name;
        else
            return null;
    }
    public static bool GetIfSFXOn()
    {
        bool isSFXOn = true;
        int i = PlayerPrefs.GetInt(P_SFX);
        if (i == 0)
            isSFXOn = false;
        return isSFXOn;
    }
    public static bool GetIfMusicOn()
    {
        bool isMusicOn = true;
        int i = PlayerPrefs.GetInt(P_Music);
        if (i == 0)
            isMusicOn = false;
        return isMusicOn;
    }

    public static void SetPlayerName(string name)
    {
        Toast.MakeText(EnumToast.Debug, "Nickname changed!");
        PlayerPrefs.SetString(P_nickName, name);
        NetworkingManager.SetNickName(name);
    }
    public static void SetSFX(bool isOn)
    {
        if (isOn)
            PlayerPrefs.SetInt(P_SFX, 1);
        else
            PlayerPrefs.SetInt(P_SFX, 0);
    }
    public static void SetMusic(bool isOn)
    {
        if (isOn)
            PlayerPrefs.SetInt(P_Music, 1);
        else
            PlayerPrefs.SetInt(P_Music, 0);
    }


}
