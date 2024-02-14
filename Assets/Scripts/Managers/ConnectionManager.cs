using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using System.Net;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance { get; private set; }

    private const string KEY_PLAYER_NAME = "PlayerName";
    private const string KEY_GAME_MODE = "GameMode";
    private const string KEY_JOIN_CODE = "JoinCode";
    private const string KEY_LAN_IP = "IP";
    private const string DEFAULT_LOBBY_NAME = "MyLobby";
    private const int maxPlayers = 4;

    private Lobby hostLobby;
    private Lobby joinedLobby;
    private string playerName;
    private bool isAuthenticated = false;

    [SerializeField] private float heartbeatTimer;
    [SerializeField] private float heartbeatTimerMax = 15f;
    [SerializeField] public List<Color> availableColorList;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<string> CreateRelay()
    {
        try
        {
            // Get Allocation and create join code
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);

            // Setup Host Relay Data
            RelayServerData relayData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayData);
            NetworkManager.Singleton.StartHost();
            return joinCode;

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async Task<string> CreateLobby(string lobbyName, bool isPrivate)
    {
        try
        {
            string joinCode = await CreateRelay();

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

    public async Task<int> JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Relay Join Code: " + joinCode);
            JoinAllocation join = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(join.RelayServer.IpV4, (ushort)join.RelayServer.Port);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                    join.RelayServer.IpV4,
                    (ushort)join.RelayServer.Port,
                    join.AllocationIdBytes,
                    join.Key,
                    join.ConnectionData,
                    join.HostConnectionData
                );

            NetworkManager.Singleton.StartClient();
            return 0;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
            return 1;
        }
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

    public async Task QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
            await JoinRelay(lobby.Data[KEY_JOIN_CODE].Value);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task LANHostLobby()
    {
        try
        {
            string lobbyName = "MyLobby";

            string hostName = Dns.GetHostName();

            hostName = Dns.GetHostName();
            IPHostEntry myIP = Dns.GetHostEntry(hostName);
            IPAddress[] address = myIP.AddressList;
            string ip = address[1].ToString();

            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {KEY_LAN_IP, new DataObject(DataObject.VisibilityOptions.Public, ip) },
                }
            };

            hostLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            Debug.Log("Created Lobby! " + hostLobby.Name + " " + hostLobby.MaxPlayers + " " + hostLobby.Id + " " + hostLobby.LobbyCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, (ushort)9000);
            NetworkManager.Singleton.StartHost();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    public async Task LANJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions
            {
                Player = GetPlayer()
            };

            var lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
            Debug.Log("Quickly Joined lobby");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(lobby.Data[KEY_LAN_IP].Value, (ushort)9000);
            NetworkManager.Singleton.StartClient();
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
