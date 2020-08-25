using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ToggleSlider : MonoBehaviour
{
    public RectTransform toggleImageRect;
    public Image background;
    public Color highlightColor;

    [Header("On toggle action")]
    public UnityEvent onToggleAction;

    private bool isOn;
    public bool IsOn {
        get { return isOn; }
        set {
                if (value == isOn)
                    return;
                isOn = value;
                OnvalueChanged();
            }
    }


    public void TriggerToggle()
    {
        if (!IsOn)
        {
            toggleImageRect.anchorMin = new Vector2(0.5f, 0.5f);
            toggleImageRect.anchorMax = new Vector2(0.5f, 0.5f);
            background.color = highlightColor;
        }
        else
        {
            toggleImageRect.anchorMin = new Vector2(0f, 0.5f);
            toggleImageRect.anchorMax = new Vector2(0f, 0.5f);
            background.color = Color.white;
        }
        IsOn = !IsOn;

        if (onToggleAction != null)
            onToggleAction.Invoke();
    }
    
    private void OnvalueChanged()
    {
        if (IsOn)
        {
            toggleImageRect.anchorMin = new Vector2(0.5f, 0.5f);
            toggleImageRect.anchorMax = new Vector2(0.5f, 0.5f);
            background.color = highlightColor;
        }
        else
        {
            toggleImageRect.anchorMin = new Vector2(0f, 0.5f);
            toggleImageRect.anchorMax = new Vector2(0f, 0.5f);
            background.color = Color.white;
        }
    }
}
