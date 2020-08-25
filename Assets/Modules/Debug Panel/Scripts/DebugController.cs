using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{

    public GameObject debugPanel;

    private bool isPanelOpen;

    public Text bugReportText;

    public Text logText;

   private void Awake()
    {
        Application.logMessageReceived += LogCallBack;
        if (debugPanel.activeSelf)
        {
            debugPanel.SetActive(false);
        }
    }

    public void ButtonOpenCloseDebugPanel()
    {
        if (!isPanelOpen)
        {
            debugPanel.SetActive(true);
        }
        else
        {
            debugPanel.SetActive(false);
        }
        isPanelOpen = !isPanelOpen;
        
    }


    void LogCallBack(string condition , string stackTrace , LogType logType)
    {
        if (logType == LogType.Exception || logType == LogType.Error || logType == LogType.Warning)
        {
            if (logType == LogType.Exception)
            {
                debugPanel.SetActive(true);
            }
            isPanelOpen = true;
            bugReportText.text = condition + " " + stackTrace;
            CopyToClipBoard(bugReportText.text);
        }
        if(logType == LogType.Log)
        {
            logText.text += "\n" + condition;
        }
    }

    public void ButtonCopyLog()
    {
        CopyToClipBoard(logText.text);
    }

    void CopyToClipBoard(string s)
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = s;
        textEditor.SelectAll();
        textEditor.Copy();
    }

}
