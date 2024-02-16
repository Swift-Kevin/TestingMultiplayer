using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_PauseMenu : MonoBehaviour
{
    [SerializeField] Button button_Continue;
    [SerializeField] Button button_Exit;
    [SerializeField] Toggle toggle_toggleConsole;


    void Start()
    {
        button_Continue.onClick.AddListener(ContinueButtonFunctionality);
        button_Exit.onClick.AddListener(ExitButtonFunctionality);
        toggle_toggleConsole.onValueChanged.AddListener(ToggleConsole);
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

    private void ToggleConsole(bool val)
    {
        InGameConsole.Instance.ToggleConsole();
    }
}
