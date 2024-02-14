using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiManager : MonoBehaviour
{
    public static TaxiManager instance;

    public TaxiDestination[] destinations;
    List<Taxi> allTaxis;
    TaxiDestination currentDestination;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private void Start()
    {
        Debug.Log("all taxis added to list");
        allTaxis = new List<Taxi>(FindObjectsOfType<Taxi>());
    }

    public void SetDestination(string destinationName)
    {
        Debug.Log("destination set");
        
        foreach (TaxiDestination destination in destinations)
        {
            if (destination.destinationName == destinationName)
            {
                currentDestination = destination;
                break;
            }
        }
        Debug.Log("current destination: " + currentDestination.destinationName);
    }
}
