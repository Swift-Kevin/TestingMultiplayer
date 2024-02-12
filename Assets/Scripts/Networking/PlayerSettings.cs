using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private MeshRenderer colorMeshRenderer;
    [SerializeField] TextMeshProUGUI text_playerName;
    [SerializeField] private NetworkVariable<FixedString128Bytes> networkPlayerName = new NetworkVariable<FixedString128Bytes>(
        "Player: 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        colorMeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        networkPlayerName.Value = "Player: " + (OwnerClientId + 1);
        text_playerName.text = networkPlayerName.Value.ToString();
    }

}
