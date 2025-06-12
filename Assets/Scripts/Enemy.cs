using UnityEngine;

public class Enemy : MonoBehaviour
{
    private WaveSpawner waveSpawner;
    public string enemyName;
    public int maxHP;
    public int currentHP;
    public int damage;
    public float moveSpeed = 2f;

    private Transform target;

    private void Start()
    {
        currentHP = maxHP;
        target = GameObject.FindWithTag("Castle").transform;
        waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    private void Update()
    {
        if (target != null)
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
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
