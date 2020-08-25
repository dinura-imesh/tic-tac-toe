using DefynModules.EventCore.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
    public EnumSelection selection;
    public BoxCollider2D selfCollder;
    public SpriteRenderer spriteRenderer;
    public Vector2Int id;
    public void SetState(EnumSelection _state , Sprite icon)
    {
        selection = _state;
        TurnOffCollider();
        Debug.Log("Called");
        spriteRenderer.sprite = icon;
        EventManager.Raise(new WinnerCheckEvent(DefynModules.EventCore.Definitions.E.Launch));
    }

    public void SetEnemyState(EnumSelection _state, Sprite icon)
    {
        selection = _state;
        TurnOffCollider();
        spriteRenderer.sprite = icon;
    }

    public void SetSpriteOnly(EnumSelection _state , Sprite icon)
    {
        selection = _state;
        TurnOffCollider();
        spriteRenderer.sprite = icon;
    }

    public void TurnOffCollider()
    {
        selfCollder.enabled = false;
    }

    public void TurnOnCollider()
    {
        selfCollder.enabled = true;
    }
    public void Reset()
    {
        selection = EnumSelection.NotMarked;
        spriteRenderer.sprite = null;
        selfCollder.enabled = true;
    }
}
