using System.Collections;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class PistolScript : MonoBehaviour
    {
        public static PistolScript Instance;
        public int AmmoTotal = 7;
        public int AmmoInMag = 7;
        private int AmmoCapacity = 0;
        public int Damage = 40;
        public bool isFiring = false;
        public bool isGrabbed = false;
        public ParticleSystem fireParticle;


        private void Awake()
        {
            Instance = this;
        }

        public void Fire()
        {
            if (isFiring) return;
            if (InventoryManager.Instance.isInventoryOpened) return;
            if (AmmoInMag > 0)
            {
                AmmoInMag--;
                GameCanvas.Instance.Update_Text_Ammo(AmmoInMag, AmmoTotal);
                isFiring = true;
                //Firing Animation
                HeroPlayerScript.Instance.Hand_Pistol.GetComponent<Animation>().Play("Firing");
                CameraLook.Instance.fCamShakeImpulse = 0.3f;
                AudioManager.Instance.Play_Audio_PistolFire();
                fireParticle.Play();
                CheckTheTarget();
                StartCoroutine(CheckAmmo(0.75f));
            }
            else
            {
                AudioManager.Instance.Play_Audio_PistolEmpty();
                isFiring = false;
            }
        }

        private void Update()
        {
            if (!isGrabbed) return;
            if (HeroPlayerScript.Instance.isHiding) return;

            if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
            {
                if (Input.GetMouseButtonUp(0) && !HeroPlayerScript.Instance.GetHeroBusy())
                {
                    Fire();
                }
                if(Input.GetMouseButtonUp(0) && HeroPlayerScript.Instance.GetHeroBusy())
                {
                    HeroPlayerScript.Instance.SetHeroBusy(false);
                }
            }
        }

        public void CheckTheTarget()
        {
            Ray ray = Camera.main.ScreenPointToRay(GameCanvas.Instance.Crosshair.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.GetComponent<DemonScript>() != null)
                {
                    hit.collider.GetComponent<DemonScript>().GetDamageByPistolOrBaseBallStick(Damage);
                }
            }
        }

        public void Grabbed(bool isGrabbedBefore)
        {
            if (isGrabbed)
            {
                // Add Extra Ammo! Player has already Pistol.
                if(!isGrabbedBefore)
                {
                    PistolScript.Instance.AmmoTotal = PistolScript.Instance.AmmoTotal + AmmoCapacity;
                }
                AudioManager.Instance.Play_Item_Grab();
                isGrabbed = true;
                GameCanvas.Instance.Update_Text_Ammo(PistolScript.Instance.AmmoInMag, PistolScript.Instance.AmmoTotal);
                if (PistolScript.Instance.AmmoInMag <= 0)
                {
                    // Reload automatically!
                    PistolScript.Instance.CheckAmmoNow();
                }
            }
            else
            {
                if (AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
                {
                    GameCanvas.Instance.Activate_FiringButton();
                }
                HeroPlayerScript.Instance.FPSHands.SetActive(true);
                HeroPlayerScript.Instance.Hand_Pistol.SetActive(true);
                HeroPlayerScript.Instance.Hand_FlashLight.SetActive(false);
                HeroPlayerScript.Instance.Hand_Key.SetActive(false);
                HeroPlayerScript.Instance.Hand_Baseball.SetActive(false);
                GameCanvas.Instance.Switch_HitButtonToPistol();
                GameCanvas.Instance.Activate_PistolPanel();
            }
            isGrabbed = true;
        }

       
        private void Start()
        {
            AmmoCapacity = AmmoInMag;
        }

        public void CheckAmmoNow()
        {
            StartCoroutine(CheckAmmo(0));
        }

        public IEnumerator CheckAmmo(float time)
        {
            isFiring = true;
            yield return new WaitForSeconds(time);
            if (AmmoInMag <= 0)
            {
                if (AmmoTotal > 0)
                {
                    // Doldurma Animasyonu
                    HeroPlayerScript.Instance.Hand_Pistol.GetComponent<Animation>().Play("Reloading");
                    if (AmmoTotal > 7)
                    {
                        AmmoInMag = 7;
                        AmmoTotal = AmmoTotal - 7;
                        GameCanvas.Instance.Update_Text_Ammo(AmmoInMag, AmmoTotal);
                    }
                    else
                    {
                        AmmoInMag = AmmoTotal;
                        AmmoTotal = 0;
                        GameCanvas.Instance.Update_Text_Ammo(AmmoInMag, AmmoTotal);
                    }
                    yield return new WaitForSeconds(0.4f);
                    AudioManager.Instance.Play_Audio_Reload();
                    yield return new WaitForSeconds(1.1f);
                    isFiring = false;
                }
                else
                {
                    isFiring = false;
                }
            }
            else
            {
                isFiring = false;
            }
        }
    }
}