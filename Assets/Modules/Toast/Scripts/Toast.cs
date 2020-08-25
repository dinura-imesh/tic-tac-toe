using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Debug = UnityEngine.Debug;

public class Toast : MonoBehaviour
{
    public static Toast instance;

    [Header("Turn this off after initialization")]
    public bool debug;

    public Image backgroundImage;

    public TextMeshProUGUI toastText;

    public Image notificationItem;

    public Toaster[] toasters;

    private float duration;

    public RectTransform basicStyleText;

    public Text basicStyleTextText;

    public RectTransform basicStyleImage;

    private bool isToasting;

    private bool isNativeToasting;


    Queue<Toaster> toastQueue = new Queue<Toaster>();

    Queue<string> nativeQueue = new Queue<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (debug)
        {
            MakeText(EnumToast.Debug, "test 0");
            MakeText(EnumToast.Debug, "test 1");
            MakeText(EnumToast.Debug, "test 2");
        }
    }

    #region NATIVETOAST

    public static void MakeNativeToast(string _text)
    {
        if (instance.isNativeToasting)
        {
            instance.nativeQueue.Enqueue(_text);
        }
        else
        {
            instance.isNativeToasting = true;
            instance.basicStyleTextText.text = _text;
            LeanTween.alpha(instance.basicStyleText, 1f, 0.8f).setIgnoreTimeScale(true).setOnComplete(() =>
            {
                LeanTween.delayedCall(0.8f, () => { LeanTween.alpha(instance.basicStyleText, 0, 0.8f); }).setIgnoreTimeScale(true);
            }).setIgnoreTimeScale(true);
            LeanTween.alpha(instance.basicStyleImage, 0.75f, 0.8f).setIgnoreTimeScale(true).setOnComplete(() =>
            {
                LeanTween.delayedCall(0.8f, () => { LeanTween.alpha(instance.basicStyleImage, 0f, 0.8f).setIgnoreTimeScale(true); });
            }).setIgnoreTimeScale(true);;
            LeanTween.delayedCall(2.6f, () =>
            {
                instance.isNativeToasting = false;
                instance.ContinueNativeQueue();
            }).setIgnoreTimeScale(true);;
        }
    }


    void ContinueNativeQueue()
    {
        if (nativeQueue.Count > 0)
        {
            string s = nativeQueue.Dequeue();
            MakeNativeToast(s);
        }
    }

    #endregion

    public static void MakeText(EnumToast toaster, string _text = null, float _duration = 1.5f, Sprite icon = null)
    {
        if (instance.isToasting)
        {
            Toaster t = new Toaster();
            t.enumToast = toaster;
            t.duration = _duration;
            t.notificationIcon = icon;
            t.text = _text;
            instance.toastQueue.Enqueue(t);
        }
        else
        {
            instance.isToasting = true;

            Toaster s = Array.Find(instance.toasters, Toaster => Toaster.enumToast == toaster);
            if (s != null)
            {
                if (_text != null)
                {
                    instance.toastText.text = _text;
                }
                else
                {
                    instance.toastText.text = s.text;
                }

                if (icon != null)
                    instance.notificationItem.sprite = icon;
                else
                    instance.notificationItem.sprite = s.notificationIcon;

                instance.duration = s.duration;
                instance.backgroundImage.color = s.backGroundColor;

                LeanTween.moveY(instance.backgroundImage.rectTransform, -120f, 0.6f).setEase(LeanTweenType.easeOutSine)
                    .setOnComplete(() =>
                    {
                        LeanTween.delayedCall(instance.duration,
                            () =>
                            {
                                LeanTween.moveY(instance.backgroundImage.rectTransform, 80f, 0.6f)
                                    .setEase(LeanTweenType.easeInSine);
                            });
                    });

                LeanTween.delayedCall(1.3f + instance.duration, () => { instance.isToasting = false; });
            }
            else
            {
                Debug.LogError("The Toaster " + toaster + " not found");
            }
        }

        LeanTween.delayedCall(1.3f + instance.duration, () =>
        {
            if (instance.toastQueue.Count > 0)
            {
                Toaster t = instance.toastQueue.Dequeue();
                Toast.MakeText(t.enumToast, t.text, t.duration, t.notificationIcon);
            }
        });
    }
}