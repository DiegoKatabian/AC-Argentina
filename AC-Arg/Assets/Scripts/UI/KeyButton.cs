using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class KeyButton : MonoBehaviour
{
    public KeyCode key;
    public Evento triggeringEventPressed;
    public Evento triggeringEventReleased;
    bool keyup = false;
    bool keydown = false;
    public Button button { get; private set; }

    Graphic targetGraphic;
    Color normalColor;

    void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = false;
        targetGraphic = GetComponent<Graphic>();

        ColorBlock cb = button.colors;
        cb.disabledColor = cb.normalColor;
        button.colors = cb;
    }

    void Start()
    {
        button.targetGraphic = null;
        EventManager.Instance.Subscribe(triggeringEventPressed, SetKeyDown);
        EventManager.Instance.Subscribe(triggeringEventReleased, SetKeyUp);

        Up();
    }

    private void SetKeyDown(object[] parameters)
    {
        keydown = true;
    }

    private void SetKeyUp(object[] parameters)
    {
        keyup = true;
    }


    void Update()
    {
        if (keydown)
        {
            Down();
            keydown = false;
        }
        else if (keyup)
        {
            Up();
            keyup = false;
        }
    }

    void Up()
    {
        StartColorTween(button.colors.normalColor, false);
    }

    void Down()
    {
        StartColorTween(button.colors.pressedColor, false);
        button.onClick.Invoke();
    }

    void StartColorTween(Color targetColor, bool instant)
    {
        if (targetGraphic == null)
            return;

        targetGraphic.CrossFadeColor(targetColor, instant ? 0f : button.colors.fadeDuration, true, true);
    }

    void OnApplicationFocus(bool focus)
    {
        Up();
    }

    public void LogOnClick()
    {
        Debug.Log("LogOnClick() - " + GetComponentInChildren<Text>().text);
    }
}