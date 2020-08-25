using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : UIPanelBehaviour
{
    public RectTransform playImage;
    private const string playstoreURL = "https://play.google.com/store/apps/details?id=me.technoob.tictactoe";
    private bool isBackButtonPressed;

    private void Start()
    {
        LeanTween.scale(playImage, new Vector2(1.1f, 1.1f), 0.9f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }
    public void ButtonLoadConnectionPanel()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        UIManager.RenderPanel(EnumUI.ConnectionPanel);
    }

    public override void OnEnableUIPanel()
    {
        LeanTween.delayedCall(0.05f, () =>
        {
            AudioManager.FadeIn(EnumAudioId.MainTheme, 0.7f, 2.5f);
        });
        LeanTween.delayedCall(0.3f, () =>
        {
            int randomX = Random.Range(0, 2);
            if (randomX == 1)
                AdManager.ShowVideoAd();
        });
    }

    public override void OnDisableUIPanel()
    {
        if (AudioManager.instance != null)
            AudioManager.StopMusic(EnumAudioId.MainTheme);
            //AudioManager.FadeOut(EnumAudioId.MainTheme , 2.5f);
    }

    public override void SubscribeEvents()
    {
    }

    public override void UnsubscribeEvents()
    {
    }

    public void ButtonOpenSettingsPanel()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        UIManager.RenderPanel(EnumUI.Settings);
    }
    public void ButtonShare()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        new NativeShare().SetTitle("Wanna play TicTacToe multiplayer?")
            .SetText($"Wanna play TicTacToe multiplayer? Download and install |{playstoreURL}|")
            .Share();
    }
    public void ButtonRate()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        Application.OpenURL(playstoreURL);

    }
    public void OpenCreditsPanel()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        UIManager.RenderPanel(EnumUI.Credits);
    }

    public override void OnBackButtonPressed()
    {
        if (isBackButtonPressed)
        {
            Application.Quit();
        }
        else
        {
            isBackButtonPressed = true;
            LeanTween.delayedCall(1.5f, () => { isBackButtonPressed = false; });
            Toast.MakeText(EnumToast.Debug, "Press again to quit!");
        }
    }
}
