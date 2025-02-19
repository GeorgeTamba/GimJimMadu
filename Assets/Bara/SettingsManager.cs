using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    void Start()
    {
        LoadSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();

        // Update sensitivitas secara langsung di game
        GameObject player = GameObject.FindWithTag("Player"); // Temukan objek player
        if (player != null)
        {
            PlayerLook lookScript = player.GetComponentInChildren<PlayerLook>();
            if (lookScript != null)
            {
                lookScript.sensitivity = sensitivitySlider.value; // Update sensitivitas real-time
            }
        }
    }


    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");

        if (PlayerPrefs.HasKey("Volume"))
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }
    public void GoBackIngame()
    {
        // Kembali ke Main Menu jika dari MainMenu, kembali ke GameScene jika dari Pause Menu
        if (PlayerPrefs.GetInt("LastScene") == 1)
            SceneManager.LoadScene("MainMenu");
        else
            SceneManager.LoadScene("GameScene");
    }public void GoBackInmainmenu()
    {
        // Kembali ke Main Menu jika dari MainMenu, kembali ke GameScene jika dari Pause Menu
            if (PlayerPrefs.GetInt("LastScene") == 1)
                SceneManager.LoadScene("MainMenu");
            else
                SceneManager.LoadScene("GameScene");
    }

    public void ApplySettings()
    {
        SaveSettings();
        UIManager.instance.CloseSettingsPanel(); // Tutup panel settings jika dari pause menu
    }
}
