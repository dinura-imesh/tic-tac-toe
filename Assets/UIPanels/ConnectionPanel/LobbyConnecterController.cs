using DefynModules.EventCore.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//ff00c6  00d8ff 2f2f2f

public class LobbyConnecterController : UIPanelBehaviour
{
    public InputField createLobbyInputField; 
    public InputField joinLobbyInputField;
    public RectTransform joinRandomButtonRect, createRandomButtonRect;


    private void Start()
    {
        LeanTween.scale(joinRandomButtonRect, new Vector3(1.08f, 1.08f, 1), 0.9f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
        LeanTween.scale(createRandomButtonRect, new Vector3(1.08f, 1.08f, 1), 0.9f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }
    public void ButtonCreateRoom()
    {
            AudioManager.PlaySFX(EnumAudioId.Button);
            if (createLobbyInputField.text != "")
            {
                NetworkingManager.CreateNewRoom(createLobbyInputField.text);
            }
            else
            {
                Toast.MakeText(EnumToast.Debug, "Enter a valid lobby name");
            }
    }
    public void ButtonJoinRoom()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        if (joinLobbyInputField.text != "")
        {
            NetworkingManager.JoinRoom(joinLobbyInputField.text);
        }
        else
        {
            Toast.MakeText(EnumToast.Debug, "Enter a valid lobby name");
        }
    }
    public void ButtonJoinRandomRoom()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        NetworkingManager.JoinRandomRoom();
    }
    public void ButtonCreateRandomRoom()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        NetworkingManager.CreateRandomRoom();
    }

    public override void OnEnableUIPanel()
    {
    }

    public override void OnDisableUIPanel()
    {
    }

    public override void SubscribeEvents()
    {
    }

    public override void UnsubscribeEvents()
    {
    }

    public void ButtonCloseConnectionPanel()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        UIManager.FlushPanel();
    }
}
