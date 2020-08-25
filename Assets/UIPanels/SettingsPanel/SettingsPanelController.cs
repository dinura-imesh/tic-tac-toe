using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : UIPanelBehaviour
{

    public ToggleSlider musicToggle;
    public ToggleSlider sfxToggle;
    public InputField nickNameInputField; 

    public override void OnDisableUIPanel()
    {
    }

    public override void OnEnableUIPanel()
    {
        musicToggle.IsOn = PlayerPrefsSaveSystem.GetIfMusicOn();
        sfxToggle.IsOn = PlayerPrefsSaveSystem.GetIfSFXOn();
        nickNameInputField.text = PlayerPrefsSaveSystem.GetPlayerName();
    }

    public override void SubscribeEvents()
    {
    }

    public override void UnsubscribeEvents()
    {
    }

    public void ButtonCloseSettingsPanel()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        UIManager.FlushPanel();
    }
    public void ToggleOnSFXSlider()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        if (sfxToggle.IsOn)
        {
            PlayerPrefsSaveSystem.SetSFX(true);
            AudioManager.playSFX = true;
        }
        else
        {
            PlayerPrefsSaveSystem.SetSFX(false);
            AudioManager.playSFX = false;
        }
    }
    public void ToggleOnMusicSlider()
    {
        if (musicToggle.IsOn)
        {
            PlayerPrefsSaveSystem.SetMusic(true);
            AudioManager.playMusic = true;
            AudioManager.FadeIn(EnumAudioId.MainTheme, 0.7f, 2.5f);
        }
        else
        {
            PlayerPrefsSaveSystem.SetMusic(false);
            AudioManager.playMusic = false;
            AudioManager.StopAllMusic();
        }
        AudioManager.PlaySFX(EnumAudioId.Button);
    }

    public void ButtonUpdateName()
    {
        if(nickNameInputField.text != "" && nickNameInputField.text != null)
        {
            PlayerPrefsSaveSystem.SetPlayerName(nickNameInputField.text);
        }
        else
        {
            Toast.MakeText(EnumToast.Debug, "Enter your name!");
        }
        AudioManager.PlaySFX(EnumAudioId.Button);
    }
}
