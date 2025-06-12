using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;
    private int damage;

    public void SetTarget(Transform targetTransform, int damageAmount)
    {
        target = targetTransform;
        damage = damageAmount;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.1f)
        {
            // Lakukan damage ke Castle
            Castle castle = target.GetComponent<Castle>();
            if (castle != null)
            {
                castle.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
