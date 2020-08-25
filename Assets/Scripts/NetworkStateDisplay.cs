using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkStateDisplay : MonoBehaviour
{
    public static NetworkStateDisplay instance;

    public Text stateText;

    private void Awake()
    {
        instance = this;
    }

    public static void ShowNetworkState(string state)
    {
        instance.stateText.text = state;
    }
}
