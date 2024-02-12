using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Services.Authentication;

public class ExampleNetworkingFunctions : NetworkBehaviour
{
    struct MyCustomData : INetworkSerializable // needed to read values
    {
        public int _int;
        public bool _bool;
        // public string message; // String is a reference type so it cannot be used
        // include Unity.Collections for FixedString
        // FixedString is a class, use the one with "Bytes" at the end, others are deprecated
        public FixedString128Bytes _message;

        // Interface's required function
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);  // serializes the referenced variable
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref _message);
        }
    }

    // must be used within a NetworkBehavior class
    // also must be initialized now
    // cannot use values that can be null, not Class
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, // default value 
        NetworkVariableReadPermission.Everyone, // everyone can read
        NetworkVariableWritePermission.Owner);  // only the owner can write to it

    private NetworkVariable<MyCustomData> customData = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 69,
            _bool = false // initialize data here
        }, // default value 
       NetworkVariableReadPermission.Everyone,
       NetworkVariableWritePermission.Owner);


    [SerializeField] private Transform spawnObjectPrefab;
    private Transform spawnedObjectTransform;
    private void TestVariables()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTransform = Instantiate(spawnObjectPrefab, gameObject.transform);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            // TestServerRpc();
            // randomNumber.Value = UnityEngine.Random.Range(0, 100);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(spawnedObjectTransform.gameObject);
        }
    }


    // This doesnt run on the client at all, only runs on the server
    // Useful for sending messages from client to server
    [ServerRpc] // must add "ServerRpc" to the end of the function with [ServerRpc] defined above it
    private void TestServerRpc()
    {
        Debug.Log("TestServerRpc: " + OwnerClientId); // NetworkBehavior is needed instead of MonoBehavior
    }

    // Useful for maybe changing scenes
    [ClientRpc]
    private void TestClientRpc()
    {
        Debug.Log("TestClientRpc: "); // meant to be called on the server to be sent to clients
        // like Broadcasting but to clients only
    }








    //private async void ListLobbies()
    //{
    //    try
    //    {
    //        QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
    //        {
    //            Count = 25,
    //            Filters = new List<QueryFilter>()
    //            {
    //                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
    //            },
    //            Order = new List<QueryOrder>
    //            {
    //                new QueryOrder(false, QueryOrder.FieldOptions.Created)
    //            }
    //        };

    //        QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
    //        Debug.Log("Lobbies Found: " + queryResponse.Results.Count);

    //        foreach (Lobby iLobby in queryResponse.Results)
    //        {
    //            Debug.Log(iLobby.Name + " " + iLobby.MaxPlayers);
    //        }
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e);
    //    }
    //}

    //private async void JoinLobbyByCode(string lobbyCode)
    //{
    //    try
    //    {
    //        JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
    //        {
    //            Player = GetPlayer()
    //        };

    //        Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
    //        joinedLobby = lobby;
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e);
    //    }

    //}

    //public async void RefreshLobbyList()
    //{
    //    try
    //    {
    //        QueryLobbiesOptions options = new QueryLobbiesOptions();
    //        options.Count = 25;

    //        options.Filters = new List<QueryFilter> {
    //            new QueryFilter(
    //                field: QueryFilter.FieldOptions.AvailableSlots,
    //                op: QueryFilter.OpOptions.GT,
    //                value: "0"
    //                )
    //        };

    //        options.Order = new List<QueryOrder> {
    //            new QueryOrder(asc: false, field: QueryOrder.FieldOptions.Created)
    //        };

    //        QueryResponse lobbyListQueryResponse = await Lobbies.Instance.QueryLobbiesAsync();
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e);
    //    }
    //}

    //private async void MigrateLobbyHost()
    //{
    //    try
    //    {
    //        hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
    //        {
    //            HostId = joinedLobby.Players[1].Id,
    //        });

    //        joinedLobby = hostLobby;

    //        PrintPlayers(hostLobby);
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e);
    //    }
    //}

    //private async void DeleteLobby()
    //{
    //    try
    //    {
    //        await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e);
    //    }
    //}





    // ===========================================================================================================================
    //              NOTHING PAST HERE IS USABLE AS OF RIGHT NOW -- NEED TO REWORK / FIGURE IT OUT !
    // ===========================================================================================================================

    //private async void HandleLobbyPollForUpdates()
    //{
    //    if (joinedLobby != null)
    //    {
    //        lobbyTimer -= Time.deltaTime;
    //        if (lobbyTimer < 0)
    //        {
    //            lobbyTimer = lobbyTimerMax;

    //            joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

    //            OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { evt_lobby = joinedLobby });

    //            if (!IsPlayerInLobby())
    //            {
    //                Debug.Log("Player was kicked");
    //                OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { evt_lobby = joinedLobby });
    //                joinedLobby = null;
    //            }

    //            if (joinedLobby.Data[KEY_START_GAME].Value != "0")
    //            {
    //                if (!IsLobbyHost())
    //                {
    //                    RelayManager.Instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
    //                }

    //                joinedLobby = null;
    //            }

    //        }
    //    }
    //}


    //private bool IsPlayerInLobby()
    //{
    //    if (joinedLobby != null && joinedLobby.Players != null)
    //    {
    //        foreach (Player player in joinedLobby.Players)
    //        {
    //            if (player.Id == AuthenticationService.Instance.PlayerId)
    //                return true;
    //        }
    //    }
    //    return false;
    //}


    //public bool IsLobbyHost()
    //{
    //    return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    //}


    //private async void UpdateLobbyGameMode(string gameMode)
    //{
    //    try
    //    {
    //        hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
    //        {
    //            Data = new Dictionary<string, DataObject> {
    //                { KEY_GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, gameMode)}
    //            }
    //        });

    //        joinedLobby = hostLobby;

    //        PrintPlayers(hostLobby);
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e);
    //    }
    //}

    //private void PrintPlayers(Lobby lobby)
    //{
    //    Debug.Log("Players in Lobby: " + lobby.Name);
    //    foreach (var player in lobby.Players)
    //    {
    //        Debug.Log(player.Id + " " + player.Data[KEY_PLAYER_NAME].Value);
    //    }
    //}

}
