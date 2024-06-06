using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MovementCues
{
    public List<Transform> cues;
    public PawnName pawnName;
}


public class MovementCueManager : Singleton<MovementCueManager>
{
    public MovementCues[] movementCues;

    List<Queue<Transform>> queues = new List<Queue<Transform>>();

    public void Start()
    {
        foreach (MovementCues mc in movementCues)
        {
            Queue<Transform> q = new Queue<Transform>();
            foreach (Transform t in mc.cues)
            {
                //Debug.Log("manager: agrego cue a la queue");
                q.Enqueue(t);
            }
            //Debug.Log("manager: agrego queue a la lista");
            queues.Add(q);
        }
    }

    //whenever a character is done with a cue, call this function to get the next cue
    public Transform GetNextCue(PawnName character)
    {
        Transform t = null;
        for (int i = 0; i < movementCues.Length; i++)
        {
            //Debug.Log("manager: busco busco");
            if (movementCues[i].pawnName == character)
            {
                //Debug.Log("manager: encontre el movementCues de " + character);
                if (queues[i].Count > 0)
                {
                    //Debug.Log("manager: te doy el siguiente cue");
                    t = queues[i].Dequeue();
                }
            }
        }
        return t;

    }

}
