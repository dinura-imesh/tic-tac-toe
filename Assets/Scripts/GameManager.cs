using DefynModules.EventCore.Managers;
using DefynModules.EventCore.Monobehaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : EventHandledMonoBehavior
{
    private bool isLocalPlayerTurn;

   // bool isGamePlaying = true;

    bool isFirstTurnSet;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;

        DontDestroyOnLoad(gameObject);
    }

    public static bool GetIfLocalPlayerTurn()
    {
        return instance.isLocalPlayerTurn;
    }

    public static void SetFirstTurn(bool _isLocalPlayer)
    {
        if (!instance.isFirstTurnSet)
        {
            Debug.Log("First Turn Set");
            instance.isLocalPlayerTurn = _isLocalPlayer;
            instance.isFirstTurnSet = true;
        }
    }

    public static void StartGame()
    {
        object[] array = null;
        NetworkingManager.RaiseNetworkEvent(EnumNetworkEvent.GameStart, array, true);
    }

    public override void SubscribeEvents()
    {
        EventManager.AddListener<NetworkEvent>(HandleOnNetworkEvent);
        EventManager.AddListener<GameResetEvent>(HandleOnGameResetEvent);
    }

    private void HandleOnGameResetEvent(GameResetEvent e)
    {
        if (e.launch)
            isLocalPlayerTurn = false;
    }

    private void HandleOnNetworkEvent(NetworkEvent e)
    {
        if(e.enumNetworkEvent == EnumNetworkEvent.GameStart)
        {
            //isGamePlaying = true;
        }
    }

    public override void UnsubscribeEvents()
    {
    }
}
