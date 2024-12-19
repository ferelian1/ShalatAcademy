using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomUIComponent : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {

        Init();
    }

    public abstract void Setup();
    public abstract void Configure();
    public void Init()
    {
        Setup();
        Configure();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) // Prevent null issues in Editor
        {
            return;
        }
        Init();
    }
}
