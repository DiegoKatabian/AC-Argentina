using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Cutscene cutscene;

    private void OnTriggerEnter(Collider other)
    {
        //if is player
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Cutscene Triggered by player");
            CutsceneManager.Instance.PlayCutscene(cutscene);
        }
    }

    
}
