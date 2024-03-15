using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StealthStatus
{
    Anonymous,
    Warning,
    Alert,
    Hidden
}

[CreateAssetMenu(fileName = "New Stealth Status", menuName = "StealthStatus")]
public class StealthStatusSO : ScriptableObject
{
    public string statusName;
    public StealthStatus status;
    public Color statusColor;
}
