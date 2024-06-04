using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem targetParticleSystem;
    public bool doesPlaySound;
    public AudioClip soundToPlay;

    public bool doesStopSound;
    public AudioClip soundToStop;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asumiendo que la etiqueta del jugador es "Player"
        {
            targetParticleSystem.Play();
            if (doesPlaySound && soundToPlay != null)
            {
                AudioManager.Instance.PlaySound(soundToPlay);
            }

            if (doesStopSound && soundToStop != null)
            {
                AudioManager.Instance.StopSound(soundToStop);
            }
        }
    }
}
