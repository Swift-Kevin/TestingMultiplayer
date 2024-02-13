using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ConnectionManager script_LobbyManagement;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        //Application.targetFrameRate = 45; // causes issues with camera rotation?
        QualitySettings.vSyncCount = 1;
        Screen.fullScreen = true;
    }

    public ConnectionManager GetLobbyManagementScript()
    {
        return script_LobbyManagement;
    }

    public void PlaySinglePlayer()
    {
        Debug.Log("not implemented unfortunately");
    }

    public void DisconnectPlayer()
    {
        if (!NetworkManager.Singleton.ShutdownInProgress)
        {
            ConnectionManager.Instance.LeaveLobby();
        }   NetworkManager.Singleton.Shutdown();
    }

    public void DisconnectPlayer(ulong playerId)
    {
        NetworkManager.Singleton.DisconnectClient(playerId);
    }

    public void PauseGame()
    {
        UIManagerScript.Instance.TogglePauseMenu();
    }

    public void ReturnToMainMenu()
    {
        DisconnectPlayer(); // disconnect from the relay
        MouseUnlockShow(); // show cursor and unlock it from center
        UIManagerScript.Instance.SetIsInGame(false
            ); // disable turning on pause menu
        UIManagerScript.Instance.DisplayMainMenu(); // turn on main menu
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MouseUnlockShow()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void MouseLockHide()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
