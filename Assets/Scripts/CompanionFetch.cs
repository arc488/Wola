using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionFetch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera camera;
    [SerializeField] GameObject markerPrefab = null;
    [SerializeField] CompanionMovement companion = null;

    bool raycastingMarker = false;
    Vector3 rayOrigin;
    GameObject markerInstance = null;
    RaycastHit hit;

    private void Awake()
    {
        companion = FindObjectOfType<CompanionMovement>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("CompanionFetch"))
        {
            ToggleRaycasting();
        }

        if (raycastingMarker)
        {
            FetchRaycast();
        }
    }

    private void ToggleRaycasting()
    {
        if (!raycastingMarker)
        {
            raycastingMarker = true;
        }
        else if (raycastingMarker)
        {
            raycastingMarker = false;
            if (markerInstance != null)
            {
                Destroy(markerInstance);
            }
        }
    }

    public void FetchRaycast()
    {
        rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(rayOrigin, camera.transform.forward, out hit, 50f))
        {
            if (hit.collider.tag == "Terrain" && IsReachable(hit))
            {
                if (markerInstance == null)
                {
                    markerInstance = Instantiate(markerPrefab, hit.transform);
                    markerInstance.transform.position = hit.point;
                }
                else
                {
                    markerInstance.transform.position = hit.point + new Vector3(0, 2, 0);
                }
            }
        }

    }

    private bool IsReachable(RaycastHit hit)
    {
        return companion.CanReachTarget(hit);
    }
}
