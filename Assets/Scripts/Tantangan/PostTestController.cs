using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using ArabicSupport;
using UnityEngine.SceneManagement;

public class PostTestController : MonoBehaviour
{
    public GameObject SelesaiPanel;
    public TextMeshProUGUI TimerText;
    public Question[] questionData; // Array ScriptableObject pertanyaan
    public TextMeshProUGUI questionText; // UI Text untuk pertanyaan
    public Button[] optionButtons; // Tombol pilihan
    private int currentQuestion = 0;
    private int score = 0;
    private float timer;
    public bool isTimerRunning = true;

    // Variabel untuk menyimpan data per bagian
    private Dictionary<string, float> sectionTimers; // Waktu per bagian
    private Dictionary<string, int> sectionScores; // Skor per bagian
    private Dictionary<string, int> sectionQuestionCount; // Jumlah soal per bagian
    private string currentSection;

    private List<Question> questions;

    // Bobot untuk AHP (satuan harus bernilai 1)
    private float accuracyWeight = 0.5f; // Ketepatan jawaban
    private float timeWeight = 0.3f; // Waktu menjawab
    private float relevanceWeight = 0.2f; // Relevansi bagian

    void Start()
    {
        // Inisialisasi dictionary untuk menyimpan nilai per bagian
        sectionTimers = new Dictionary<string, float>();
        sectionScores = new Dictionary<string, int>();
        sectionQuestionCount = new Dictionary<string, int>();

        // Copy questionData to List and shuffle questions for randomization
        questions = new List<Question>(questionData);
        ShuffleList(questions);

        // Hitung jumlah soal per bagian
        foreach (Question question in questions)
        {
            if (!sectionScores.ContainsKey(question.relatedSection))
            {
                sectionScores[question.relatedSection] = 0;
                sectionTimers[question.relatedSection] = 0;
                sectionQuestionCount[question.relatedSection] = 0;
            }
            sectionQuestionCount[question.relatedSection]++;
        }

        LoadQuestion();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            TimerText.text = $"Time: {timer:F2}";
        }
    }

    public void LoadQuestion()
    {
        if (currentQuestion < questions.Count)
        {
            // Reset timer untuk soal baru
            if (currentQuestion == 0 || currentSection != questions[currentQuestion].relatedSection)
            {
                // Simpan waktu sebelumnya sebelum berpindah ke bagian lain
                if (currentQuestion > 0)
                {
                    sectionTimers[currentSection] += timer;
                }

                timer = 0;
                currentSection = questions[currentQuestion].relatedSection;
            }

            // Set pertanyaan
            questionText.text = ArabicFixer.Fix(questions[currentQuestion].questionText);

            // Copy options and shuffle them for random answer order
            List<string> shuffledOptions = new List<string>(questions[currentQuestion].options);
            ShuffleList(shuffledOptions);

            // Set pilihan jawaban
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < shuffledOptions.Count) // Pastikan indeks tidak melebihi jumlah opsi yang ada
                {
                    optionButtons[i].gameObject.SetActive(true);
                    TextMeshProUGUI buttonText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();

                    if (buttonText != null)
                    {
                        // Cek apakah teks adalah bahasa Arab dan ubah jika perlu
                        if (IsArabic(shuffledOptions[i]))
                        {
                            buttonText.text = ArabicFixer.Fix(shuffledOptions[i]);
                        }
                        else
                        {
                            buttonText.text = shuffledOptions[i];
                        }
                    }

                    // Tambahkan listener untuk jawaban
                    int index = i; // Capture index for lambda
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => CheckAnswer(shuffledOptions[index]));
                }
                else
                {
                    // Sembunyikan tombol jika tidak ada opsi
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // Simpan waktu terakhir
            sectionTimers[currentSection] += timer;

            // Semua soal selesai
            Debug.Log($"Post-Test selesai! Skor Anda: {score}");
            isTimerRunning = false;
            ShowSelesaiPanel();
        }
    }

    public void CheckAnswer(string selectedAnswer)
    {
        // Periksa jawaban benar berdasarkan teks
        bool isCorrect = selectedAnswer == questions[currentQuestion].options[questions[currentQuestion].correctIndex];

        if (isCorrect)
        {
            score++;
            sectionScores[currentSection] += (int)(100.0f / questions.Count) / sectionQuestionCount[currentSection]; // Setiap bagian mendapat skor proporsional dari 100
            Debug.Log("Jawaban benar!");
        }
        else
        {
            Debug.Log("Jawaban salah.");
        }

        // Menghitung skor AHP untuk pertanyaan ini
        CalculateAHPScore(isCorrect);

        currentQuestion++;
        LoadQuestion();
    }

    public void ShowSelesaiPanel()
    {
        // Aktifkan panel selesai
        SelesaiPanel.SetActive(true);

        // Tampilkan skor total dan detail setiap bagian di panel selesai
        TextMeshProUGUI selesaiText = SelesaiPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (selesaiText != null)
        {
            selesaiText.text = $"Skor Total: {score * (100.0f / questions.Count):F1}/100\n";

            // Tampilkan rincian per bagian
            foreach (var section in sectionScores.Keys)
            {
                float averageScore = (float)sectionScores[section] / sectionQuestionCount[section];
                selesaiText.text += $"\nBagian {section}:\n- Skor: {sectionScores[section]}";
            }
        }
    }

    private void CalculateAHPScore(bool isCorrect)
    {
        // Ketepatan jawaban, 1 untuk benar, 0 untuk salah
        float accuracyScore = isCorrect ? 1.0f : 0.0f;

        // Penilaian waktu (semakin cepat semakin tinggi nilainya)
        float maxTime = 30.0f; // Batas waktu ideal dalam detik
        float timeScore = Mathf.Clamp(1.0f - (timer / maxTime), 0.0f, 1.0f);

        // Relevansi bagian (bisa dikustomisasi, misal semakin sulit semakin tinggi nilainya)
        float relevanceScore = sectionScores.ContainsKey(currentSection) ? 1.0f : 0.5f; // Skor relevansi default

        // Hitung nilai akhir AHP untuk pertanyaan ini
        float ahpScore = (accuracyScore * accuracyWeight) + (timeScore * timeWeight) + (relevanceScore * relevanceWeight);

        // Tambahkan hasil AHP ke skor bagian yang sedang diujikan
        sectionScores[currentSection] += Mathf.RoundToInt(ahpScore * 100 / sectionQuestionCount[currentSection]);
        Debug.Log($"Nilai AHP untuk soal ini: {ahpScore * 100:F1}");
    }

    private bool IsArabic(string text)
    {
        // Periksa apakah teks mengandung karakter Arab
        foreach (char c in text)
        {
            if (c >= 0x0600 && c <= 0x06FF) // Rentang Unicode karakter Arab
            {
                return true;
            }
        }
        return false;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void GetBackToMainMenu()
    {
        // Fungsi untuk kembali ke menu utama (Scene Management)
        SceneManager.LoadScene("MainMenu"); // Ganti dengan nama scene menu utama Anda
    }
}
