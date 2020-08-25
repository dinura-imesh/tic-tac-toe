using DefynModules.EventCore.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelController : UIPanelBehaviour
{
    private bool isBackPressed;

    public void ButtonExitToMainMenu()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        Toast.MakeText(EnumToast.Debug, "Disconnected from lobby!");
        NetworkingManager.DisconnectFromLobby();
        EventManager.Raise(new GameResetEvent(DefynModules.EventCore.Definitions.E.Launch));
        UIManager.FlushPanel();
        UIManager.RenderPanel(EnumUI.MainMenu);
    }

    public override void OnBackButtonPressed()
    {
        if (isBackPressed)
        {
            ButtonExitToMainMenu();
        }
        else
        {
            isBackPressed = true;
            LeanTween.delayedCall(1.5f, () => { isBackPressed = false; });
            Toast.MakeText(EnumToast.Debug, "Press again to quit the match!");
        }
    }

    public override void OnDisableUIPanel()
    {
        if (AudioManager.instance != null)
            AudioManager.StopMusic(EnumAudioId.GameMusic);
           // AudioManager.FadeOut(EnumAudioId.GameMusic, 0.5f);
    }

    public override void OnEnableUIPanel()
    {
            AudioManager.FadeIn(EnumAudioId.GameMusic , 0.7f , 2.5f);
    }

    public override void SubscribeEvents()
    {
    }

    public override void UnsubscribeEvents()
    {
    }
}
