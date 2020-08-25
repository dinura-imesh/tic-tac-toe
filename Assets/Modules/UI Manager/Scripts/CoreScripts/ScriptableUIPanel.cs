using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIPanel" , menuName = "Scriptable_UIPanel")]
public class ScriptableUIPanel : ScriptableObject
{
    public EnumUI id;
    public EnumAnimation startAnimation;
    public EnumAnimation endAnimation;
    public UIPanelBehaviour uiPanelBehaviour;
    public string panelName;
    public GameObject panelPrefab;
    public bool LoadOnAwake;
}
