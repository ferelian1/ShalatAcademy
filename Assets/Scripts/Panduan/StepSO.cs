using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShalatStep", menuName = "Shalat/Step")]
public class StepSO : ScriptableObject
{
    public string stepName; // Nama langkah (misalnya, "Takbiratul Ihram")
    public string arabicText; // Teks Arab
    public string latinText; // Teks Latin
    public string translation; // Terjemahan
    public string animationName; // Nama animasi yang terkait
    public AudioClip stepAudio; // Audio clip untuk bacaan
    
}
