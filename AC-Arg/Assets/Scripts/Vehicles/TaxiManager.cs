using System;
using System.Collections;
using System.Collections.Generic;
using TrafficSimulation;
using UnityEngine;
using UnityEngine.UI;


public class TaxiManager : MonoBehaviour
{
    public static TaxiManager instance;

    public TaxiDestination[] destinations;
    public GameObject destinationPopup;
    public Image destinationBlackScreen;

    List<Taxi> _allTaxis;
    Taxi _currentTaxi;
    TaxiDestination _currentDestination;

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

    public void SetDestination(string destinationName)
    {
        //Debug.Log("destination set");
        
        foreach (TaxiDestination destination in destinations)
        {
            if (destination.destinationName == destinationName)
            {
                _currentDestination = destination;
                break;
            }
        }
        Debug.Log("current destination: " + _currentDestination.destinationName);
        StartCoroutine(WaitForBlackScreenLerp());
        DisableDestinationPopup();
    }

    internal void EnableDestinationCanvas()
    {
        destinationPopup.SetActive(true);
    }

    internal void DisableDestinationPopup()
    {
        destinationPopup.SetActive(false);
        MoveTaxi(_currentDestination);
    }

    public IEnumerator WaitForBlackScreenLerp()
    {
        StartCoroutine(AlphaLerpUtility.LerpAlpha(destinationBlackScreen, 0, 1, 0.1f));
        yield return new WaitForSeconds(2);
        StartCoroutine(AlphaLerpUtility.LerpAlpha(destinationBlackScreen, 1, 0, 2));

    }

    private void MoveTaxi(TaxiDestination destination)
    {
        _currentTaxi.Teleport(destination);
        _currentTaxi.InduceStop();
    }

    internal void StartTrip(Taxi taxi)
    {
        _currentTaxi = taxi;
        EnableDestinationCanvas();
    }
}