using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] private Transform spawn;

    public Transform Spawner => spawn;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        InputManager.Instance.playerInput.Player.ChangeSpawn.started += UpdateSpawn;
    }

    private void UpdateSpawn(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        spawn.position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
    }
}
