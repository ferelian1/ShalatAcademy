using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.cyborgAssets.inspectorButtonPro;

public class View2 : CustomUIComponent
{
    public ViewSO viewData;

    public GameObject ContainerTop;
    public GameObject ContainerCenter;
    public GameObject ContainerBottom;

    private Image ImageTop;
    private Image ImageCenter;
    private Image ImageBottom;

    private VerticalLayoutGroup verticalLayoutGroup;

    public override void Setup()
    {
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        ImageTop = ContainerTop.GetComponent<Image>();
        ImageCenter = ContainerCenter.GetComponent<Image>();
        ImageBottom = ContainerBottom.GetComponent<Image>();

    }

    public override void Configure()
    {
        verticalLayoutGroup.padding = viewData.padding;
        verticalLayoutGroup.spacing = viewData.spacing;

        ImageTop.color = viewData.theme.primary_BG;
        ImageCenter.color = viewData.theme.secondary_BG;
        ImageBottom.color = viewData.theme.tertiary_BG;

    }
}
