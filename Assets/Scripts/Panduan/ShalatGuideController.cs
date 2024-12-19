using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using com.cyborgAssets.inspectorButtonPro;
using ArabicSupport;

public class ShalatGuideController : MonoBehaviour
{
    public Animator animator; // Reference ke Animator
    public AudioSource audioSource; // Reference ke AudioSource
    public StepSO[] steps; // Array dari langkah shalat

    public GameObject TantanganPanel; // Panel Tantangan setelah selesai panduan
    public TextMeshProUGUI arabicTextUI; // UI untuk Teks Arab
    public TextMeshProUGUI latinTextUI; // UI untuk Teks Latin
    public TextMeshProUGUI translationTextUI; // UI untuk Terjemahan
    public Button nextButton; // Tombol untuk langkah berikutnya
    public Button previousButton; // Tombol untuk langkah sebelumnya

    private int currentStep = 0;

    void Start()
    {
        // Pastikan semua referensi sudah diassign
        if (arabicTextUI == null) Debug.LogError("arabicTextUI is not assigned!");
        if (latinTextUI == null) Debug.LogError("latinTextUI is not assigned!");
        if (translationTextUI == null) Debug.LogError("translationTextUI is not assigned!");
        if (nextButton == null) Debug.LogError("nextButton is not assigned!");
        if (previousButton == null) Debug.LogError("previousButton is not assigned!");
        if (steps == null || steps.Length == 0) Debug.LogError("steps array is empty or null!");
        if (animator == null) Debug.LogError("animator is not assigned!");
        if (audioSource == null) Debug.LogError("audioSource is not assigned!");

        // Inisialisasi panduan dengan langkah pertama jika semua objek sudah valid
        if (arabicTextUI != null && latinTextUI != null && translationTextUI != null && steps.Length > 0)
        {
            LoadStep(currentStep);
        }

        // Menambahkan listener untuk tombol navigasi
        nextButton.onClick.AddListener(NextStep);
        previousButton.onClick.AddListener(PreviousStep);
        
        // Atur state tombol Previous di awal
        previousButton.gameObject.SetActive(currentStep > 0);
    }

    public void LoadStep(int stepIndex)
    {
        if (stepIndex >= 0 && stepIndex < steps.Length)
        {
            currentStep = stepIndex;

            // Cek apakah arabicText dari StepSO tidak null sebelum di proses
            if (steps[currentStep].arabicText != null)
            {
                arabicTextUI.text = ArabicSupport.ArabicFixer.Fix(steps[currentStep].arabicText);
            }
            else
            {
                Debug.LogWarning("Arabic text is null for step " + currentStep);
                arabicTextUI.text = "Text not available"; // Tampilkan teks fallback
            }

            // Set teks Latin dan Terjemahan
            latinTextUI.text = steps[currentStep].latinText;
            translationTextUI.text = steps[currentStep].translation;

            // Mainkan animasi
            if (animator != null)
            {
                animator.Play(steps[currentStep].animationName);
            }
            else
            {
                Debug.LogError("Animator is null");
            }

            // Mainkan audio
            if (audioSource != null && steps[currentStep].stepAudio != null)
            {
                audioSource.clip = steps[currentStep].stepAudio;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource or audio clip is missing for step " + currentStep);
            }

            // Mengatur state tombol Previous
            previousButton.gameObject.SetActive(currentStep > 0);
        }
        else
        {
            Debug.LogError("Invalid stepIndex: " + stepIndex);
        }
    }

    [ProButton]
    public void PlayCurrentStep()
    {
        if (currentStep < steps.Length)
        {
            // Mainkan animasi
            if (animator != null)
            {
                animator.Play(steps[currentStep].animationName);
            }
            else
            {
                Debug.LogError("Animator is null");
            }

            // Mainkan audio
            if (audioSource != null && steps[currentStep].stepAudio != null)
            {
                audioSource.clip = steps[currentStep].stepAudio;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource or audio clip is missing for step " + currentStep);
            }
        }
        else
        {
            Debug.LogWarning("Current step is out of bounds: " + currentStep);
        }
    }

    [ProButton]
    public void NextStep()
    {
        if (currentStep < steps.Length - 1)
        {
            currentStep++;
            LoadStep(currentStep);
        }
        else
        {
            // Jika langkah terakhir, tampilkan panel tantangan
            TantanganPanel.SetActive(true);
        }
    }

    [ProButton]
    public void PreviousStep()
    {
        if (currentStep > 0)
        {
            currentStep--;
            LoadStep(currentStep);
        }
    }

    public void GoToTantangan()
    {
        SceneManager.LoadScene("Tantangan"); // Pindah ke scene Tantangan
    }
}
