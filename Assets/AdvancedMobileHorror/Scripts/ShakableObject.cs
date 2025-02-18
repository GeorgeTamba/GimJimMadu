using UnityEngine;
namespace AdvancedHorrorFPS
{
    public class ShakableObject : MonoBehaviour
    {
        private float speed = 12f;
        private float amount = 0;
        Vector3 startingPos;

        private void Awake()
        {
            startingPos.x = transform.position.x;
            startingPos.y = transform.position.y;
            startingPos.z = transform.position.z;
        }

        void Update()
        {
            if (Mathf.Abs(amount) > 0)
            {
                gameObject.transform.position = new Vector3(startingPos.x + Mathf.Sin(Time.time * speed) * amount, startingPos.y + Mathf.Sin(Time.time * speed) * amount / 2, startingPos.z + (Mathf.Sin(Time.time * speed) * amount));
                amount = Mathf.Lerp(amount, 0, 0.025f);
            }
        }

        public void Shake()
        {
            if (Random.Range(0, 2) == 0)
            {
                amount = 0.1f;
            }
            else
            {
                amount = -0.1f;
            }
        }
    }
}