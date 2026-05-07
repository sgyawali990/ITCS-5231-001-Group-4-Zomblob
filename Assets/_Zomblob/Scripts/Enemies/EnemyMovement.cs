using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;

    // How often we update the path (prevents spam)
    [SerializeField] private float repathRate = 0.25f;
    private float repathTimer;

    private Animator animator;

    private NavMeshAgent agent;

    [Header("Target")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private Transform playerTransform;

    [Header("Damage")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    private float nextAttackTime;


    void Start()
    {

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            // Movement
            agent.speed = moveSpeed;

            // NavMesh handles rotation
            agent.updateRotation = true;

            // These matter for turning
            agent.angularSpeed = 300f;   
            agent.acceleration = 20f;

            // Prevent zombie from walking into player
            agent.stoppingDistance = 1f;
        }

        // Auto-find player if not assigned
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }
        Debug.Log(animator.enabled);
    }


    void Update()
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            return;

        if (playerTransform == null)
            return;

        if (!agent.isStopped)
        {
            repathTimer -= Time.deltaTime;
            if (repathTimer <= 0f)
            {
                agent.SetDestination(playerTransform.position);
                repathTimer = repathRate;
            }
        }

        animator.SetFloat("speed", agent.velocity.magnitude);

        if (Time.time < nextAttackTime)
        {
            return;
        }

        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist <= attackRange)
        {
            agent.isStopped = true;
            agent.ResetPath();

            animator.SetTrigger("attack");

            if (playerTransform.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            {
                health.TakeDamage(damage);
                nextAttackTime = Time.time + attackCooldown;
                StartCoroutine(ResumeMovementAfterDelay());
            }
        }
    }

    IEnumerator ResumeMovementAfterDelay()
    {
        yield return new WaitForSeconds(0.6f);

        if (agent != null && agent.enabled)
        {
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);
        }
    }

}