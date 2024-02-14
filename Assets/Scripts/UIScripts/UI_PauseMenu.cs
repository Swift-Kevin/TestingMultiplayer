using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PauseMenu : MonoBehaviour
{
    [SerializeField] Button button_Continue;
    [SerializeField] Button button_Exit;


    void Start()
    {
        button_Continue.onClick.AddListener(ContinueButtonFunctionality);
        button_Exit.onClick.AddListener(ExitButtonFunctionality);
    }

    private void ContinueButtonFunctionality()
    {
        GameManager.Instance.PauseGame();
    }

    private void ExitButtonFunctionality()
    {
        InGameConsole.Instance.ClearInGameConsole();
        GameManager.Instance.ReturnToMainMenu();
    }
}
