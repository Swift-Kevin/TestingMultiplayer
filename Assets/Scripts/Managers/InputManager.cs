using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows.WebCam;

public class InputManager : MonoBehaviour
{
    public PlayerInputs playerInput;
    public static InputManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerInput = new PlayerInputs();
        playerInput.Player.Enable();

        playerInput.Player.PauseMenu.performed += PauseMenu;
    }

    private void PauseMenu(InputAction.CallbackContext context)
    {
        if (UIManagerScript.Instance.GetIsInGame())
        {
            GameManager.Instance.PauseGame();
        }
    }

    public bool SprintPerformed()
    {
        return playerInput.Player.Sprint.WasPerformedThisFrame();
    }

    public bool SprintReleased()
    {
        return playerInput.Player.Sprint.WasReleasedThisFrame();
    }

    public Vector2 CameraReadValue()
    {
        return playerInput.Player.Camera.ReadValue<Vector2>();
    }

    public Vector2 PlayerMoveVector()
    {
        return playerInput.Player.Movement.ReadValue<Vector2>();
    }

    public bool SpawnObjectPerformed()
    {
        return playerInput.Player.SpawnObject.WasPerformedThisFrame();
    }

    private bool GetSpawnObj()
    {
        return playerInput.Player.SpawnObject.WasPerformedThisFrame();
    }

    public bool CycleColorWasPerformed()
    {
        return playerInput.Player.ColorCycle.WasPerformedThisFrame();
    }

    public bool WasSummonConsolePressed()
    {
        return playerInput.Player.SummonConsole.WasPerformedThisFrame();
    }

    public bool WasRayCastPressed()
    {
        return playerInput.Player.CheckRayHit.WasPerformedThisFrame();
    }

}
