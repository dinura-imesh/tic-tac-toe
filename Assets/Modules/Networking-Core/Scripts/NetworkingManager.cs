using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using DefynModules.EventCore.Managers;
using DefynModules.EventCore.Definitions;

public class NetworkingManager : MonoBehaviour, IOnEventCallback, IConnectionCallbacks, IMatchmakingCallbacks , IInRoomCallbacks
{
    public static NetworkingManager instance;

    bool isGamePaused;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        PhotonNetwork.ConnectUsingSettings();
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnConnected()
    {
        NetworkStateDisplay.ShowNetworkState("Connected to the global server...");
    }

    public void OnConnectedToMaster()
    {
        NetworkStateDisplay.ShowNetworkState("Connected to the master server...");
        PhotonNetwork.LocalPlayer.NickName = PlayerPrefsSaveSystem.GetPlayerName();
        Debug.Log("Connected to the server");
    }
    public void OnCreatedRoom()
    {
        Toast.MakeText(EnumToast.Debug ,  "Lobby created");
        Toast.MakeText(EnumToast.Debug , "Waiting for the opponent to connect");
        Debug.Log("created a new room");
        GameManager.SetFirstTurn(true);
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Toast.MakeText(EnumToast.Debug, "Create room  failed" );
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        NetworkStateDisplay.ShowNetworkState("Disconnected from master server , Reconneting...");
        Debug.Log("Disconnected");
        EventManager.Raise(new GameResetEvent(E.Launch));
        PhotonNetwork.Reconnect();
        UIManager.FlushPanel();
        UIManager.RenderPanel(EnumUI.MainMenu);
    }
    
    public void OnEvent(EventData photonEvent)
    {
        EnumNetworkEvent enumEvent = (EnumNetworkEvent)photonEvent.Code;
        object[] objects = (object[])photonEvent.CustomData;
        switch (enumEvent)
        {
            case EnumNetworkEvent.GameStart:
                EventManager.Raise(new NetworkEvent(E.Launch,  enumEvent , new GameStartNetworkData(enumEvent, objects , photonEvent)));
                break;
            case EnumNetworkEvent.Move:
                EventManager.Raise(new NetworkEvent(E.Launch, enumEvent, new MoveNetworkData(enumEvent, objects, photonEvent)));
                break;
            case EnumNetworkEvent.GameOver:
                EventManager.Raise(new NetworkEvent(E.Launch, enumEvent, new GameOverNetworkData(enumEvent, objects, photonEvent)));
                break;
            case EnumNetworkEvent.Draw:
                EventManager.Raise(new NetworkEvent(E.Launch, enumEvent, new GameDrawNetworkData(enumEvent, objects, photonEvent)));
                break;
            case EnumNetworkEvent.Emoji:
                EventManager.Raise(new NetworkEvent(E.Launch, enumEvent, new EmojiSendNetworkData(enumEvent, objects, photonEvent)));
                break;
            case EnumNetworkEvent.Message:
                EventManager.Raise(new NetworkEvent(E.Launch, enumEvent, new MessageSendNetworkData(enumEvent, objects, photonEvent)));
                break;
        }
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        NetworkStateDisplay.ShowNetworkState("Joined lobby...");
        UIManager.FlushPanel();
        UIManager.FlushPanel();
        UIManager.RenderPanel(EnumUI.GamePanel);
        if (!PhotonNetwork.IsMasterClient)
            Toast.MakeText(EnumToast.Debug, "Joined lobby!");
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Toast.MakeText(EnumToast.Debug, "No lobbies found!");
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Toast.MakeText(EnumToast.Debug, message);
    }

    public void OnLeftRoom()
    {
        NetworkStateDisplay.ShowNetworkState("Disconnected from lobby...");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    public static void RaiseNetworkEvent(EnumNetworkEvent networkEvent, object[] data, bool recieveSelf  = false ,  byte channel = 0, bool encrypt = false)
    {
        RaiseEventOptions raiseEventOptions;
        SendOptions sendOptions = new SendOptions { Reliability = true };
        if (channel != 0)
            sendOptions.Channel = channel;
        if (encrypt)
            sendOptions.Encrypt = true;
        if (recieveSelf)
            raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        else
            raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        
        PhotonNetwork.RaiseEvent((byte)networkEvent, data, raiseEventOptions, sendOptions);
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Toast.MakeText(EnumToast.Debug , newPlayer.NickName + " Conneted!");
        Toast.MakeText(EnumToast.Debug, "Game Started!");
        Debug.Log("New player joined");
        GameManager.StartGame();
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Toast.MakeText(EnumToast.Debug, otherPlayer.NickName + " disconnected!");
        PhotonNetwork.CurrentRoom.IsVisible = false;
        LeanTween.delayedCall(2f , ()=>
        {
            Toast.MakeText(EnumToast.Debug, "Returning to main menu!");
            DisconnectFromLobby();
            EventManager.Raise(new GameResetEvent(E.Launch));
            UIManager.FlushPanel();
            UIManager.RenderPanel(EnumUI.MainMenu);
        });
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
    }

    public static void CreateNewRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = false;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log($"new room created room : {roomName}");
    }
    public static void JoinRoom(string roomName)
    {
        GameManager.SetFirstTurn(false);
        PhotonNetwork.JoinRoom(roomName);
        Debug.Log($"Join room called {roomName}");
    }
    public static void JoinRandomRoom()
    {
        GameManager.SetFirstTurn(false);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log($"Join Random room called");
    }
    public static void CreateRandomRoom()
    {
        string s = Random.Range(0, 5555).ToString();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(s , roomOptions);
        Debug.Log("Creare random room called");
    }

    public static void DisconnectFromLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

    public static void SetNickName(string name)
    {
        PhotonNetwork.LocalPlayer.NickName = name;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PhotonNetwork.Disconnect();
        }
    }
}
