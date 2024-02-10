using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

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

}
