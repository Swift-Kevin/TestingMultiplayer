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

    /// <summary>
    /// The struct to contain exceptions and storing them for later use.
    /// </summary>
    private struct ExceptionInfo
    {
        public string ExceptionText { get; set; }
        public string ExceptionStackTrace { get; set; }
        public LogType ExceptionType { get; set; }
        public int ExceptionCounter { get; set; }
        public ConsoleLogEntry ConsoleLogScript { get; set; }
    }

    // String = Exception Text -> So we can find how many times (the value) its been generated
    // This way we don't spawn infinite errors
    private Dictionary<string, ExceptionInfo> exceptionsGathered = new Dictionary<string, ExceptionInfo>();

    [Space, Header("Console Info")]
    [Tooltip("The console to view")]
    [SerializeField] private ScrollView consoleView;
    [Tooltip("The Content box")]
    [SerializeField] private GameObject content;
    [Tooltip("The Console Entry Prefab to spawn in for each error")]
    [SerializeField] private GameObject textEntryPrefab;
    [Tooltip("The Object to toggle the console without turning off the console")]
    [SerializeField] private GameObject toggleObj;
    [Tooltip("How far the console should be from the player when it's summoned")]
    [SerializeField] private float offsetFromPlayer;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        textEntryPrefab.SetActive(false);
    }

    /// <summary>
    /// Subscribe the logging function to the log callback
    /// </summary>
    public void BootUpConsole()
    {
        Application.logMessageReceived += LogCaughtException;
    }

    /// <summary>
    /// Log the caught exception
    /// </summary>
    /// <param name="logText">The logs message</param>
    /// <param name="stackTrace">The stack of where it came from</param>
    /// <param name="logType">The type of log (ex. warning, log, exception, error)</param>
    private void LogCaughtException(string logText, string stackTrace, LogType logType)
    {
        if (exceptionsGathered.ContainsKey(logText))
        {
            // Already contains key
            UpdateCountForEntryInConsole(exceptionsGathered[logText]);
        }
        else
        {
            // Doesnt Contain key so we add a new one
            AddNewEntryToConsole(logText, stackTrace, logType);
        }
    }

    /// <summary>
    /// Updates the given entry with the new count of times that log has appeared.
    /// </summary>
    /// <param name="_info"></param>
    private void UpdateCountForEntryInConsole(ExceptionInfo _info)
    {
        ExceptionInfo x = _info;
        x.ExceptionCounter++;

        // Update the entry
        exceptionsGathered[_info.ExceptionText] = x;

        _info.ConsoleLogScript.UpdateCount(_info.ExceptionCounter);
    }

    /// <summary>
    /// Adds a new entry into the console.
    /// </summary>
    /// <param name="_info">The info to use when spawning the object</param>
    private void AddNewEntryToConsole(string logText, string stackTrace, LogType logType)
    {
        GameObject newEntry = Instantiate(textEntryPrefab, content.transform);
        newEntry.SetActive(true);

        ConsoleLogEntry logEntry = newEntry.GetComponent<ConsoleLogEntry>();
        logEntry.SetExceptionText(logText);
        logEntry.UpdateCount(1);
        
        ExceptionInfo info = new ExceptionInfo();
        info.ExceptionText = logText;
        info.ExceptionCounter = 1;
        info.ExceptionStackTrace = stackTrace;
        info.ExceptionType = logType;
        info.ConsoleLogScript = logEntry;

        exceptionsGathered.Add(logText, info);
    }

    /// <summary>
    /// Clears the console's objects (except the first template object)
    /// </summary>
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

    /// <summary>
    /// Stop receiving logs from the console.
    /// </summary>
    public void ShutdownConsole()
    {
        Application.logMessageReceived -= LogCaughtException;
    }

    /// <summary>
    /// Summons the Console to the player. 
    /// </summary>
    /// <param name="pos">The position of the player.</param>
    /// <param name="fwd">The forward of the player.</param>
    /// <param name="rot">The rotation of the player.</param>
    public void SummonConsole(Vector3 pos, Vector3 fwd, Quaternion rot)
    {
        Vector3 nPos = pos + fwd * offsetFromPlayer;
        if ((gameObject.transform.rotation != rot) && (gameObject.transform.position != nPos))
        {
            Debug.Log("Summoned Console to new position.");
            gameObject.transform.rotation = rot;
            gameObject.transform.position = nPos;
        }
    }

    /// <summary>
    /// Toggle the given console on and off.
    /// </summary>
    public void ToggleConsole()
    {
        toggleObj.SetActive(!toggleObj.activeSelf);
    }
}
