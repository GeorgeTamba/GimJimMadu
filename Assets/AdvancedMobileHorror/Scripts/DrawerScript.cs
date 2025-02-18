using System.Collections;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class DrawerScript : MonoBehaviour
    {
        public bool isOpened = false;
        public bool isLocked = false;
        public int KeyID_ToOpen = 0;
        private float LastTimeTry = 0;
        public GameObject itemInDrawer;
        void Start()
        {
            if (itemInDrawer != null)
            {
                itemInDrawer.GetComponent<Collider>().enabled = false;
            }
        }

        public void Interact()
        {
            if (isOpened == false)
            {
                if (Time.time > LastTimeTry + 1)
                {
                    LastTimeTry = Time.time;
                    if (isLocked)
                    {
                        if (HeroPlayerScript.Instance.Hand_Key.activeSelf)
                        {
                            if (HeroPlayerScript.Instance.GetCurrentKey() == KeyID_ToOpen)
                            {
                                isLocked = false;
                                InventoryManager.Instance.Remove_Item();
                                HeroPlayerScript.Instance.Hand_Key.SetActive(false);
                                AudioManager.Instance.Play_Door_UnLock();
                                isOpened = true;
                                AudioManager.Instance.Play_Audio_Drawer_Open();
                                BlinkEffect effect = GetComponentInChildren<BlinkEffect>();
                                effect.Disable();
                                GetComponent<Animation>().Play("DrawerOpen");
                                StartCoroutine(ActivateInsideCollider(effect));
                            }
                            else
                            {
                                AudioManager.Instance.Play_Door_TryOpen();
                                GameCanvas.Instance.Show_Warning("Locked! Find the key.");
                                return;
                            }
                        }
                        else
                        {
                            AudioManager.Instance.Play_Door_TryOpen();
                            GameCanvas.Instance.Show_Warning("Locked! Find the key.");
                            return;
                        }
                    }
                    else
                    {
                        isOpened = true;
                        AudioManager.Instance.Play_Audio_Drawer_Open();
                        BlinkEffect effect = GetComponentInChildren<BlinkEffect>();
                        effect.Disable();
                        GetComponent<Animation>().Play("DrawerOpen");
                        StartCoroutine(ActivateInsideCollider(effect));
                    }
                }
            }
            else
            {
                isOpened = false;
                AudioManager.Instance.Play_Audio_Drawer_Open();
                GetComponent<Animation>().Play("DrawerClose");
            }
        }

        IEnumerator ActivateInsideCollider(BlinkEffect effect)
        {
            yield return new WaitForSeconds(1);
            effect.enabled = false;
            if (itemInDrawer != null)
            {
                itemInDrawer.GetComponent<Collider>().enabled = true;
            }
        }
    }
}