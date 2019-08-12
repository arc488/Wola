using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionFetch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera camera;
    [SerializeField] GameObject markerPrefab = null;
    Vector3 rayOrigin;
    GameObject markerInstance = null;

    private void Update()
    {
        if (Input.GetButton("CompanionFetch"))
        {
            FetchRaycast();
        }
        else
        {
            if (markerInstance != null)
            {
                Destroy(markerInstance);
            }
        }
    }

    private void FetchRaycast()
    {
        RaycastHit[] hits;
        rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        hits = Physics.RaycastAll(rayOrigin, camera.transform.forward);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Terrain")
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

}
