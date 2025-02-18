using System.Collections.Generic;
using UnityEngine;


namespace AdvancedHorrorFPS
{
    public class EnemySpawnerScript : MonoBehaviour
    {
        public List<Transform> Points_Demon;
        public GameObject DemonPrefab;
        public int MaxDemonCountAlive = 3;
        [HideInInspector]
        public List<GameObject> Demons;
        private float LastCheckTime = 0;
        public float EnemyBornPeriod = 0;
        public bool canSpawn = true;
        private Transform randomPosition;
        public static EnemySpawnerScript Instance;
        public EnemyType EnemyTypeToSpawn;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < Points_Demon.Count; i++)
            {
                Points_Demon[i].gameObject.SetActive(false);
            }
        }


        void Update()
        {
            if (!canSpawn) return;
            if (Time.time > LastCheckTime + EnemyBornPeriod)
            {
                LastCheckTime = Time.time;
                ResetDemonList();
                if (Demons.Count < MaxDemonCountAlive)
                {
                    randomPosition = EnemySpawnerScript.Instance.Points_Demon[Random.Range(0, EnemySpawnerScript.Instance.Points_Demon.Count)];
                    GameObject newDemon = Instantiate(DemonPrefab, randomPosition.position, Quaternion.identity);
                    newDemon.GetComponent<DemonScript>().enemyType = EnemyTypeToSpawn;
                    Demons.Add(newDemon);
                }
            }
        }

        public void ResetDemonList()
        {
            for (int i = 0; i < Demons.Count; i++)
            {
                if (Demons[i] == null)
                {
                    Demons.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}