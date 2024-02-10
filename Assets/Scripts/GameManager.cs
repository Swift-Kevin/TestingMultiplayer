using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIStartAndTransition script_NetworkingButtons;
    [SerializeField] LobbyManager script_LobbyManagement;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public UIStartAndTransition GetScriptNetworkingButtons()
    {
        return script_NetworkingButtons;
    }

    public LobbyManager GetLobbyManagementScript()
    {
        return script_LobbyManagement;
    }

    public void PlaySinglePlayer()
    {
        Debug.Log("not implemented unfortunately");
    }
}
