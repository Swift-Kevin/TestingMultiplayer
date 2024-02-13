using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    public static UIManagerScript Instance;

    [SerializeField] private UI_MutliplayerMenu multiplayerMenuScript;

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject multiplayerMenu;
    [SerializeField] private GameObject pauseMenu;

    [Header("Buttons")]
    [SerializeField] private Button button_Singleplayer;
    [SerializeField] private Button button_Multiplayer;
    [SerializeField] private Button button_Quit;

    private bool isInGame = false;

    private bool isPauseOpened = false;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        button_Singleplayer.interactable = false;
        
        DisplayMainMenu();
        DisableMultiplayerButtons();
        AddListeners();
    }

    public void HideAllMenus()
    {
        mainMenu?.SetActive(false);
        multiplayerMenu?.SetActive(false);
        pauseMenu?.SetActive(false);
    }

    public void DisplayMainMenu()
    {
        HideAllMenus();
        mainMenu?.SetActive(true);
    }

    private void DisplayMultiplayerMenu()
    {
        HideAllMenus();
        multiplayerMenu?.SetActive(true);
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            HideAllMenus();
            GameManager.Instance.MouseLockHide();
            isPauseOpened = false;
        }
        else
        {
            DisplayPauseMenu();
            GameManager.Instance.MouseUnlockShow();
            isPauseOpened = true;
        }
    }

    private void DisplayPauseMenu()
    {
        HideAllMenus();
        pauseMenu?.SetActive(true);
    }

    private void AddListeners()
    {
        button_Multiplayer.onClick.AddListener(MultiplayerButtonFunctionality);
        button_Quit.onClick.AddListener(QuitButtonFunctionality);
    }

    // ====================================
    //          Button Functionality 
    // ====================================
    

    private void QuitButtonFunctionality()
    {
        GameManager.Instance.QuitGame();
    }

    private void MultiplayerButtonFunctionality()
    {
        DisplayMultiplayerMenu();
        AuthStart();
    }

    public void EnableMultiplayerButtons()
    {
        multiplayerMenuScript.EnableMultiplayerButtons();
    }
    
    public void DisableMultiplayerButtons()
    {
        multiplayerMenuScript.DisableMultiplayerButtons();
    }

    private void AuthStart()
    {
        ConnectionManager.Instance.Authenticate("Player0");
    }

    public void SetIsInGame(bool value)
    {
        isInGame = value;
    }

    public bool GetIsInGame()
    {
        return isInGame;
    }

    public bool GetPauseOpened()
    {
        return isPauseOpened;
    }
}
