using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class PistolAmmoScript : MonoBehaviour
    {
        public int AmmoCount = 7;
        public bool isGrabbed = false;


        public void GrabIt()
        {
            if(!isGrabbed)
            {
                PistolScript.Instance.AmmoTotal = PistolScript.Instance.AmmoTotal + AmmoCount;
                AudioManager.Instance.Play_Item_Grab();
                isGrabbed = true;
                GameCanvas.Instance.Update_Text_Ammo(PistolScript.Instance.AmmoInMag, PistolScript.Instance.AmmoTotal);
                if(PistolScript.Instance.AmmoInMag <= 0)
                {
                    // Reload automatically!
                    PistolScript.Instance.CheckAmmoNow();
                }
                Destroy(gameObject, 0.25f);
            }
        }
    }
}