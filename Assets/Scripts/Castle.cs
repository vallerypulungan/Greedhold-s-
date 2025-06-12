using UnityEngine;

public class Castle : MonoBehaviour
{
    public int maxHP = 5000;
    public int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Debug.Log("Castle destroyed!");
            // Trigger Game Over
        }
    }

    public void Repair(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }
}
