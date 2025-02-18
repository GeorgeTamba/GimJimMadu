using System.Collections;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class FlashLightScript : MonoBehaviour
    {
        public static FlashLightScript Instance;
        public bool isGrabbed = false;
        public Light Light;
        public float BlueBattery = 100;
        public float DamageRate = 0.25f;
        public float BatterySpendNumber = 1;
        RaycastHit hit;
        public AudioSource audioSource;
        public Transform aimPoint;
        public LayerMask layerMask;
        private bool isOn = false;

        void Awake()
        {
            Instance = this;
        }
        public void FlashLight_Decision(bool decision)
        {
            Light.enabled = decision;
            if (AdvancedGameManager.Instance.blueUVLightAttack)
            {
                GameCanvas.Instance.Indicator_BlueLight.SetActive(decision);
            }
        }

        private void Start()
        {
            Light = GetComponent<Light>();
        }

       

        public void Grabbed()
        {
            if (AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
            {
                GameCanvas.Instance.Activate_FlashLightButton();
            }
            else
            {
                GameCanvas.Instance.ShowHint("Press F for Flashlight!");
            }
            HeroPlayerScript.Instance.FPSHands.SetActive(true);
            if (!HeroPlayerScript.Instance.Hand_Pistol.activeInHierarchy && !HeroPlayerScript.Instance.Hand_Baseball.activeInHierarchy)
            {
                HeroPlayerScript.Instance.Hand_FlashLight.SetActive(true);
            }
            isGrabbed = true;
        }
        private void Update()
        {
            if (!isGrabbed) return;
            if (InventoryManager.Instance.isInventoryOpened) return;

            if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
            {
                if (Input.GetKeyUp(KeyCode.F))
                {
                    if(!isOn)
                    {
                        isOn = true;
                        FlashLightScript.Instance.FlashLight_Decision(true);
                        AudioManager.Instance.Play_Flashlight_Open();
                    }
                    else
                    {
                        isOn = false;
                        FlashLightScript.Instance.FlashLight_Decision(false);
                    }
                    AudioManager.Instance.Play_Flashlight_Open();
                }
                if (AdvancedGameManager.Instance.blueUVLightAttack)
                {
                    if (Input.GetMouseButtonDown(1) && AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
                    {
                        GameCanvas.Instance.FlashLight_BlueEffect_Down();
                    }
                    else if (Input.GetMouseButtonUp(1) && AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
                    {
                        GameCanvas.Instance.FlashLight_BlueEffect_Up();
                    }
                }
            }
        }

        public void PlayAudioBlueLight()
        {
            audioSource.Play();
        }

        public void StopAudioBlueLight()
        {
            audioSource.Stop();
        }

        void LateUpdate()
        {
            if (!isGrabbed) return;

            if (GameCanvas.Instance.isFlashBlueNow && BlueBattery > 0)
            {
                BlueBattery = BlueBattery - Time.deltaTime * BatterySpendNumber * 2;
                if (!audioSource.isPlaying)
                {
                    PlayAudioBlueLight();
                }
                var directionLeft2 = Quaternion.AngleAxis(20, aimPoint.transform.right * -1) * Vector3.forward;
                var directionLeft = Quaternion.AngleAxis(10, aimPoint.transform.right * -1) * Vector3.forward;
                var directionForward = aimPoint.TransformDirection(Vector3.forward);
                var directionRight = Quaternion.AngleAxis(10, aimPoint.transform.right) * Vector3.forward;
                var directionRight2 = Quaternion.AngleAxis(20, aimPoint.transform.right) * Vector3.forward;
                if (Physics.Raycast(aimPoint.position, directionLeft2, out hit, 5, layerMask))
                {
                    hit.transform.GetComponent<DemonScript>().GetDamageByFlashlight(DamageRate);
                }
                else if (Physics.Raycast(aimPoint.position, directionLeft, out hit, 5, layerMask))
                {
                    hit.transform.GetComponent<DemonScript>().GetDamageByFlashlight(DamageRate);
                }
                else if (Physics.Raycast(aimPoint.position, directionForward, out hit, 5, layerMask))
                {
                    hit.transform.GetComponent<DemonScript>().GetDamageByFlashlight(DamageRate);
                }
                else if (Physics.Raycast(aimPoint.position, directionRight, out hit, 5, layerMask))
                {
                    hit.transform.GetComponent<DemonScript>().GetDamageByFlashlight(DamageRate);
                }
                else if (Physics.Raycast(aimPoint.position, directionRight2, out hit, 5, layerMask))
                {
                    hit.transform.GetComponent<DemonScript>().GetDamageByFlashlight(DamageRate);
                }
            }
            else if (BlueBattery < 100)
            {
                BlueBattery = BlueBattery + Time.deltaTime * BatterySpendNumber;
                if (BlueBattery > 100)
                {
                    BlueBattery = 100;
                }
            }
            if (BlueBattery <= 0)
            {
                GameCanvas.Instance.FlashLight_BlueEffect_Up();
                StopAudioBlueLight();
            }
            GameCanvas.Instance.Image_BlueLight.fillAmount = (BlueBattery / 100);
        }
    }
}