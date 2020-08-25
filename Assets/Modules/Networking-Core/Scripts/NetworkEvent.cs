using DefynModules.EventCore.Definitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkEvent : BaseEvent
{
    public EnumNetworkEvent enumNetworkEvent;
    public NetworkEventData networkEventData;

    public NetworkEvent(E ev , EnumNetworkEvent _enumNetworkEvent , NetworkEventData _networkEventData ) : base(ev)
    {
        enumNetworkEvent = _enumNetworkEvent;
        networkEventData = _networkEventData;
    }
}
