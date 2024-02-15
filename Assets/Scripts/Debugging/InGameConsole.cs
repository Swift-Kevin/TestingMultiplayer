using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
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
        public int ExceptionCounter { get; set; }
        public TextMeshProUGUI ExceptionCountText { get; set; }
    }

    // String = Exception Text -> So we can find how many times (the value) its been generated
    // This way we don't spawn infinite errors
    private Dictionary<string, ExceptionInfo> exceptionsGathered = new Dictionary<string, ExceptionInfo>();

    [Space, Header("Console Info")]
    [SerializeField] private ScrollView consoleView;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject textEntryPrefab;
    [SerializeField] private float offset;

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
        if (exceptionsGathered.ContainsKey(logText))
        {
            // Already contains key
            UpdateCountForEntryInConsole(exceptionsGathered[logText]);
        }
        else
        {
            // Doesnt Contain key
            ExceptionInfo info = new ExceptionInfo();
            info.ExceptionText = logText;
            info.ExceptionCounter = 1;
            info.ExceptionStackTrace = stackTrace;
            info.ExceptionType = logType;

            AddNewEntryToConsole(info);
        }
    }

    private void UpdateCountForEntryInConsole(ExceptionInfo _info)
    {
        _info.ExceptionCounter += 1;
        _info.ExceptionCountText.text = _info.ExceptionCounter.ToString();
    }

    private void AddNewEntryToConsole(ExceptionInfo _info)
    {
        if (_info.ExceptionType != LogType.Log)
        {
            GameObject newEntry = Instantiate(textEntryPrefab, content.transform);
            newEntry.GetComponent<TextMeshProUGUI>().text = _info.ExceptionText;
            newEntry.SetActive(true);
            
            TextMeshProUGUI counterText = newEntry.GetComponentInChildren<TextMeshProUGUI>();
            counterText.text = _info.ExceptionCounter.ToString();
            
            _info.ExceptionCountText = counterText;
            exceptionsGathered.Add(_info.ExceptionText, _info);
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

    public void SummonConsole(Vector3 pos, Vector3 fwd, Quaternion rot)
    {
        Vector3 nPos = pos + fwd * offset;
        if ((gameObject.transform.rotation != rot) && (gameObject.transform.position != nPos))
        {
            Debug.Log("Summoned Console to new position.");
            gameObject.transform.rotation = rot;
            gameObject.transform.position = nPos;
        }
    }
}
