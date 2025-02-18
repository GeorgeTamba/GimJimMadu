using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class LockManager : MonoBehaviour
    {
        public Animation lockAnim;
        public AnimationClip[] clips;
        public bool isLocked = true;
        public GameObject lockItem, rotatableItem, lockObject;
        public float angle, offset;
        public ChestScript treasureScript;
        public bool state, selected, firstselect;
        void Start()
        {
            lockItem.transform.localEulerAngles = new Vector3(0, 0, Random.Range(-80, 80));
            angle = rotatableItem.transform.localEulerAngles.z;
        }

        void Update()
        {
            if (!selected)
            {
                if (state)
                {
                    angle += offset * Time.deltaTime;
                }
                else
                {
                    angle -= offset * Time.deltaTime;
                }
                rotatableItem.transform.localEulerAngles = new Vector3(0, 0, angle);
                if (angle <= -80) state = true;
                if (angle >= 80) state = false;

            }
            if (Input.GetMouseButtonUp(0))
            {
                selected = true;
                float a = Mathf.Abs(rotatableItem.transform.localEulerAngles.z - lockItem.transform.localEulerAngles.z);
                if (a <= 19)
                {
                    if (!firstselect)
                    {
                        firstselect = true;
                        isLocked = false;
                        OpenTreasure();
                        lockObject.SetActive(false);
                    }
                }
                else
                {
                    lockAnim.clip = clips[Random.Range(0, 2)];
                    lockAnim.Play();
                    AudioManager.Instance.Play_PadlockTry();
                    Invoke("SetSelectable", 2f);
                }
            }
        }

        public void SetSelectable()
        {
            selected = false;
        }

        public void OpenTreasure()
        {
            this.enabled = false;
            treasureScript.collider.enabled = false;
            treasureScript.isOpened = true;
            AudioManager.Instance.Play_TreasureOpen();
            treasureScript.Cover.GetComponent<Animation>().Play("Chest_Cover_Open");
            if (treasureScript.itemInChest != null)
            {
                treasureScript.itemInChest.GetComponent<Collider>().enabled = true;
            };
        }
    }
}