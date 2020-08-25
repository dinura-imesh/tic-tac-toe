using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ScriptableUIPanel[] panelsArray;

    public Transform containerTransform;

    public GameObject testArea;

    private static UIManager instance;

    private Dictionary<EnumUI, UIPanelBehaviour> loadedPanels = new Dictionary<EnumUI, UIPanelBehaviour>();

    public Stack<EnumUI> renderingPanelsStack = new Stack<EnumUI>();

    private CanvasGroup _animationStartCanvasGroup;
    private CanvasGroup _animationEndCanvasGroup;

    private void Awake()
    {
        #region SINGLETON

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        #endregion

        //Destroy the Test Area Gameobject
        Destroy(instance.testArea);
        InitUIManager();
    }


    public static void InitUIManager()
    {
        ScriptableUIPanel panel = Array.Find(instance.panelsArray, UIPanel => UIPanel.LoadOnAwake);
        RenderPanel(panel.id);
        Debug.Log("Init UI Manager");
    }


    /// <summary>
    /// Show the panel with the given id
    /// </summary>
    /// <param name="id">EnumUi id</param>
    /// <param name="timeIndependantRendering">set true to do rendering without animations</param>
    public static void RenderPanel(EnumUI id, bool timeIndependantRendering = false)
    {
        ScriptableUIPanel panel = Array.Find(instance.panelsArray, UIPanel => UIPanel.id == id);

        if (!instance.loadedPanels.ContainsKey(id))
        {
            GameObject spawnedPanel = Instantiate(panel.panelPrefab, instance.containerTransform);
            UIPanelBehaviour uiPanelBehaviour = panel.uiPanelBehaviour;
            uiPanelBehaviour.panel = spawnedPanel;
            uiPanelBehaviour.canvasGroup = spawnedPanel.gameObject.GetComponent<CanvasGroup>();
            instance._animationStartCanvasGroup = uiPanelBehaviour.canvasGroup;
            instance.loadedPanels.Add(panel.id, uiPanelBehaviour);
            instance.loadedPanels[id].EnumUiState = EnumUIState.Rendering;
            spawnedPanel.SetActive(true);
            spawnedPanel.transform.localScale = Vector3.one;
            spawnedPanel.transform.SetAsLastSibling();
            if (!timeIndependantRendering)
            {
                instance.AnimateStart(spawnedPanel, panel.startAnimation);
            }

            instance.renderingPanelsStack.Push(panel.id);
        }
        else if (instance.loadedPanels[id].EnumUiState == EnumUIState.Hidden)
        {
            GameObject loadedPanel = instance.loadedPanels[id].panel;
            instance._animationStartCanvasGroup = instance.loadedPanels[id].canvasGroup;
            instance._animationStartCanvasGroup.alpha = 1f;
            instance.loadedPanels[id].EnumUiState = EnumUIState.Rendering;
            loadedPanel.SetActive(true);
            loadedPanel.transform.localScale = Vector3.one;
            loadedPanel.transform.SetAsLastSibling();
            if (!timeIndependantRendering)
            {
                instance.AnimateStart(loadedPanel, panel.startAnimation);
            }

            instance.renderingPanelsStack.Push(panel.id);
        }
        else if (instance.loadedPanels[id].EnumUiState == EnumUIState.Rendering)
        {
            Debug.Log("The panel " + panel.panelName + " is already Rendering");
        }
    }

    /// <summary>
    /// Hide the panel on the Top
    /// </summary>
    public static void FlushPanel(bool timeIndependantRendering = false)
    {
        EnumUI e = instance.renderingPanelsStack.Pop();
        ScriptableUIPanel panel = Array.Find(instance.panelsArray, UIPanel => UIPanel.id == e);
        instance._animationEndCanvasGroup = panel.uiPanelBehaviour.canvasGroup;
        instance.loadedPanels[panel.id].EnumUiState = EnumUIState.Hidden;
        if (timeIndependantRendering)
        {
            instance.loadedPanels[panel.id].panel.SetActive(false);
        }
        else
        {
            instance.AnimateEnd(instance.loadedPanels[panel.id].panel, panel.endAnimation);
        }
    }

    void FlushOnBackButtonPressed()
    {
        EnumUI e = instance.renderingPanelsStack.Peek();
        ScriptableUIPanel scriptableUiPanel = Array.Find(instance.panelsArray, UIPanel => UIPanel.id == e);
        instance.loadedPanels[scriptableUiPanel.id].OnBackButtonPressed();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FlushOnBackButtonPressed();
        }
    }

    #region ANIMATIONS

    void AnimateStart(GameObject panel, EnumAnimation animation)
    {
        switch (animation)
        {
            case EnumAnimation.PopIn:
                _animationStartCanvasGroup.alpha = 0f;
                panel.transform.localScale = Vector3.zero;
                LeanTween.scale(panel.gameObject, Vector3.one, 0.25f).setIgnoreTimeScale(true);
                LeanTween.value(0f, 1f, 0.25f).setOnUpdate(ChangeAnimationStartAlpha).setIgnoreTimeScale(true);
                break;
            case EnumAnimation.PopOut:
                _animationStartCanvasGroup.alpha = 0f;
                panel.transform.localScale = new Vector2(2.5f, 2.5f);
                LeanTween.scale(panel.gameObject, Vector3.one, 0.25f).setIgnoreTimeScale(true);
                LeanTween.value(0f, 1f, 0.25f).setOnUpdate(ChangeAnimationStartAlpha).setIgnoreTimeScale(true);
                break;
            case EnumAnimation.SlideIn:
                panel.transform.localPosition = new Vector3(1080, 0, 0);
                LeanTween.moveLocalX(panel.gameObject, 0, 0.25f).setIgnoreTimeScale(true);
                break;
        }
    }

    void AnimateEnd(GameObject panel, EnumAnimation animation)
    {
        switch (animation)
        {
            case EnumAnimation.Default:
                panel.SetActive(false);

                break;
            case EnumAnimation.PopOut:
                LeanTween.scale(panel, new Vector2(2.5f, 2.5f), 0.25f).setOnComplete(() => { panel.SetActive(false); })
                    .setIgnoreTimeScale(true);
                LeanTween.value(1f, 0f, 0.25f).setOnUpdate(ChangeAnimationEndAlpha).setIgnoreTimeScale(true);
                break;
            case EnumAnimation.PopIn:
                LeanTween.scale(panel, Vector3.zero, 0.25f).setOnComplete(() => { panel.SetActive(false); })
                    .setIgnoreTimeScale(true);
                LeanTween.value(1f, 0f, 0.25f).setOnUpdate(ChangeAnimationEndAlpha).setIgnoreTimeScale(true);
                break;
            case EnumAnimation.SlideOut:
                LeanTween.moveLocalX(panel, -1080, 0.25f).setOnComplete(() => { panel.SetActive(false); })
                    .setIgnoreTimeScale(true);
                break;
        }
    }

    void ChangeAnimationStartAlpha(float a)
    {
        _animationStartCanvasGroup.alpha = a;
    }

    void ChangeAnimationEndAlpha(float a)
    {
        _animationEndCanvasGroup.alpha = a;
    }

    #endregion
}