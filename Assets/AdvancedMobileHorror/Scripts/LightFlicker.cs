using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class LightFlicker : MonoBehaviour
    {
        private bool isFlicking;
        public Light light;
        public MeshRenderer meshRenderer;
        private float startIntesity;
        private float waitTime;
        private float flickerCount;
        private float delayBetweenFlickers;

        // Start is called before the first frame update
        void Start()
        {
            startIntesity = light.intensity;
            delayBetweenFlickers = Random.Range(0, 0.1f);
            waitTime = Random.Range(2, 5);
            flickerCount = Random.Range(3, 8);
            isFlicking = true;
            StartCoroutine(Flicker());
        }

        IEnumerator Flicker()
        {
            yield return new WaitForSeconds(waitTime);
            flickerCount = Random.Range(3, 8);
            waitTime = Random.Range(2, 5);
            light.enabled = true;
            for (int i = 0; i < flickerCount; i++)
            {
                var randomFlickerTimer = 0f;
                randomFlickerTimer = Random.Range(delayBetweenFlickers - delayBetweenFlickers / 2,
                delayBetweenFlickers + delayBetweenFlickers / 2);
                yield return new WaitForSeconds(randomFlickerTimer);
                light.intensity = 0;
                Material[] materials = meshRenderer.materials;
                Material m = materials[0];
                m.SetColor("_EmissionColor", Color.black);
                meshRenderer.materials = materials;
                yield return new WaitForSeconds(randomFlickerTimer);
                light.intensity = startIntesity;
                Material[] materials2 = meshRenderer.materials;
                Material m2 = materials2[0];
                m2.SetColor("_EmissionColor", Color.white);
                meshRenderer.materials = materials2;
            }
            if (isFlicking)
                StartCoroutine(Flicker());
            else
            {
                light.intensity = startIntesity;
            }
        }
    }
}