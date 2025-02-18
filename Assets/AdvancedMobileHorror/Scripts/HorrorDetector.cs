using UnityEngine;
using System.Linq;

namespace AdvancedHorrorFPS
{
    public class HorrorDetector : MonoBehaviour
    {
        private float LastTimePlayerCheck = 0;
        private float LastPeriodPlayerCheck = 1;
        float Distance = 0;
        public float PlayerRange = 20;
        public float PlayerRangeForShake = 20;
        public AudioSource audioSourceBreathing;
        public AudioSource audioSourceShakingObjects;
        [HideInInspector]
        public GameObject[] shakableObjects;
        [HideInInspector]
        public GameObject[] flickableObjects;
        [HideInInspector]
        public float LastShakeTime = 0;
        [HideInInspector]
        public float LastFlickerTime = 0;

        void Update()
        {
            if (Time.time >= LastTimePlayerCheck + LastPeriodPlayerCheck)
            {
                LastTimePlayerCheck = Time.time;
                GameObject horrorItems = GameObject.FindObjectsOfType<HorrorItem>().OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).Select(x => x.gameObject).FirstOrDefault();
                if (horrorItems != null)
                {
                    Distance = Vector3.Distance(transform.position, horrorItems.transform.position);
                    MakeFeelForPlayer();
                    ShakeTheObjectsAroundPlayer();
                }
            }
        }

        public void MakeFeelForPlayer()
        {
            if (Distance <= PlayerRange)
            {
                float current = (PlayerRange - Distance) / PlayerRange;
                audioSourceBreathing.volume = current;
                audioSourceShakingObjects.volume = current;
            }
            else
            {
                audioSourceBreathing.volume = 0;
                audioSourceShakingObjects.volume = 0;
            }
        }

        public float ShakeTimePeriod = 4;
        public float FlickerTimePeriod = 1;
        public void ShakeTheObjectsAroundPlayer()
        {
            if (Distance <= PlayerRange / 1.5f)
            {
                if (Time.time >= LastShakeTime + ShakeTimePeriod)
                {
                    LastShakeTime = Time.time;
                    shakableObjects = Physics.OverlapSphere(HeroPlayerScript.Instance.transform.position, PlayerRangeForShake).Where(x => x.GetComponent<ShakableObject>() != null).Select(x => x.gameObject).ToArray();
                    for (int i = 0; i < shakableObjects.Length; i++)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            shakableObjects[i].GetComponent<ShakableObject>().Shake();
                        }
                    }
                }
            }
            if (Distance <= PlayerRange / 1.5f)
            {
                if (Time.time >= LastFlickerTime + FlickerTimePeriod)
                {
                    LastFlickerTime = Time.time;
                    flickableObjects = Physics.OverlapSphere(HeroPlayerScript.Instance.transform.position, PlayerRangeForShake).Where(x => x.GetComponent<FlickableObject>() != null).Select(x => x.gameObject).ToArray();
                    for (int i = 0; i < flickableObjects.Length; i++)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            flickableObjects[i].GetComponent<FlickableObject>().FlickerNow();
                        }
                    }
                }
            }
        }
    }
}