using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;


public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 120f;

    private NavMeshAgent agent;
    [SerializeField] public Transform playerTransform;

    [Header("Damage")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCoolDown = 1f;
    private float lastAttackTime;

    private float repathTimer;
    [SerializeField] private float repathRate = 0.25f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.angularSpeed = rotationSpeed;
        }

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            return;

        if (playerTransform == null)
            return;

        repathTimer -= Time.deltaTime;

        if (repathTimer <= 0f)
        {
            agent.SetDestination(playerTransform.position);
            repathTimer = repathRate;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Time.time < lastAttackTime)
            return;

        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            player.TakeDamage((int)damage);

            Debug.Log("Enemy collided with player!");

            lastAttackTime = Time.time + attackCoolDown;
        }
    }
}
