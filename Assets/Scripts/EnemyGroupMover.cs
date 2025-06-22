using UnityEngine;

public class EnemyGroupMover : MonoBehaviour
{
    public float speed = 1f;
    public float stopOffset = 0.5f;

    private Vector3 stopPoint;
    private bool isMoving = false;

    void Start()
    {
        GameObject castleObj = GameObject.FindWithTag("Castle");
        if (castleObj == null) return;

        Collider2D castleCollider = castleObj.GetComponent<Collider2D>();
        if (castleCollider == null) return;

        Vector2 direction = (transform.position - castleObj.transform.position).normalized;
        Vector2 edgePoint = castleCollider.bounds.ClosestPoint(transform.position);

        stopPoint = edgePoint + direction * stopOffset;  
        stopPoint.z = transform.position.z;

        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        float dist = Vector2.Distance(transform.position, stopPoint);
        if (dist > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, stopPoint, speed * Time.deltaTime);
        }
        else
        {
            isMoving = false;

            foreach (Transform child in transform)
            {
                var ranged = child.GetComponent<RangedEnemy>();
                var melee = child.GetComponent<Enemy>();
                ranged?.ActivatePersonalCombat();
                melee?.ActivateCombat();
            }
        }
    }
}
