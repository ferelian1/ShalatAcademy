using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomUI/ThemeSO", fileName = "ThemeSO")]
public class ThemeSO : ScriptableObject
{
    [Header("Primary")]
    public Color primary_BG;
    public Color primary_text;

    [Header("Secondary")]
    public Color secondary_BG;
    public Color secondary_text;

    [Header("Tertiary")]
    public Color tertiary_BG;
    public Color tertiary_text;

    [Header("other")]
    public Color disable;

    public Color GetBackgroundColor(Style style)
    {
        if(style == Style.Primary)
        {
            return primary_BG;
        }else if(style == Style.Secondary)
        {
            return secondary_BG;
        }else if(style == Style.Tertiary)
        {
            return tertiary_BG;
        }
        return disable;
    }

    public Color GetTextColor(Style style)
    {
        if (style == Style.Primary)
        {
            return primary_text;
        }
        else if (style == Style.Secondary)
        {
            return secondary_text;
        }
        else if (style == Style.Tertiary)
        {
            return tertiary_text;
        }
        return disable;
    }
}
