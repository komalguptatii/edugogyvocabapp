using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PushButton : MonoBehaviour
{
    // Start is called before the first frame update
    public static event Action<string> ButtonPressed = delegate { };
    private int dividerPosition;
    private string buttonName, buttonValue;

    void Start()
    {
        buttonName = gameObject.name;
        dividerPosition = buttonName.IndexOf("_");
        buttonValue = buttonName.Substring(0, dividerPosition);

        gameObject.GetComponent<Button>().onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        ButtonPressed(buttonValue);
    }

}
