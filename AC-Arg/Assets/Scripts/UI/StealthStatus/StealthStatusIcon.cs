using UnityEngine;
using UnityEngine.UI;

public class StealthStatusIcon : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        EventManager.Instance.Subscribe(Evento.OnStealthUpdate, UpdateStealthIcon);
    }

    public void UpdateStealthIcon(params object[] parameters)
    {
        if (parameters != null && parameters.Length > 0 && parameters[0] is StealthStatusSO)
        {
            StealthStatusSO status = (StealthStatusSO)parameters[0];
            image.sprite = status.icon;
            image.color = status.statusColor;
        }
        else
        {
            Debug.LogError("Parámetro incorrecto o ausente para UpdateStealthIcon.");
        }
    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnStealthUpdate, UpdateStealthIcon);
        }
    }
}
