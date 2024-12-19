using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CustomButton : CustomUIComponent
{
    public ThemeSO theme;
    public Style style;
    public UnityEvent onClick;

    private Button button;
    private TextMeshProUGUI buttonText;

    public override void Setup()
    {
        button = GetComponentInChildren<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found in children!");
            return;
        }

        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in children!");
            return;
        }
    }

    public override void Configure()
    {

        if (button == null)
        {
            Debug.LogError("Button component not found in children!");
            return;
        }

        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in children!");
            return;
        }
        ColorBlock cb = button.colors;
        cb.normalColor = theme.GetBackgroundColor(style);
        button.colors = cb;

        buttonText.color = theme.GetTextColor(style);
    }

    public void OnClick()
    {
        onClick.Invoke();
    }
}
