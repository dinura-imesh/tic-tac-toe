using System.Collections;
using System.Collections.Generic;
using DefynModules.EventCore.Definitions;
using ExitGames.Client.Photon;
using UnityEngine;

public class EmojiSendNetworkData : NetworkEventData
{
    public EmojiSendNetworkData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData)
    {
        ConvertEventData(enumNetworkEvent, objects, _photonEventData);
    }

    public int emojiId;

    public override void ConvertEventData(EnumNetworkEvent enumNetworkEvent, object[] objects, EventData _photonEventData)
    {
        this.enumNetworkEvent = enumNetworkEvent;
        this.objects = objects;
        emojiId = (int)objects[0];
        photonEventData = _photonEventData;
    }
}
