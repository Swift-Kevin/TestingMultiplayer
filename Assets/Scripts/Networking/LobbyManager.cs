using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    private const string KEY_PLAYER_NAME = "PlayerName";
    private const string KEY_GAME_MODE = "GameMode";
    private const string KEY_JOIN_CODE = "JoinCode";
    
    private const int maxPlayers = 4;
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private string playerName;
    private bool isAuthenticated = false;

    [SerializeField] private float heartbeatTimer;
    [SerializeField] private float heartbeatTimerMax = 15f;



    private void Awake()
    {
        Instance = this;
    }

    public async void Authenticate(string _playerName)
    {
        if (!isAuthenticated)
        {
            try
            {
                playerName = _playerName;

                InitializationOptions options = new InitializationOptions();
                options.SetProfile(playerName);

                await UnityServices.InitializeAsync(options);

                AuthenticationService.Instance.SignedIn += () =>
                {
                    Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
                };

                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                isAuthenticated = true;
                UIManagerScript.Instance.EnableMultiplayerButtons();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0)
            {
                heartbeatTimer = heartbeatTimerMax;

                try
                {
                    await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }
        }
    }

    public async Task<string> CreateLobby(string lobbyName, bool isPrivate)
    {
        try
        {
            string joinCode = await RelayManager.Instance.CreateRelay();

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, "Capture the Flag") },
                    { KEY_JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
                }
            };

            hostLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            joinedLobby = hostLobby;

            Debug.Log("Created Lobby: " + hostLobby.Name + " " + hostLobby.MaxPlayers);

            return joinCode;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async Task QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
            await RelayManager.Instance.JoinRelay(lobby.Data[KEY_JOIN_CODE].Value);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject> {
                        { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                    }
        };
    }

    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                   {KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

}
