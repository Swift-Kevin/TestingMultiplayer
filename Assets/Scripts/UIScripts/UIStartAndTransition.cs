using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStartAndTransition : MonoBehaviour 
{
    [Header("Parent Objects")]
    [SerializeField] private GameObject mainMenuParent;
    [SerializeField] private GameObject gameTitle;
    [SerializeField] private GameObject setupLobbyParent;
    [SerializeField] private GameObject joinLobbyParent;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button singleplayerButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private Button quitButton;

    [Header("Private Lobby Connect")]
    [SerializeField] TMP_InputField lobbyJoinPrivateCode;


    // Start is called before the first frame update
    void Start()
    {
        // Setup parents to false
        mainMenuParent.SetActive(true);
        gameTitle.SetActive(true);
        setupLobbyParent.SetActive(false);
        joinLobbyParent.SetActive(false);

        // Setup Event Listeners
        singleplayerButton.onClick.AddListener(PlaySinglePlayer);
        hostButton.onClick.AddListener(DisplayLobbyCreationMenu);
        joinLobbyButton.onClick.AddListener(DisplayLobbyJoinMenu);
        quitButton.onClick.AddListener(QuitGame);
    }
     
    #region DisplayMenus
    public void DisplayMainMenu()
    {
        mainMenuParent.SetActive(true);
        setupLobbyParent.SetActive(false);
        joinLobbyParent.SetActive(false);
        gameTitle.SetActive(true);
    }
    
    private void DisplayLobbyCreationMenu()
    {
        mainMenuParent.SetActive(false);
        setupLobbyParent.SetActive(true);
        joinLobbyParent.SetActive(false);
        gameTitle.SetActive(false);
    }
    
    private void DisplayLobbyJoinMenu()
    {
        mainMenuParent.SetActive(false);
        setupLobbyParent.SetActive(false);
        joinLobbyParent.SetActive(true);
        gameTitle.SetActive(false);
    }

    #endregion

    private void PlaySinglePlayer()
    {
        GameManager.Instance.PlaySinglePlayer();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
