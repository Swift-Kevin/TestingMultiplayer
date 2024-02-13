using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerVisual : NetworkBehaviour
{
    [SerializeField] private Renderer teamColourRenderer;

    private NetworkVariable<byte> colorIndex = new NetworkVariable<byte>(byte.MinValue);

    [ServerRpc]
    public void SetPlayerColorServerRpc(byte newTeamIndex)
    {
        if (newTeamIndex > ConnectionManager.Instance.availableColorList.Count)
        {
            Debug.Log("Current Index is outside of Colors available");
            return;
        }

        colorIndex.Value = newTeamIndex;
    }

    private void OnEnable()
    {
        colorIndex.OnValueChanged += OnColorChanged;
    }

    private void OnDisable()
    {
        colorIndex.OnValueChanged -= OnColorChanged;
    }

    private void OnColorChanged(byte oldTeamIndex, byte newTeamIndex)
    {
        if (!IsClient)
        {
            Debug.Log("Not Client trying to Change Color");
            return;
        }
        //teamColourRenderer.material.color = RelayManager.Instance.GetPlayerColor(newTeamIndex);
    }
}
