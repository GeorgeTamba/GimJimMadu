using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedHorrorFPS
{
    public class SettingsScript : MonoBehaviour
    {
        public List<string> Difficulties;
        public Slider Slider_Mouse;
        public Dropdown DropDown_Difficulty;
        public Dropdown DropDown_Quality;
        public Toggle Toggle_SoundFX;

        void Start()
        {
            AddQualityOptions();
            AddDifficultyOptions();
            GetMouseSlider();
            GetMusicToggle();
        }

        public void AddQualityOptions()
        {
            DropDown_Quality.options.Clear();
            // De?er De?i?irse Event'ini atal?m.
            DropDown_Quality.onValueChanged.AddListener(delegate { SelectQualityLevel(DropDown_Quality); });

            // Quality Settings leri drop down a y?kleyelim.
            string[] names = QualitySettings.names;
            for (int i = 0; i < names.Length; i++)
            {
                DropDown_Quality.options.Add(new Dropdown.OptionData() { text = names[i] });
            }

            // Bakal?m daha ?nceden hi? quality settings ayarlam?? m?? Ayarlam??sa onu set edelim t?m projeye.
            if (PlayerPrefs.GetInt("QualitySetting", -1) != -1)
            {
                // Demekki daha ?nceden bir ayar se?mi?. Onu y?kleyelim!
                QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualitySetting"), true);
                DropDown_Quality.value = PlayerPrefs.GetInt("QualitySetting", 0);
                DropDown_Quality.RefreshShownValue();
            }
            // Yok daha ?nce ayarlamam??. O zaman Proje'de ?uan neyse, drop down'a onu set edelim.
            else
            {
                DropDown_Quality.value = QualitySettings.GetQualityLevel();
                DropDown_Quality.RefreshShownValue();
            }
        }

        void SelectQualityLevel(Dropdown dropdown)
        {
            PlayerPrefs.SetInt("QualitySetting", dropdown.value);
            QualitySettings.SetQualityLevel(dropdown.value, true);
        }

        public void GetMouseSlider()
        {
            Slider_Mouse.value = PlayerPrefs.GetFloat("MouseSensivity", 1);
            Slider_Mouse.onValueChanged.AddListener(delegate { ChangeMouse(Slider_Mouse); });
        }

        public void GetMusicToggle()
        {
            Toggle_SoundFX.isOn = (PlayerPrefs.GetInt("Music", 1) == 1 ? true : false);
            Toggle_SoundFX.onValueChanged.AddListener(delegate { ChangeMusic(Toggle_SoundFX); });
        }

        public void ChangeMusic(Toggle toggle)
        {
            PlayerPrefs.SetInt("Music", (toggle.isOn == true ? 1 : 0));
            if (toggle.isOn)
            {
                AudioListener.volume = 1;
            }
            else
            {
                AudioListener.volume = 0;
            }
        }

        public void ChangeMouse(Slider slider)
        {
            PlayerPrefs.SetFloat("MouseSensivity", slider.value);
        }

        public void AddDifficultyOptions()
        {
            for (int i = 0; i < Difficulties.Count; i++)
            {
                DropDown_Difficulty.options.Add(new Dropdown.OptionData() { text = Difficulties[i] });
            }

            DropDown_Difficulty.value = PlayerPrefs.GetInt("Difficulty", 1);
            DropDown_Difficulty.RefreshShownValue();


            DropDown_Difficulty.onValueChanged.AddListener(delegate { SelectDifficulty(DropDown_Difficulty); });
            DropDown_Difficulty.value = PlayerPrefs.GetInt("Difficulty", 1) == 0 ? 0 : PlayerPrefs.GetInt("Difficulty", 1);
        }

        void SelectDifficulty(Dropdown dropdown)
        {
            Debug.Log(dropdown.value);
            PlayerPrefs.SetInt("Difficulty", dropdown.value);
        }
    }
}