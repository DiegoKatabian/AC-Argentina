using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stealth Status", menuName = "StealthStatus")]
public class StealthStatusSO : ScriptableObject
{
    public string statusName;
    public Color statusColor;
}
