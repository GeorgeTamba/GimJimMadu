using System.Collections;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class BlinkEffect : MonoBehaviour
    {
        public Color startColor = Color.green;
        public Color endColor = Color.black;
        [Range(0, 10)]
        public float speed = 1;
        Material a;
        Renderer ren;

        void Awake()
        {
            ren = GetComponentInChildren<Renderer>();
            a = ren.material;
            a.EnableKeyword("_EMISSION");
        }

        private void Start()
        {
            if(!AdvancedGameManager.Instance.blinkOnInteractableObjects)
            {
                this.enabled = false;
            }
        }

        public void Disable()
        {
            speed = 0;
            StartCoroutine(DisableNow());
        }

        IEnumerator DisableNow()
        {
            yield return new WaitForSeconds(0.5f);
            this.enabled = false;
        }

        void Update()
        {
            a.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1)));
        }
    }
}
