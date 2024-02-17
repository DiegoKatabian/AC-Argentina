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

    public float canvasDelayTime = 2.5f;

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
        //reaplace with corrouite
        StartCoroutine(WaitBeforeCanvas(true, canvasDelayTime));
    }

    internal void DisableDestinationPopup()
    {
        //reaplace with corrouite
        StartCoroutine(WaitBeforeCanvas(false, 0.5f));
        MoveTaxi(_currentDestination);
    }

    //an ienumerator that waits 1 seconds before doing destinationPopup.SetActive(state);

    public IEnumerator WaitBeforeCanvas(bool state, float time)
    {
        yield return new WaitForSeconds(time);
        destinationPopup.SetActive(state);
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
