using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    public float chaseRange = 10f; // Jarak musuh mulai mengejar
    public float stopDistance = 1.5f; // Jarak berhenti saat dekat pemain

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Cari pemain dengan tag "Player"
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            agent.SetDestination(player.position); // Kejar pemain

            // Jika terlalu dekat, berhenti
            agent.isStopped = distance <= stopDistance;
        }
    }

    // ---- GIZMOS UNTUK MELIHAT RANGE ----
    void OnDrawGizmosSelected()
    {
        // Warna hijau untuk chase range (jarak pengejaran)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Warna merah untuk stop distance (jarak berhenti)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
