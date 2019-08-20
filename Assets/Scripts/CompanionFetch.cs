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
    [SerializeField] float fetchNumber = 3f;
    [SerializeField] LayerMask ignoreLayer;

    float remainingFetch;
    Vector3 rayOrigin;
    GameObject markerInstance = null;
    RaycastHit hit;

    public bool raycastingMarker = false;


    private void Awake()
    {
        remainingFetch = fetchNumber;
        companion = FindObjectOfType<CompanionMovement>();
    }

    private void Update()
    {
        if (remainingFetch <= 0) return;

        if (Input.GetButtonDown("CompanionFetch"))
        {
            ToggleRaycasting();
        }

        if (raycastingMarker)
        {
            FetchRaycast();
        }

        if (Input.GetButtonDown("ConfirmFetch"))
        {
            if (raycastingMarker)
            {
                companion.Fetch(hit);
                remainingFetch -= 1;
            }
            raycastingMarker = false;
            Destroy(markerInstance);
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
        if (Physics.Raycast(rayOrigin, camera.transform.forward, out hit, 50f, ~ignoreLayer))
        {
            MoveFetchMarker(hit);
        }

    }

    private void MoveFetchMarker(RaycastHit hit)
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

    public float RemainingFetch()
    {
        return remainingFetch;
    }

    public void ResetFetch()
    {
        remainingFetch = fetchNumber;
    }


    private bool IsReachable(RaycastHit hit)
    {
        return companion.CanReachTarget(hit);
    }
}
