using UnityEngine;

public class BatController : MonoBehaviour
{
    [Header("Swing Settings")]
    public float swingTime = 0.35f;
    public float swingAngle = 95f;
    public float verticalOffset = 30f;

    [Header("Damage")]
    public float damage = 20f;
    private bool swinging;
    private bool hitSomething;
    private float timer;
    private Quaternion neutralRot;
    private Quaternion startArc;
    private Quaternion endArc;
    private bool swingLeft = true;


    void Start()
    {
        neutralRot = transform.localRotation;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !swinging)
        {
            StartSwing();
        }

        if (!swinging) return;

        timer += Time.deltaTime;

        float t = timer / swingTime;


        if (t <= 1f)
        {
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            transform.localRotation =
                Quaternion.Slerp(startArc, endArc, smooth);
        }
        else
        {
            EndSwing();
        }
    }


    void StartSwing()
    {
        swinging = true;
        timer = 0f;
        hitSomething = false;

        float dir = swingLeft ? 1f : -1f;

        startArc = neutralRot * Quaternion.Euler(-verticalOffset, -dir * swingAngle, 0);

        endArc = neutralRot * Quaternion.Euler(verticalOffset, dir * swingAngle, 0);

        swingLeft = !swingLeft;
    }


    void EndSwing()
    {
        swinging = false;

        transform.localRotation = neutralRot;
    }


    void OnTriggerEnter(Collider other)
    {
        if (!swinging || hitSomething)
            return;

        // Check if object can take damage
        if (other.TryGetComponent<IDamageable>(out var dmg))
        {
            dmg.TakeDamage(damage);

            hitSomething = true;
        }
    }
}