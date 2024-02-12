using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_MutliplayerMenu : MonoBehaviour
{
    [SerializeField] Button button_Host; 
    [SerializeField] Button button_Join;
    [SerializeField] Button button_Back; 

    private void Start()
    {
        button_Host.onClick.AddListener(HostButtonFunctionality);
        button_Join.onClick.AddListener(JoinButtonFunctionality);
        button_Back.onClick.AddListener(BackButtonFunctionality);
    }

    private async void HostButtonFunctionality()
    {
        await LobbyManager.Instance.CreateLobby("MyLobby", false);
        
        UIManagerScript.Instance.HideAllMenus();
        UIManagerScript.Instance.SetIsInGame(true);
    }

    private async void JoinButtonFunctionality()
    {
        await LobbyManager.Instance.QuickJoinLobby();
        UIManagerScript.Instance.HideAllMenus();
        UIManagerScript.Instance.SetIsInGame(true);
    }


    private void BackButtonFunctionality()
    {
        UIManagerScript.Instance.DisplayMainMenu();
    }

    public void DisableMultiplayerButtons()
    {
        button_Host.interactable = false;
        button_Join.interactable = false;
    }

    public void EnableMultiplayerButtons()
    {
        button_Host.interactable = true;
        button_Join.interactable = true;
    }


}
