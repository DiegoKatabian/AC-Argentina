using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make it so i can create a new taxidestination asset thru the right click context menu

[CreateAssetMenu(fileName = "New Taxi Destination", menuName = "TaxiDestination")]
public class TaxiDestination : ScriptableObject
{
    public Vector3 position;
    public string destinationName;

}
