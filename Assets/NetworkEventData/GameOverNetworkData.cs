using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class GameOverNetworkData : NetworkEventData
{
    public GameOverNetworkData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData)
    {
        ConvertEventData(enumNetworkEvent, objects, _photonEventData);
    }
    public Vector2Int movePos = new Vector2Int();
    public override void ConvertEventData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData)
    {
        this.enumNetworkEvent = enumNetworkEvent;
        photonEventData = _photonEventData;
        movePos.x = (int)objects[0];
        movePos.y = (int)objects[1];
    }
}
