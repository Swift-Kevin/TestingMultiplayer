using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_MutliplayerMenu : MonoBehaviour
{
    [SerializeField] Button button_QuickHost; 
    [SerializeField] Button button_QuickJoin;
    [SerializeField] Button button_HostLAN; 
    [SerializeField] Button button_JoinLAN;
    [SerializeField] Button button_Back; 

    private void Start()
    {
        // Quick Lobby
        button_QuickHost.onClick.AddListener(QuickHostButtonFunctionality);
        button_QuickJoin.onClick.AddListener(QuickJoinButtonFunctionality);
        
        // LAN Buttons
        button_HostLAN.onClick.AddListener(LANHostButtonFunctionality);
        button_JoinLAN.onClick.AddListener(LANJoinButtonFunctionality);

        button_Back.onClick.AddListener(BackButtonFunctionality);
    }

    private async void LANHostButtonFunctionality()
    {
        // Start ingame console
        InGameConsole.Instance.BootUpConsole();

        await ConnectionManager.Instance.LANHostLobby();

        InGameHideUI();
    }

    private async void LANJoinButtonFunctionality()
    {
        // Start ingame console
        InGameConsole.Instance.BootUpConsole();

        await ConnectionManager.Instance.LANJoinLobby();
    
        InGameHideUI();
    }

    private async void QuickHostButtonFunctionality()
    {
        // Start ingame console
        InGameConsole.Instance.BootUpConsole();
        
        await ConnectionManager.Instance.CreateLobby("MyLobby", false);

        InGameHideUI();
    }

    private async void QuickJoinButtonFunctionality()
    {
        // Start ingame console
        InGameConsole.Instance.BootUpConsole();

        await ConnectionManager.Instance.QuickJoinLobby();
        InGameHideUI();
    }

    private void InGameHideUI()
    {
        UIManagerScript.Instance.HideAllMenus();
        UIManagerScript.Instance.SetIsInGame(true);
    }

    private void BackButtonFunctionality()
    {
        UIManagerScript.Instance.DisplayMainMenu();
    }

    public void DisableButtons()
    {
        button_QuickHost.interactable = false;
        button_QuickJoin.interactable = false;
        button_HostLAN.interactable = false;
        button_JoinLAN.interactable = false;
    }

    public void EnableButtons()
    {
        button_QuickHost.interactable = true;
        button_QuickJoin.interactable = true;
        button_HostLAN.interactable = true;
        button_JoinLAN.interactable = true;
    }
}
