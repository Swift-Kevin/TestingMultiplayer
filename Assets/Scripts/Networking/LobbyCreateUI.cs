using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    public static LobbyCreateUI Instance {  get; private set; }
    
    [SerializeField] Button createLobby;
    [SerializeField] TMP_InputField lobbyName;
    [SerializeField] Toggle lobbyAccess;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        createLobby.onClick.AddListener(CreateLobby);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void CreateLobby()
    {
        if (lobbyName.text != string.Empty) 
        {
            LobbyManager.Instance.CreateLobby(lobbyName.text, lobbyAccess.isOn);
        }
        else
        {
            Debug.Log("Name needed");
        }
    }
}
