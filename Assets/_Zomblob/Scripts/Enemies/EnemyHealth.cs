using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AI;
using System.Xml.Serialization;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public static event Action EnemyDied;

    [Header("Health")]
    [SerializeField] private float maxHealth = 50f; 
    private float currentHealth;

    [SerializeField] FloatingHealthBar healthBar;

    [Header("Visual")]
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashTime = 0.1f;
    private Color originalColor;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 3f;
    private bool isKnockback = false;

    [Header("DeathTimer")]
    [SerializeField] private float deathTimer = 2f;

    [Header("Death Effects")]
    [SerializeField] private AudioClip[] deathSounds;

    [Header("Ammo Drop")]
    [SerializeField] private GameObject ammoBoxPrefab;
    [SerializeField] private float ammoDropChance = 0.1f;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private ZombiePool owningPool;
    private Camera mainCamera;

    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;

    


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        mainCamera = Camera.main;
        healthBar = GetComponentInChildren<FloatingHealthBar>();

        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;

        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        DisableRagdoll();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(HitFlash());

        if (!isKnockback)
            StartCoroutine(KnockbackRoutine());
    }

    private IEnumerator KnockbackRoutine()
    {
        isKnockback = true;
        if (agent != null) agent.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false;

            Vector3 knockDir = transform.position - mainCamera.transform.position;
            knockDir.y = 0;
            knockDir.Normalize();

            rb.AddForce(knockDir * knockbackForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.4f);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
        }

        if (agent != null && currentHealth > 0) agent.enabled = true;
        isKnockback = false;
    }

    private void Die()
    {

        StopAllCoroutines();

        if (enemyRenderer != null)
            enemyRenderer.material.color = originalColor;
        
        TryDropAmmo();
        EnemyDied?.Invoke();

        if (agent != null) agent.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false;
        }
        EnableRagDoll();
        PlayDeathSounds();

        

        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(deathTimer);

        if (owningPool != null)
            owningPool.Release(gameObject);
        else
            Destroy(gameObject);
    }

    private IEnumerator HitFlash()
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = hitColor;
            yield return new WaitForSeconds(flashTime);
            enemyRenderer.material.color = originalColor;
        }
    }

    public void Init(ZombiePool pool)
    {
        owningPool = pool;
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (enemyRenderer != null)
            enemyRenderer.material.color = originalColor;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (agent != null)
        {
            agent.enabled = false;
            agent.enabled = true;
        }
    }

    private void DisableRagdoll()
    {
        foreach(Rigidbody body in ragdollBodies)
        {
            if(body != rb)
            {
                body.isKinematic = true;
            }
        }

        foreach(Collider col in ragdollColliders)
        {
            if(col.gameObject != gameObject)
            {
                col.enabled = false;
            }
        }
    }

    private void EnableRagDoll()
    {
        foreach(Rigidbody body in ragdollBodies)
        {
            
            body.isKinematic = false;
            
        }

        foreach(Collider col in ragdollColliders)
        {
            col.enabled = true;
        }

        if(rb != null)
        {
            Vector3 fallDir = transform.forward * 2f +Vector3.up;
            rb.AddForce(fallDir, ForceMode.Impulse);
        }
    }

    private void PlayDeathSounds()
    {
        if(deathSounds != null && deathSounds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0,deathSounds.Length);

            AudioClip clip = deathSounds[randomIndex];

            AudioSource.PlayClipAtPoint(
                clip,
                transform.position
            );
        }
    }

    private void TryDropAmmo()
    {
        if (ammoBoxPrefab == null)
            return;

        float roll = UnityEngine.Random.value;

        if (roll < ammoDropChance)
        {
            Vector3 dropPos = transform.position + Vector3.up * 0.5f;
            Instantiate(ammoBoxPrefab, dropPos, Quaternion.identity);

            Debug.Log($"Ammo dropped from {name}");
        }
        else
        {
            Debug.Log($"No ammo drop from {name}");
        }
    }
}