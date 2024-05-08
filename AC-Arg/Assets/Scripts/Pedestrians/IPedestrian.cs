using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPedestrian
{
    public GameObject gameObject { get; }

    public void SetInteractionMarkerActive(bool active);
    public void GetAssassinated(GameObject assassin);
    public void GetStolen();
    public bool CanInteract();
}
