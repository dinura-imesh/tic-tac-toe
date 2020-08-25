using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class GameStartNetworkData : NetworkEventData
{
    public GameStartNetworkData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData)
    {
        ConvertEventData(enumNetworkEvent , objects , _photonEventData);
    }
    public override void ConvertEventData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData)
    {
        this.enumNetworkEvent = enumNetworkEvent;
        this.objects = objects;
        this.photonEventData = _photonEventData;
    }
}
