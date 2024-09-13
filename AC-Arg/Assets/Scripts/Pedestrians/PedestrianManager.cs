using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianManager : Singleton<PedestrianManager>
{
    [SerializeField] PedestrianWaypointCollection[] pedestrianWaypointCollection;

    public Vector3 GetRandomWaypoint()
    {
        return GetRandomWaypointInSpecificCollection(Random.Range(0, pedestrianWaypointCollection.Length));
    }

    public Vector3 GetRandomWaypointInSpecificCollection(int collectionIndex)
    {
        return pedestrianWaypointCollection[collectionIndex].GetRandomWaypointPosition();
    }

    //same but pássing a PedestrianWaypointCollection as paameter

    public Vector3 GetRandomWaypointInSpecificCollection(PedestrianWaypointCollection collection)
    {
        return collection.GetRandomWaypointPosition();
    }

    public void TriggerPedestrianAlarm(Pedestrian pedestrian)
    {
        EnemyManager.Instance.TriggerPedestrianAlarm(pedestrian.transform.position);
    }
}
