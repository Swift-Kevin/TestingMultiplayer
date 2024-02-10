using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyEntryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lobbyNameText;

    public void UpdateLobby(Lobby lobby)
    {
        lobbyNameText.text = lobby.Name;
    }
}
