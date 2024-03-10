using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class CustomNetworkManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public void CustomPlayerSpawn()
    {
        /*
         * 
         * // Create player object
			var team = teamManager.teams[teamId];
			var spawnPosition = team.RandomSpawnPosition;
			var spawnRotation = team.SpawnRotation;
			var playerPrefab = NetworkManager.Singleton.NetworkConfig.PlayerPrefab;
			var playerObject = GameObject.Instantiate(playerPrefab, spawnPosition, Const.NoRotation);

			// Assign team
			var player = playerObject.GetComponent<IPlayer>();
			player.TeamId = teamId;

			// Spawn player on every client
			var networkObject = playerObject.GetComponent<NetworkObject>();
			networkObject.SpawnAsPlayerObject(clientId);

			player.Respawn(spawnPosition, spawnRotation);
         */


    }



    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    //    player.GetComponent<Player>().color = Color.red;
    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    //}
}
