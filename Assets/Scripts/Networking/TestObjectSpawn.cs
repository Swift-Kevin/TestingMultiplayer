using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestObjectSpawn : NetworkBehaviour
{
    private Transform spawnedObjectTransform;
    [SerializeField] private Transform spawnObjectPrefab;

    private void Update()
    {
        if (!IsOwner) return;

        if (InputManager.Instance.SpawnObjectPerformed())
        {
            SpawnObjectServerRpc();
        }
    }

    [ServerRpc]
    private void SpawnObjectServerRpc()
    {
        spawnedObjectTransform = Instantiate(spawnObjectPrefab, gameObject.transform);
        spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
    }
}
