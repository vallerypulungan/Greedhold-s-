using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    public float normalSpeed = 0.5f;
    public int damage = 5;

    private float cooldownTimer = 0f;
    private bool isActive = false;
    private Transform target;

    private void Start()
    {
        target = GameObject.FindWithTag("Castle")?.transform;
    }

    public void ActivatePersonalCombat()
    {
        isActive = true;
    }

    void Update()
    {
        if (!isActive || target == null) return;

        cooldownTimer -= Time.deltaTime;
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            if (cooldownTimer <= 0f)
            {
                ShootProjectile();
                cooldownTimer = attackCooldown;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, normalSpeed * Time.deltaTime);
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
