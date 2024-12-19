using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public GameObject PanelMainMenu;
    public GameObject PanelSettings;
    public void PanduanButton()
    {
        SceneManager.LoadScene("Panduan");
    }

    public void TantanganButton()
    {
        SceneManager.LoadScene("Tantangan");
    }

    public void SettingsButton()
    {
        PanelMainMenu.SetActive(false);
        PanelSettings.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        PanelMainMenu.SetActive(true);
        PanelSettings.SetActive(false);
    }
}
