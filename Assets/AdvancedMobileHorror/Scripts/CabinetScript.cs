using System.Collections;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class CabinetScript : MonoBehaviour
    {
        public bool isOpened = false;
        public GameObject itemInCabinet;
        void Start()
        {
            if (itemInCabinet != null)
            {
                itemInCabinet.GetComponent<Collider>().enabled = false;
            }
        }

        public void Open()
        {
            if (isOpened == false)
            {
                isOpened = true;
                AudioManager.Instance.Play_Audio_Cabinet_Open();
                BlinkEffect effect = GetComponentInChildren<BlinkEffect>();
                effect.Disable();
                GetComponent<Animation>().Play("Open");
                GetComponent<SphereCollider>().enabled = false;
                StartCoroutine(ActivateInsideCollider(effect));
                Destroy(GetComponent<ItemScript>());
                
            }
        }

        IEnumerator ActivateInsideCollider(BlinkEffect effect)
        {
            yield return new WaitForSeconds(1);
            effect.enabled = false;
            if (itemInCabinet != null)
            {
                itemInCabinet.GetComponent<Collider>().enabled = true;
            }
            Destroy(this);
        }
    }
}