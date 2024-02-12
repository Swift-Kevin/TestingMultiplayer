using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.Processors;
using System.Runtime.Serialization.Json;
using UnityEngine.Windows;

public class PlayerNetwork : NetworkBehaviour
{
    [Range(0.1f, 1000f)]
    [SerializeField] private float moveSpeed = 15f;
    [Range(0.1f, 5000f)]
    [SerializeField] private float camSens = 100f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Camera playerCam;

    float yRotation, origSpeed;

    // NOTE !!! Should not use Awake or Start with Networking objects
    public override void OnNetworkSpawn()
    {
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
    }

    private void SprintChecking()
    {
        if (InputManager.Instance.SprintPerformed())
        {
            moveSpeed *= 2f;
        }
        else if (InputManager.Instance.SprintReleased())
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
        if (!IsOwner) return;

        var input = InputManager.Instance.CameraReadValue() * Time.deltaTime * camSens;
        yRotation += -input.y;
        yRotation = Mathf.Clamp(yRotation, -45, 45);

        // Rotate Vertically
        cameraTransform.parent.localRotation = Quaternion.Euler(yRotation, 0, cameraTransform.parent.localRotation.z);

        // Rotate horizontally
        if (Mathf.Abs(input.x) > 0)
        {
            transform.Rotate(Vector3.up * input.x);
        }
    }

    private void Movement()
    {
        Vector2 move = InputManager.Instance.PlayerMoveVector() * moveSpeed * Time.deltaTime;
        rigidBody.velocity = transform.TransformDirection(new Vector3(move.x, rigidBody.velocity.y, move.y));
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            GameManager.Instance.ReturnToMainMenu();
        }
    }

    public GameObject GetPlayerObject()
    {
        return playerObject;
    }

    public Camera GetPlayerCam()
    {
        return playerCam;
    }
}
