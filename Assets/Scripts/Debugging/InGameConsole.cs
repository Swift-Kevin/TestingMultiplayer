using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameConsole : MonoBehaviour
{
    public static InGameConsole Instance;

    private struct ExceptionInfo
    {
        public string ExceptionText { get; set; }
        public string ExceptionStackTrace { get; set; }
        public LogType ExceptionType { get; set; }
    }

    private List<ExceptionInfo> exceptionInfos = new List<ExceptionInfo>();

    [SerializeField] private ScrollView scrollView;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject textEntryPrefab;

    public void BootUpConsole()
    {
        Application.logMessageReceived += LogCaughtException;
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        textEntryPrefab.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void LogCaughtException(string logText, string stackTrace, LogType logType)
    {
        ExceptionInfo info = new ExceptionInfo();
        info.ExceptionText = logText;
        //exceptionInfos.Add(info);
        AddToConsole(info);
    }

    private void AddToConsole(ExceptionInfo _info)
    {
        if (_info.ExceptionType != LogType.Log)
        {
            GameObject newEntry = Instantiate(textEntryPrefab, content.transform);
            newEntry.GetComponent<TextMeshProUGUI>().text = _info.ExceptionText;
            newEntry.SetActive(true);
        }
    }

    public void ClearInGameConsole()
    {


        List<GameObject> childrenInContent = new List<GameObject>();

        content.GetChildGameObjects(childrenInContent);

        foreach (GameObject child in childrenInContent)
        {
            if (child.activeSelf)
            {
                Destroy(child);
            }
        }

        ShutdownConsole();
    }

    public void ShutdownConsole()
    {
        Application.logMessageReceived -= LogCaughtException;
    }
}
