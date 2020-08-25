using ExitGames.Client.Photon;
using UnityEngine;

public class MoveNetworkData : NetworkEventData
{
    public MoveNetworkData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData)
    {
        ConvertEventData(enumNetworkEvent, objects, _photonEventData);
    }
    public Vector2Int movePos = new Vector2Int();
    public override void ConvertEventData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData)
    {
        this.enumNetworkEvent = enumNetworkEvent;
        this.objects = objects;
        photonEventData = _photonEventData;
        movePos.x = (int)objects[0];
        movePos.y = (int)objects[1];
    }
}
