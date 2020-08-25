using DefynModules.EventCore.Definitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartEvent : BaseEvent
{
    public bool isFirstTurn;
    public GameStartEvent(E ev , bool _isFirstTurn) : base(ev)
    {
        isFirstTurn = _isFirstTurn;
    }
       
}
