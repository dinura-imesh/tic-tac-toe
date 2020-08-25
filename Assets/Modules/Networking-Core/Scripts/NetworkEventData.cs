using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkEventData
{
    public EnumNetworkEvent enumNetworkEvent;

    public EventData photonEventData;

    public object[] objects;

    public abstract void ConvertEventData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData);
}
