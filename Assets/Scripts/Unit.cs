// Unit.cs
// Skrip dasar untuk representasi unit (Archer/Knight).
// Anda akan melampirkan skrip ini ke prefab unit Anda.
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitType { Archer, Knight }
    public UnitType type;
    public int attackDamage = 10;
    public int health = 100;
    public float movementSpeed = 3f;

    void Start()
    {
        Debug.Log($"Unit {type} telah spawn!");
        // Anda bisa menambahkan logika inisialisasi unit di sini
    }

    // Contoh metode yang bisa dimiliki unit
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{type} mati!");
        Destroy(gameObject);
    }
}