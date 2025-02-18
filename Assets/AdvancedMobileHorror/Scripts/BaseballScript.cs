using System.Collections;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class BaseballScript : MonoBehaviour
    {
        public static BaseballScript Instance;
        public int Damage = 40;
        public bool isHitting = false;
        public bool isGrabbed = false;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {

        }

        void Update()
        {
            if (!isGrabbed) return;
            if (HeroPlayerScript.Instance.isHiding) return;

            if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
            {
                if (Input.GetMouseButtonUp(0) && !HeroPlayerScript.Instance.GetHeroBusy())
                {
                    Hit();
                }
                if (Input.GetMouseButtonUp(0) && HeroPlayerScript.Instance.GetHeroBusy())
                {
                    HeroPlayerScript.Instance.SetHeroBusy(false);
                }
            }
        }

        public void Hit()
        {
            if (isHitting) return;
            if (InventoryManager.Instance.isInventoryOpened) return;
            isHitting = true;
            //Firing Animation
            HeroPlayerScript.Instance.Hand_Baseball.GetComponent<Animation>().Play("Hitting");
            StartCoroutine(ReleaseHit(1.5f));
        }

        public IEnumerator ReleaseHit(float time)
        {
            yield return new WaitForSeconds(0.5f);
            CheckTheTarget();
            AudioManager.Instance.Play_Audio_BaseBallHit();
            CameraLook.Instance.fCamShakeImpulse = 0.25f;
            yield return new WaitForSeconds(1f);
            isHitting = false;
        }

        public void CheckTheTarget()
        {
            Ray ray = Camera.main.ScreenPointToRay(GameCanvas.Instance.Crosshair.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 3))
            {
                if (hit.collider.GetComponent<DemonScript>() != null)
                {
                    hit.collider.GetComponent<DemonScript>().GetDamageByPistolOrBaseBallStick(Damage);
                }
            }
        }

        public void Grabbed()
        {
            if (AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
            {
                GameCanvas.Instance.Switch_HitButtonToBaseball();
                GameCanvas.Instance.Activate_FiringButton();
            }
            HeroPlayerScript.Instance.FPSHands.SetActive(true);
            HeroPlayerScript.Instance.Hand_Baseball.SetActive(true);
            HeroPlayerScript.Instance.Hand_FlashLight.SetActive(false);
            HeroPlayerScript.Instance.Hand_Key.SetActive(false);
            HeroPlayerScript.Instance.Hand_Pistol.SetActive(false);
            GameCanvas.Instance.Panel_Ammo.SetActive(false);
            isGrabbed = true;

        }
    }
}
