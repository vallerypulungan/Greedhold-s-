// Castle.cs
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Castle : MonoBehaviour
{
    public int maxHP = 5000;
    [SerializeField] public int currentHP;

    [Header("UI Optional")]
    public Slider hpBar;
    public Text hpText;

    private void Awake()
    {
        currentHP = maxHP;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);
        Debug.Log($"Castle HP: {currentHP}/{maxHP}");

        UpdateUI();

        if (currentHP <= 0)
        {
            Debug.Log("Castle destroyed!");
        }
    }

    public void Repair(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        Debug.Log($"Castle repaired: {currentHP}/{maxHP}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (hpBar != null)
        {
            hpBar.value = (float)currentHP / maxHP;
        }
        if (hpText != null)
        {
            hpText.text = $"HP: {currentHP} / {maxHP}";
        }
    }
}