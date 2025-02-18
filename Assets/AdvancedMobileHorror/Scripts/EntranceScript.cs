using System.Collections;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class EntranceScript : MonoBehaviour
    {
        public EffectType type;
        public int Possibility = 1;
        public bool destroyAfterAction = true;
        public bool hasPlayerShock = false;
        public AudioClip Audio_SoundEffect;
        public GameObject[] objectsToAct;
        public string AnimationNameForMove = "MoveObject";
        public float Range = 3;
        public float ExplosionPower = 500;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && Random.Range(0, Possibility) == 0)
            {
                GetComponent<BoxCollider>().enabled = false;
                if (destroyAfterAction)
                {
                    Destroy(gameObject, 10);
                }
                if (type == EffectType.Animation)
                {
                    StartCoroutine(AnimateObject());
                }
                else if (type == EffectType.ObjectFall)
                {
                    StartCoroutine(FallObject());
                }
                else if (type == EffectType.Explosion)
                {
                    StartCoroutine(ExplodeObject());
                }
                CameraLook.Instance.fCamShakeImpulse = 1;
            }
        }

        IEnumerator AnimateObject()
        {
            for (int i = 0; i < objectsToAct.Length; i++)
            {
                objectsToAct[i].GetComponent<Animation>().Play(AnimationNameForMove);
            }
            AudioManager.Instance.audioSource.PlayOneShot(Audio_SoundEffect);
            yield return new WaitForSeconds(0f);
            if (hasPlayerShock)
            {
                AudioManager.Instance.Play_PlayerShock();
            }
        }

        IEnumerator FallObject()
        {
            for (int i = 0; i < objectsToAct.Length; i++)
            {
                objectsToAct[i].GetComponent<Rigidbody>().isKinematic = false;
                objectsToAct[i].GetComponent<Collider>().isTrigger = false;
                objectsToAct[i].GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90)) * 10, ForceMode.Impulse);
            }
            AudioManager.Instance.audioSource.PlayOneShot(Audio_SoundEffect);
            yield return new WaitForSeconds(0f);
            if (hasPlayerShock)
            {
                AudioManager.Instance.Play_PlayerShock();
            }
        }

        IEnumerator ExplodeObject()
        {
            for (int i = 0; i < objectsToAct.Length; i++)
            {
                objectsToAct[i].GetComponent<Rigidbody>().isKinematic = false;
                objectsToAct[i].GetComponent<Rigidbody>().useGravity = true;
                objectsToAct[i].GetComponent<Rigidbody>().AddExplosionForce(ExplosionPower, new Vector3(Random.Range(objectsToAct[0].transform.position.x - 1, objectsToAct[0].transform.position.x + 1), Random.Range(objectsToAct[0].transform.position.y - 1, objectsToAct[0].transform.position.y), Random.Range(objectsToAct[0].transform.position.z - 1, objectsToAct[0].transform.position.z + 1)), Range);
            }
            AudioManager.Instance.audioSource.PlayOneShot(Audio_SoundEffect);
            yield return new WaitForSeconds(0f);
            if (hasPlayerShock)
            {
                AudioManager.Instance.Play_PlayerShock();
            }
        }
    }

    public enum EffectType
    {
        Animation,
        ObjectFall,
        Explosion
    }
}