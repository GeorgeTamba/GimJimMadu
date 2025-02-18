using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class FPSHandRotator : MonoBehaviour
    {
        public Transform target;
        public float speed = 2.5f;
        public Transform positionTarget;
        public static FPSHandRotator Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void LateUpdate()
        {
            Vector3 dir = target.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);
            transform.position = positionTarget.position;
        }
    }
}
