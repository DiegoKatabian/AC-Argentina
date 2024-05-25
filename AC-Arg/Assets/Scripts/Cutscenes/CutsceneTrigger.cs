using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Cutscene cutscene;
    public bool isOneTimeOnly = true;
    bool hasBeenTriggered = false;
    

    private void OnTriggerEnter(Collider other)
    {
        if (isOneTimeOnly && hasBeenTriggered) return;


        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Cutscene Triggered by player");
            CutsceneManager.Instance.PlayCutscene(cutscene);
            hasBeenTriggered = true;
        }
    }

    
}
