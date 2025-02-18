using System.Collections;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class FlickableObject : MonoBehaviour
    {
        public bool isFlickering = false;
        private float timeDelay;
        private Light light;
        private ParticleSystem particle;
        private bool FlickIt = false;
        void Start()
        {
            light = GetComponent<Light>();
            if (light != null)
            {
                light.range = light.range / 1.5f;
            }
            particle = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (FlickIt)
            {
                if (isFlickering == false)
                {
                    StartCoroutine(FlickeringLight());
                }
            }
        }

        public void FlickerNow()
        {
            StartCoroutine(Basla());
        }

        IEnumerator Basla()
        {
            FlickIt = true;
            yield return new WaitForSeconds(1);
            FlickIt = false;
        }

        IEnumerator FlickeringLight()
        {
            isFlickering = true;
            if (light != null)
            {
                light.enabled = false;
            }
            else
            {
                particle.Stop();
            }
            timeDelay = Random.Range(0.01f, 0.2f);
            yield return new WaitForSeconds(timeDelay);
            if (light != null)
            {
                light.enabled = true;
            }
            else
            {
                particle.Play();
            }
            timeDelay = Random.Range(0.01f, 0.2f);
            yield return new WaitForSeconds(timeDelay);
            isFlickering = false;
        }
    }
}