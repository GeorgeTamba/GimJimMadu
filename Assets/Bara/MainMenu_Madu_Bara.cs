using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Madu_Bara : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Ganti dengan nama scene permainanmu
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingScene"); // Ganti dengan nama scene settings
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
