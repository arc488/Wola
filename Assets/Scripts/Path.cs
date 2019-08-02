using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] float waypointTolerance = 1f;
    private void OnDrawGizmos()
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (i < waypoints.Length - 1)
            {
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);

            }
        }
    }

    public float GetWaypointTolerance()
    {
        return waypointTolerance;
    }
}
