using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    // Menu Buttons
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button disconnectBtn;

    // Menu Toggles
    [SerializeField] private GameObject joinMenu;
    [SerializeField] private GameObject inGameMenu;

    [SerializeField] private RelayHandler relayScript;

    [SerializeField] private TextMeshProUGUI joinCodeTextBox;

    private void Awake()
    {
        hostBtn.onClick.AddListener(BTN_StartHost);
        clientBtn.onClick.AddListener(BTN_StartClient);
        disconnectBtn.onClick.AddListener(Disconnect);
    }

    private void Start()
    {
        DisplayJoinMenu(true);
    }

    private void BTN_StartHost()
    {
       // relayScript.CreateRelay();

        NetworkManager.Singleton.StartHost();
        DisplayJoinMenu(false);
    }

    private void BTN_StartClient()
    {
        NetworkManager.Singleton.StartClient();
        DisplayJoinMenu(false);
    }
    private void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();

        DisplayJoinMenu(true);
    }

    private void DisplayJoinMenu(bool status)
    {
        joinMenu.SetActive(status);
        inGameMenu.SetActive(!status);
    }
}
