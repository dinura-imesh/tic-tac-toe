using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class MessageSendNetworkData : NetworkEventData
{
    public MessageSendNetworkData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData)
    {
        ConvertEventData(enumNetworkEvent, objects, _photonEventData);
    }

    public string sender;
    public string message;

    public override void ConvertEventData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData)
    {
        this.enumNetworkEvent = enumNetworkEvent;
        this.objects = objects;
        sender = (string)objects[0];
        message = (string)objects[1];
        photonEventData = _photonEventData;
    }
}
