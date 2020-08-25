using DefynModules.EventCore.Managers;
using DefynModules.EventCore.Monobehaviors;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessengerController : EventHandledMonoBehavior
{
    public Sprite[] emojis;
    public InputField messageInputField;

    public Text messageShowText;
    public Image emojiShowPopUp;
    public Image countDownImage;

    bool isMessageReady = true;
    public float messageCoolDown = 6f;

    public static MessengerController instance;

    public GameObject panel;

    private void Awake()
    {
        instance = this;
    }

    public static void ShowMessengerPanel()
    {
        instance.panel.SetActive(true);
    }

    public static void HideMessengerPanel()
    {
        instance.panel.SetActive(false);

    }


    void ShowEmoji(int id)
    {
        AudioManager.PlaySFX(EnumAudioId.MessageRecieve);
        emojiShowPopUp.gameObject.SetActive(true);
        emojiShowPopUp.sprite = emojis[id];
        LeanTween.alpha(emojiShowPopUp.rectTransform, 1, 0.6f).setOnComplete(() => {
            LeanTween.delayedCall(0.8f, () => {
            LeanTween.alpha(emojiShowPopUp.rectTransform, 0, 0.6f);
            });
        });
        LeanTween.scale(emojiShowPopUp.rectTransform, Vector3.one, 2f).setOnComplete(() => {
            emojiShowPopUp.rectTransform.localScale = Vector3.zero;
            emojiShowPopUp.gameObject.SetActive(false);
        });
    }
    
    void ShowMessage(string playerName , string message)
    {
        AudioManager.PlaySFX(EnumAudioId.MessageRecieve);
        messageShowText.gameObject.SetActive(true);
        messageShowText.text = playerName + " : " + message;
        LeanTween.alphaText(messageShowText.rectTransform, 1, 0.5f).setOnComplete(() => {
            LeanTween.delayedCall(2f, () => {
                LeanTween.alphaText(messageShowText.rectTransform, 0, 0.5f);
             });
        });
        LeanTween.delayedCall(3f, () => {
        messageShowText.gameObject.SetActive(true);
        });
    }

    public void ButtonSendMessage()
    {
        if (isMessageReady)
        {
            if (messageInputField.text != "") {
                string message = messageInputField.text;
                object[] data = { PhotonNetwork.LocalPlayer.NickName , message };
                NetworkingManager.RaiseNetworkEvent(EnumNetworkEvent.Message, data);
                StartCoolDown();
            }
            else
            {
                Toast.MakeText(EnumToast.Debug, "Enter a message!");
            }
        }
    }

    public void ButtonSendEmoji(int id)
    {
        if (isMessageReady)
        {
            StartCoolDown();

            object[] data = { id };
            NetworkingManager.RaiseNetworkEvent(EnumNetworkEvent.Emoji, data);
        }
    }

    public void StartCoolDown()
    {
        isMessageReady = false;
        countDownImage.fillAmount = 0;
        LeanTween.value(0, 1, messageCoolDown).setOnUpdate(SetOnUpdate).setOnComplete(()=> {
            isMessageReady = true;
        });
    }
    void SetOnUpdate(float v)
    {
        countDownImage.fillAmount = v;
    }

    public override void SubscribeEvents()
    {
        EventManager.AddListener<NetworkEvent>(HandleOnNetworkEvent);
    }

    private void HandleOnNetworkEvent(NetworkEvent e)
    {
        if (e.launch)
        {
            if(e.enumNetworkEvent == EnumNetworkEvent.Emoji)
            {
                EmojiSendNetworkData emojiSendNetworkData = e.networkEventData as EmojiSendNetworkData;
                ShowEmoji(emojiSendNetworkData.emojiId);
            }
            if(e.enumNetworkEvent == EnumNetworkEvent.Message)
            {
                MessageSendNetworkData messageSendNetworkData = e.networkEventData as MessageSendNetworkData;
                ShowMessage(messageSendNetworkData.sender, messageSendNetworkData.message);
            }
        }
    }

    public override void UnsubscribeEvents()
    {
        EventManager.RemoveListener<NetworkEvent>(HandleOnNetworkEvent);
    }
}
