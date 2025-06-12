using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    public float normalSpeed = 0.5f;

    public int damage = 5;

    private float cooldownTimer = 0f;
    private Transform target;

    private void Start()
    {
        target = GameObject.FindWithTag("Castle")?.transform;
    }

    private void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        cooldownTimer -= Time.deltaTime;

        if (distance > attackRange)
        {
            // JIKA di luar jangkauan, maju ke castle
            transform.position = Vector2.MoveTowards(transform.position, target.position, normalSpeed * Time.deltaTime);
        }
        else
        {
            // JIKA dalam jangkauan, berhenti dan tembak
            if (cooldownTimer <= 0f)
            {
                ShootProjectile();
                cooldownTimer = attackCooldown;
            }
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetTarget(target, damage);
        }
    }
}
