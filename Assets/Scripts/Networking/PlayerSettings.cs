using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PLayerSettings : NetworkBehaviour
{
    [SerializeField] private MeshRenderer colorMeshRenderer;
    [SerializeField] TextMeshProUGUI text_playerName;
    [SerializeField] private NetworkVariable<FixedString128Bytes> networkPlayerName = new NetworkVariable<FixedString128Bytes>(
        "Player: 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public List<Color> colors = new List<Color>();

    private void Awake()
    {
        colorMeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        if (InputManager.Instance.CycleColorWasPerformed() && IsOwner)
        {
            colorMeshRenderer.material.color = colors[(int)Random.Range(0,4)];
        }
    }

    public override void OnNetworkSpawn()
    {
        networkPlayerName.Value = "Player: " + (OwnerClientId + 1);
        text_playerName.text = networkPlayerName.Value.ToString();
        colorMeshRenderer.material.color = colors[(int)OwnerClientId];
    }

}
