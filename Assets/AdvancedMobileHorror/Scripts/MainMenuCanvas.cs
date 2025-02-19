using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace AdvancedHorrorFPS
{
    public class MainMenuCanvas : MonoBehaviour
    {
        public GameObject Panel_MainMenu;
        public GameObject Panel_Settings;
        public GameObject ButtonStart;

        private void Start()
        {
            Time.timeScale = 1;
        }

        public void Click_Exit()
        {
            Application.Quit();
        }

        public void Click_PlayGame()
        {
            Panel_MainMenu.SetActive(false);
            SceneManager.LoadScene(1);
        }


        public void Click_Settings()
        {
            Panel_Settings.SetActive(true);
            Panel_MainMenu.SetActive(false);
        }

        public void Click_Close_Settings()
        {
            Panel_Settings.SetActive(false);
            Panel_MainMenu.SetActive(true);
        }
    }
}