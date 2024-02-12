using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LookAtCamera : MonoBehaviour
{
    [SerializeField] private PlayerNetwork playerScript;

    private void LateUpdate()
    {
        if (!playerScript.IsOwner)
        {
            transform.LookAt(transform.position + -playerScript.GetPlayerCam().transform.forward);
        }
    }
}
