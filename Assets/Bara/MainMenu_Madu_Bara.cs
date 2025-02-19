using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Madu_Bara : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level_2"); // Ganti dengan nama scene permainanmu
    }

    public void OpenSettings()
    {
        PlayerPrefs.SetInt("LastScene", 1);
        SceneManager.LoadScene("Setting"); // Ganti dengan nama scene settings
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
