using System.Collections;
using System.Collections.Generic;
using DefynModules.EventCore.Monobehaviors;
using UnityEngine;

public abstract class UIPanelBehaviour : EventHandledMonoBehavior
{
    [HideInInspector]
    public EnumUIState EnumUiState = EnumUIState.NotLoaded;

    [HideInInspector]
    public GameObject panel;

    [HideInInspector]
    public CanvasGroup canvasGroup;

    public abstract void OnEnableUIPanel();
    public abstract void OnDisableUIPanel();

    public virtual void OnBackButtonPressed()
    {
        UIManager.FlushPanel();
    }

    protected override void OnEnable()
    {
        SubscribeEvents();
        OnEnableUIPanel();
    }

    protected override void OnDisable()
    {
        UnsubscribeEvents();
        OnDisableUIPanel();
    }
}
