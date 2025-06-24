// GameManager.cs
// Skrip ini akan mengelola statistik pemain seperti koin/emas dan menyediakan fungsionalitas umum.
using UnityEngine;
using System.Collections.Generic; // Diperlukan untuk List

public class GameManager : MonoBehaviour
{
    // Variabel untuk menyimpan jumlah koin/emas pemain
    public static int Gold { get; private set; } = 0; // Menggunakan static agar mudah diakses dari skrip lain

    // Event untuk memberi tahu UI ketika emas berubah
    public delegate void OnGoldChanged(int newGold);
    public static event OnGoldChanged OnGoldChangedCallback;

    // Untuk melacak jumlah unit yang dimiliki pemain
    public static Dictionary<string, int> PlayerUnits = new Dictionary<string, int>();

    void Awake()
    {
        // Pastikan hanya ada satu instance GameManager
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject); // Agar GameManager tetap ada antar scene jika diperlukan

        // Inisialisasi unit yang dimiliki
        if (!PlayerUnits.ContainsKey("Archer")) PlayerUnits.Add("Archer", 0);
        if (!PlayerUnits.ContainsKey("Knight")) PlayerUnits.Add("Knight", 0);

        // Panggil callback agar UI dapat menginisialisasi tampilan emas
        OnGoldChangedCallback?.Invoke(Gold);
    }

    // Metode untuk menambahkan koin/emas
    public static void AddGold(int amount)
    {
        Gold += amount;
        OnGoldChangedCallback?.Invoke(Gold); // Panggil event ketika emas berubah
        Debug.Log("Gold saat ini: " + Gold);
    }

    // Metode untuk mengurangi koin/emas (misalnya untuk pembelian)
    public static bool SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            OnGoldChangedCallback?.Invoke(Gold); // Panggil event ketika emas berubah
            Debug.Log("Koin dikeluarkan: " + amount + ". Koin tersisa: " + Gold);
            return true;
        }
        Debug.Log("Koin tidak cukup untuk tindakan ini.");
        return false;
    }

    // Metode untuk menambahkan unit ke inventaris pemain
    public static void AddUnit(string unitType)
    {
        if (PlayerUnits.ContainsKey(unitType))
        {
            PlayerUnits[unitType]++;
        }
        else
        {
            PlayerUnits.Add(unitType, 1);
        }
        Debug.Log($"Pemain sekarang memiliki {PlayerUnits[unitType]} {unitType}(s).");
    }
}