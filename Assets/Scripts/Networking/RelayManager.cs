using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System.Threading.Tasks;
using System;
using Unity.VisualScripting;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance { get; private set; }
    [SerializeField] public List<Color> availableColorList;

    private void Awake()
    {
        Instance = this;
    }

    private async void RelayAuthenticate()
    {
        try
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed In: " + AuthenticationService.Instance.PlayerId);
            };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

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

    public void ChangePlayerColor(byte colorId)
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            Debug.Log("Local Client Object Not Found!");
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent(out PlayerVisual playerVisual))
        {
            Debug.Log("PlayerVisual Component not on Player Object");
            return;
        }

        playerVisual.SetPlayerColorServerRpc(colorId);
        Debug.Log("Set Player Color to Color: " + availableColorList[colorId] + " with ID: " + colorId);
    }

    public PlayerData GetPlayerData()
    {
        return default;
    }

    public Color GetPlayerColor(int idx)
    {
        return availableColorList[idx];
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < NetworkManager.Singleton.ConnectedClients.Count;
    }
}
