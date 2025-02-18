using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class SaverScript : MonoBehaviour
    {
        public int SavingIndex = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (PlayerPrefs.GetInt("SavingIndex", -1) < SavingIndex)
            {
                if (other.CompareTag("Player"))
                {
                    PlayerPrefs.SetInt("SavingIndex", SavingIndex);
                    PlayerPrefs.Save();
                }
            }
        }
    }
}