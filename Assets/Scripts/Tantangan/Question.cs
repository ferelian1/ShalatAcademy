using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Shalat/Question", order = 1)]
public class Question : ScriptableObject
{
    public string questionText; // Teks pertanyaan
    public string[] options; // Pilihan jawaban
    public int correctIndex; // Index jawaban benar
    public string relatedSection; // Bagian terkait (misal: "Takbir", "Rukuk", dll.)
}
