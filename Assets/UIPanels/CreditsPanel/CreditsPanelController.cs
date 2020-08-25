using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPanelController : UIPanelBehaviour
{
    public override void OnDisableUIPanel()
    {
    }

    public override void OnEnableUIPanel()
    {
    }

    public override void SubscribeEvents()
    {
    }

    public override void UnsubscribeEvents()
    {
    }
    public void ButtonCloseCreditsPanel()
    {
        AudioManager.PlaySFX(EnumAudioId.Button);
        UIManager.FlushPanel();
    }

    public void ButtonOpenSoundImage()
    {
        Application.OpenURL("https://soundimage.org/");
    }
    public void ButtonOpenAssetStore()
    {
        Application.OpenURL("https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116");
    }

    public void ButtonOpenGoogleFonts()
    {
        Application.OpenURL("https://fonts.google.com/");
    }
    public void ButtonOpenUnity()
    {
        Application.OpenURL("https://unity.com/");
    }
}
