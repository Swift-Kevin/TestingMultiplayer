using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.Processors;
using System.Runtime.Serialization.Json;

public class PlayerNetwork : NetworkBehaviour
{
    PlayerInputs playerInput;
    private Transform spawnedObjectTransform;

    //GameObject playerCam;

    [Range(0.1f, 1000f)]
    [SerializeField] private float moveSpeed = 15f;
    [Range(0.1f, 5000f)]
    [SerializeField] private float camSens = 100f;
    [SerializeField] private Transform spawnObjectPrefab;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Rigidbody rigidBody;

    float yRotation, origSpeed;

    // NOTE !!! Should not use Awake or Start with Networking objects
    public override void OnNetworkSpawn()
    {
        playerInput = new PlayerInputs();
        playerInput.Player.Enable();
        origSpeed = moveSpeed;

        if (IsLocalPlayer)
            return;
        cameraTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!IsOwner) return; // IsOwner is apart of NetworkBehavior and not MonoBehavior

        SprintChecking();

        AimCamera();

        if (playerInput.Player.SpawnObject.WasPerformedThisFrame())
        {
            SpawnObjectServerRpc();
        }
    }

    private void SprintChecking()
    {
        if (playerInput.Player.Sprint.WasPerformedThisFrame())
        {
            moveSpeed *= 2f;
        }
        else if (playerInput.Player.Sprint.WasReleasedThisFrame())
        {
            moveSpeed = origSpeed;
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void AimCamera()
    {
        var input = playerInput.Player.Camera.ReadValue<Vector2>() * Time.deltaTime * camSens;
        yRotation += -input.y;
        yRotation = Mathf.Clamp(yRotation, -45, 45);

        // Rotate Vertically
        cameraTransform.parent.localRotation = Quaternion.Euler(yRotation, 0, cameraTransform.parent.localRotation.z);

        // Rotate horizontally
        transform.Rotate(Vector3.up * input.x);
    }

    private void Movement()
    {
        Vector2 move = playerInput.Player.Movement.ReadValue<Vector2>() * moveSpeed * Time.deltaTime;
        rigidBody.velocity = transform.TransformDirection(new Vector3(move.x, rigidBody.velocity.y, move.y));
    }

    [ServerRpc]
    private void SpawnObjectServerRpc()
    {
        Debug.Log("Spawning Object");
        spawnedObjectTransform = Instantiate(spawnObjectPrefab, gameObject.transform);
        spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
    }
}
