using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerRayCaster : MonoBehaviour
{

    [SerializeField] private float maxDist = 10f;
    [SerializeField] private LineRenderer lineRenderer;

    public LayerMask hittableLayers;
    private RaycastHit rayHit;

    private void Start()
    {
        //ray = new Ray(transform.position, transform.forward);
        InputManager.Instance.playerInput.Player.CheckRayHit.started += Started_RightTrigger;
        lineRenderer.endColor = Color.red;
        lineRenderer.startColor = Color.blue;
    }

    private void Started_RightTrigger(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Checking Ray");
        CheckRay();
    }

    private void Update()
    {
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.Log("Entering Line Renderer");
        if (Physics.Raycast(ray, out rayHit))
        {
            Debug.Log("Drawing Line");
            Debug.DrawLine(ray.origin, rayHit.point, Color.red);
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, rayHit.point);
        }
        else
        {
            Debug.Log("Line too long" + rayHit);
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxDist, Color.blue);
        }
    }

    private void CheckRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out rayHit, maxDist))
        {
            Debug.Log("UI was hit!" + rayHit.collider.name);
            //Button uiButton = rayHit.collider.GetComponent<Button>();

            //if (uiButton != null)
            {
                //Debug.Log("Button Was hit lets go...");
                //uiButton.onClick.Invoke();
            }
        }
    }
}
