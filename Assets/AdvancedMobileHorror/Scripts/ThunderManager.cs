using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class ThunderManager : MonoBehaviour
    {
        Light testLight;
        public float anonymousWaitTime;
        public float maxWaitTime;
        public float LastThunderTime = 0;
        public float ThunderPeriod;
        public float ThunderPeriodMin = 5;
        public float ThunderPeriodMax = 10;
        public AudioClip[] thunderSounds;
        public AudioSource audioSource;
        private int count = 0;

        void Start()
        {
            testLight = GetComponent<Light>();
            ThunderPeriod = Random.Range(ThunderPeriodMin, ThunderPeriodMax);
            ThunderPeriodMin = 10;
            ThunderPeriodMax = 30;
        }

        IEnumerator Flashing()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(anonymousWaitTime, maxWaitTime));
                testLight.enabled = !testLight.enabled;
                count--;
                if (count == 0)
                {
                    testLight.enabled = false;
                    break;
                }
            }
        }

        void Update()
        {
            if (Time.time > LastThunderTime + ThunderPeriod)
            {
                LastThunderTime = Time.time;
                count = Random.Range(3, 8);
                StartCoroutine(Flashing());
                audioSource.clip = thunderSounds[Random.Range(0, thunderSounds.Length)];
                audioSource.Play();
                ThunderPeriod = Random.Range(ThunderPeriodMin, ThunderPeriodMax);
            }
        }

        public void DecreaseVolume()
        {
            audioSource.volume = 0.15f;
            ThunderPeriodMin = 30;
            ThunderPeriodMax = 60;
        }

        public void IncreaseVolume()
        {
            audioSource.volume = 1f;
            ThunderPeriodMin = 10;
            ThunderPeriodMax = 30;
        }
    }
}