using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianManager : Singleton<PedestrianManager>
{
    public List<GameObject> _waypoints = new List<GameObject>();

    private void Start()
    {
        //all children of this object are waypoints
        foreach (Transform child in transform)
        {
            _waypoints.Add(child.gameObject);
        }
    }

    public Vector3 GetRandomWaypointPosition()
    {
        int randomIndex = Random.Range(0, _waypoints.Count);
        return _waypoints[randomIndex].transform.position;
    }

}
