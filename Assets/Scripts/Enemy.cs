using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP = 50;
    public int damage = 10;
    public float attackCooldown = 2f;
    public float attackDistance = 0.6f;

    private int currentHP;
    private Castle castle;
    private Collider2D castleCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float cooldownTimer = 0f;
    private bool isActive = false;
    private WaveSpawner waveSpawner;

    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject castleObj = GameObject.FindWithTag("Castle");
        if (castleObj != null)
        {
            castle = castleObj.GetComponent<Castle>();
            castleCollider = castleObj.GetComponent<Collider2D>();
        }

        waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    public void ActivateCombat()
    {
        isActive = true;
    }

    void Update()
    {
        if (!isActive || castle == null || castleCollider == null) return;

        cooldownTimer -= Time.deltaTime;

        Vector2 contactPoint = castleCollider.bounds.ClosestPoint(transform.position);
        float distance = Vector2.Distance(transform.position, contactPoint);
        bool inRange = distance <= attackDistance;

        if (animator) animator.SetBool("isAttacking", inRange);

        if (inRange && cooldownTimer <= 0f)
        {
            castle.TakeDamage(damage);
            cooldownTimer = attackCooldown;
        }

        // Orientasi sprite
        if (spriteRenderer != null)
        {
            Vector2 dir = (castle.transform.position - transform.position).normalized;
            spriteRenderer.flipX = dir.x < 0;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            waveSpawner?.NotifyEnemyKilled(gameObject);
            Destroy(gameObject);
        }
    }
}
