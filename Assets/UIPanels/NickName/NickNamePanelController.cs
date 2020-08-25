using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickNamePanelController : UIPanelBehaviour
{
    public InputField nicknameInputField;

    public override void OnDisableUIPanel()
    {
    }

    public override void OnEnableUIPanel()
    {
    }

    public override void SubscribeEvents()
    {
    }

    public override void UnsubscribeEvents()
    {
    }

    public void ButtonUpdateNickName()
    {
        if (nicknameInputField.text != "" && nicknameInputField.text != null)
        {
            PlayerPrefsSaveSystem.SetPlayerName(nicknameInputField.text);
            UIManager.FlushPanel();
        }
        else
        {
            Toast.MakeText(EnumToast.Debug, "Enter a valid name!");
        }
    }

    public override void OnBackButtonPressed()
    {
        Toast.MakeText(EnumToast.Debug, "Please enter your Nickname");
    }
}
