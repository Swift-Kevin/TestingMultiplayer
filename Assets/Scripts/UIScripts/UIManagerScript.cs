using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject authMenu;
    [SerializeField] private Button authStart;
    [SerializeField] private TMP_InputField authName;


    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(false);
        authMenu.SetActive(true);
        authStart.onClick.AddListener(AuthStart);
    }

    private void AuthStart()
    {
        if (authName.text != string.Empty)
        {
            mainMenu.SetActive(true);
            authMenu.SetActive(false);
            LobbyManager.Instance.Authenticate(authName.text);
        }
        else
        {
            Debug.Log("no name");
        }
    }
}
