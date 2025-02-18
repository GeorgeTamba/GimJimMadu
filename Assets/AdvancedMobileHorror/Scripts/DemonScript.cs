using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AdvancedHorrorFPS
{
    public class DemonScript : MonoBehaviour
    {
        public EnemyType enemyType;
        public RuntimeAnimatorController crawlingAnimatorController;
        public RuntimeAnimatorController walkingAnimatorController;
        public NavMeshAgent agent;
        public Animator animator;
        public float health = 100;
        public float ChaseRange = 15;
        public float AttackRange = 3;
        public int AttackDamage = 20;
        private float BehoviourPeriod = 0.5f;
        private float LastAttackTime = 0;
        public float IdlePeriod = 5;
        public float AttackPeriod = 5;
        private float LastBehoviourTime = -10;
        private float LastIdleTime = 0;
        public ParticleSystem[] burningParticles;
        public BehoviourType currentStatus;
        public Transform target;
        public float Distance;
        public Transform targetLocation;
        public float InitSpeed = 0;
        public AudioClip[] Audio_Pains;
        public AudioClip[] Audio_Dies;
        public AudioClip[] Audio_Hits;
        public AudioSource audioSource;
        public AudioSource audioSourceRoar;
        public bool attacked = false;
        public AudioClip[] audios_Roars;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
            IdlePeriod = UnityEngine.Random.Range(5, 10);
            currentStatus = BehoviourType.Idle;
            InitSpeed = agent.speed;
            InvokeRepeating("Roar", 3, 5);


            if(enemyType == EnemyType.Random)
            {
                if(Random.Range(0,2) == 0)
                {
                    enemyType = EnemyType.Crawling;
                }
                else
                {
                    enemyType = EnemyType.Walking;
                }
            }

            if(enemyType == EnemyType.Crawling)
            {
                animator.runtimeAnimatorController = crawlingAnimatorController;
            }
            else
            {
                animator.runtimeAnimatorController = walkingAnimatorController;
                GetComponent<CapsuleCollider>().direction = 1;
                GetComponent<CapsuleCollider>().center = new Vector3(0, 0.8f, 0);
            }

            switch(AdvancedGameManager.Instance.Difficulty)
            {
                case 0:
                    health = health - 20;
                    ChaseRange = ChaseRange / 1.5f;
                    AttackDamage = AttackDamage / 2;
                    agent.speed = agent.speed - 0.2f;
                    break;
                case 2:
                    health = health + 20;
                    AttackDamage = AttackDamage * 2;
                    ChaseRange = ChaseRange * 1.5f;
                    agent.speed = agent.speed + 0.2f;
                    break;
            }


        }

        public void Roar()
        {
            if (currentStatus == BehoviourType.Chasing || currentStatus == BehoviourType.Idle)
            {
                audioSourceRoar.clip = audios_Roars[UnityEngine.Random.Range(0, audios_Roars.Length)];
                audioSourceRoar.Play();
            }
        }

        void Update()
        {
            if (currentStatus == BehoviourType.Dead) return;
            animator.SetFloat("locomotion", agent.velocity.magnitude);

            if(currentStatus == BehoviourType.GettingHit && Time.time > LastTimeFireDamage + 1f)
            {
                FinishedGetDamage();
            }

            if(currentStatus == BehoviourType.GettingHit)
            {
                return;
            }

            if (Time.time >= LastBehoviourTime + BehoviourPeriod)
            {
                LastBehoviourTime = Time.time;
                Distance = Vector3.Distance(transform.position, HeroPlayerScript.Instance.transform.position);
                if (Distance <= ChaseRange)
                {
                    if(!HeroPlayerScript.Instance.isHiding)
                    {
                        target = HeroPlayerScript.Instance.transform;
                    }
                    else if (HeroPlayerScript.Instance.isHiding)
                    {
                        if(Distance < AttackRange)
                        {
                            target = HeroPlayerScript.Instance.transform;
                        }
                        else
                        {
                            if (target != null)
                            {
                                currentStatus = BehoviourType.Idle;
                            }
                            target = null;
                        }
                    }
                }
                else
                {
                    if(target != null)
                    {
                        currentStatus = BehoviourType.Idle;
                    }
                    target = null;
                }

                if (target == null && currentStatus == BehoviourType.Idle)
                {
                    if (Time.time >= LastIdleTime + IdlePeriod)
                    {
                        int decision = UnityEngine.Random.Range(0, 2);
                        if (decision == 0)
                        {
                            agent.isStopped = true;
                            currentStatus = BehoviourType.Idle;
                            IdlePeriod = UnityEngine.Random.Range(5, 10);
                            LastIdleTime = Time.time;
                        }
                        else if (decision == 1)
                        {
                            targetLocation = EnemySpawnerScript.Instance.Points_Demon[UnityEngine.Random.Range(0, EnemySpawnerScript.Instance.Points_Demon.Count)];
                            agent.isStopped = false;
                            currentStatus = BehoviourType.Patrolling;
                            agent.SetDestination(targetLocation.position);
                        }
                    }
                }
                else if (target == null && currentStatus == BehoviourType.Patrolling)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        agent.isStopped = true;
                        IdlePeriod = UnityEngine.Random.Range(5, 10);
                        currentStatus = BehoviourType.Idle;
                        targetLocation = null;
                    }
                    else if (agent.remainingDistance >= agent.stoppingDistance)
                    {
                        agent.SetDestination(targetLocation.position);
                        currentStatus = BehoviourType.Patrolling;
                    }
                }
                else if (target != null)
                {
                    if (Distance > AttackRange)
                    {
                        agent.isStopped = false;
                        currentStatus = BehoviourType.Chasing;
                        agent.SetDestination(target.position);
                    }
                    else if (Distance <= AttackRange)
                    {
                            Attack();
                    }
                }
                else if (target == null && (currentStatus == BehoviourType.Chasing))
                {
                    SetIdle();
                }
            }
        }

        public void Attack()
        {
            if (currentStatus == BehoviourType.GettingHit) return;
            if (Camera.main == null) return;

            if(agent.enabled)
            {
                agent.isStopped = true;
            }
            if (Time.time > LastAttackTime + 2)
            {
                LastAttackTime = Time.time;
                currentStatus = BehoviourType.Attacking;
                if (AdvancedGameManager.Instance.gameType == GameType.DieWhenYouAreCaught || HeroPlayerScript.Instance.isHiding)
                {
                    if (attacked == false)
                    {
                        if(AdvancedGameManager.Instance.canPlayerEscapeByClickEnough)
                        {
                            GetComponent<CapsuleCollider>().enabled = false;
                            GetComponent<Rigidbody>().isKinematic = true;
                            attacked = true;
                            HeroPlayerScript.Instance.GetCaught();
                            HeroPlayerScript.Instance.DeactivatePlayer();
                            AudioManager.Instance.Play_DemonKilling();
                            Destroy(gameObject);
                        }
                        else
                        {
                            GetComponent<CapsuleCollider>().enabled = false;
                            GetComponent<Rigidbody>().isKinematic = true;
                            attacked = true;
                            animator.SetTrigger("Attack");
                            transform.parent = Camera.main.transform;
                            transform.localEulerAngles = new Vector3(0, -180, 0);
                            transform.localPosition = HeroPlayerScript.Instance.DemonComingPoint.localPosition;
                            HeroPlayerScript.Instance.GetDamage(100);
                            AudioManager.Instance.Play_DemonKilling();
                        }
                        
                    }
                }
                else
                {
                    attacked = true;
                    animator.SetTrigger("MeeleeAttack");
                    audioSource.PlayOneShot(Audio_Hits[UnityEngine.Random.Range(0, Audio_Hits.Length)]);
                    StartCoroutine(HitToPlayer());
                }
            }
        }

        IEnumerator HitToPlayer()
        {
            yield return new WaitForSeconds(0.5f);
            if (Distance <= AttackRange)
            {
                HeroPlayerScript.Instance.GetDamage(AttackDamage);
            }
        }

        public void SetIdle()
        {
            agent.isStopped = true;
            agent.speed = InitSpeed;
            IdlePeriod = UnityEngine.Random.Range(5, 10);
            currentStatus = BehoviourType.Idle;
            targetLocation = null;
        }

        float LastTimeFireDamage = 0;

        public void GetDamageByPistolOrBaseBallStick(float damage)
        {
            if (health <= 0) return;

            health = health - damage;
            currentStatus = BehoviourType.GettingHit;
            if (enemyType == EnemyType.Walking)
            {
                animator.SetBool("Burning", false);
            }
            animator.SetBool("GetDamage", true);
            agent.speed = 0;
            if (Time.time > LastTimeFireDamage + 0.5f)
            {
                LastTimeFireDamage = Time.time;
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = Audio_Pains[UnityEngine.Random.Range(0, Audio_Pains.Length)];
                    audioSource.Play();
                }
            }
            StartCoroutine(StopGettingDamage());

            if (health <= 0)
            {
                agent.isStopped = true;
                agent.speed = 0;
                animator.SetTrigger("Die");
                currentStatus = BehoviourType.Dead;
                audioSource.PlayOneShot(Audio_Dies[UnityEngine.Random.Range(0, Audio_Dies.Length)]);
                Destroy(gameObject, 3);
            }
        }
        
        IEnumerator StopGettingDamage()
        {
            yield return new WaitForSeconds(0.5f);
            FinishedGetDamage();
        }

       

        public void GetDamageByFlashlight(float damage)
        {
            if (health <= 0) return;

            health = health - damage;
            currentStatus = BehoviourType.GettingHit;
            if(enemyType == EnemyType.Walking)
            {
                animator.SetBool("Burning", true);
            }
            animator.SetBool("GetDamage", true);
            agent.speed = 0;
            if (Time.time > LastTimeFireDamage + 0.5f)
            {
                LastTimeFireDamage = Time.time;
                for (int i = 0; i < burningParticles.Length; i++)
                {
                    if (burningParticles[i].isPlaying == false)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0)
                        {
                            burningParticles[i].Play();
                        }
                    }
                }
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = Audio_Pains[UnityEngine.Random.Range(0, Audio_Pains.Length)];
                    audioSource.Play();
                }
            }
            if (health <= 0)
            {
                agent.isStopped = true;
                agent.speed = 0;
                animator.SetTrigger("Die");
                currentStatus = BehoviourType.Dead;
                audioSource.PlayOneShot(Audio_Dies[UnityEngine.Random.Range(0, Audio_Dies.Length)]);
                Destroy(gameObject, 3);
            }
        }

        public void FinishedGetDamage()
        {
            if(health > 0 && currentStatus == BehoviourType.GettingHit)
            {
                currentStatus = BehoviourType.Idle;
            }
            animator.SetBool("GetDamage", false);
            agent.speed = InitSpeed;
        }
    }

    public enum BehoviourType
    {
        Patrolling,
        Idle,
        Attacking,
        Dead,
        GettingHit,
        Chasing,
        WakingUp
    }

    public enum EnemyType
    {
        Crawling,
        Walking,
        Random
    }
}

