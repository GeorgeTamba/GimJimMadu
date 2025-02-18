using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class CameraLook : MonoBehaviour
    {
        public Transform viewTarget;
        public float distanceSpeed = 150.0f;
        public float collisionOffset = 0.3f;
        public float height = 1.5f;
        public float horizontalRotationSpeed = 250.0f;
        public float verticalRotationSpeed = 150.0f;
        public float rotationDampening = 0.75f;
        public float minVerticalAngle = -60.0f;
        public float maxVerticalAngle = 60.0f;
        public bool useRMBToAim = false;

        private float h, v;
        private Vector3 newPosition;
        private Quaternion newRotation, smoothRotation;
        private Transform cameraTransform;
        public float fCamShakeImpulse = 0.0f;

        public Camera camera;
        public static CameraLook Instance;
        void Start()
        {
            Initialize();
        }

        private void Awake()
        {
            Instance = this;
        }

        void Initialize()
        {
            h = this.transform.eulerAngles.x;
            v = this.transform.eulerAngles.y;

            cameraTransform = this.transform;
            camera = GetComponent<Camera>();
        }


        void LateUpdate()
        {
            if (!viewTarget)
                return;

            if(AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
            {
                h += Touchpad.Instance.HorizontalValue * horizontalRotationSpeed * Time.deltaTime * PlayerPrefs.GetFloat("MouseSensivity", 1);
                v -= Touchpad.Instance.VerticalValue * verticalRotationSpeed * Time.deltaTime * PlayerPrefs.GetFloat("MouseSensivity", 1);
            }
            else
            {
                h += Input.GetAxis("Mouse X") * horizontalRotationSpeed * PlayerPrefs.GetFloat("MouseSensivity", 1);
                v -= Input.GetAxis("Mouse Y") * verticalRotationSpeed * PlayerPrefs.GetFloat("MouseSensivity", 1);
            }

            h = ClampAngle(h, -360.0f, 360.0f);
            v = ClampAngle(v, minVerticalAngle, maxVerticalAngle);

            newRotation = Quaternion.Euler(v, h, 0.0f);

            smoothRotation = Quaternion.Slerp(smoothRotation, newRotation, TimeSignature((1 / rotationDampening) * 100.0f));

            newPosition = viewTarget.position;

            smoothRotation.eulerAngles = new Vector3(smoothRotation.eulerAngles.x, smoothRotation.eulerAngles.y, 0.0f);

            cameraTransform.position = newPosition;
            cameraTransform.rotation = smoothRotation;

            if (fCamShakeImpulse > 0.0f)
                shakeCamera();
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;

            if (angle > 360)
                angle -= 360;

            return Mathf.Clamp(angle, min, max);
        }

        private float TimeSignature(float speed)
        {
            return 1.0f / (1.0f + 80.0f * Mathf.Exp(-speed * 0.02f));
        }

        public void shakeCamera()
        {
            Camera.main.transform.position += new Vector3(Random.Range(-fCamShakeImpulse, fCamShakeImpulse) / 4, Random.Range(-fCamShakeImpulse, fCamShakeImpulse) / 4, Random.Range(-fCamShakeImpulse, fCamShakeImpulse) / 4);
            fCamShakeImpulse -= Time.deltaTime * fCamShakeImpulse * 4.0f;
            if (fCamShakeImpulse < 0.01f)
            {
                fCamShakeImpulse = 0.0f;
            }
        }
    }
}