using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBillboard : MonoBehaviour
{
    void Update()
    {
        //transform.LookAt(Camera.main.transform.position, -Vector3.up);
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);


        //apply the same rotation to each children ibject of this object

        foreach (Transform child in transform)
        {
            child.LookAt(Camera.main.transform.position, -Vector3.up);
            child.eulerAngles = new Vector3(0, child.eulerAngles.y + 180, 0);
        }

    }
}
