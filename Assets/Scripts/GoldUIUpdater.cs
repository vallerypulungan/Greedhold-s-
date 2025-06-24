// GoldUIUpdater.cs
// Skrip ini akan memperbarui tampilan koin/emas di UI.
using UnityEngine;
using TMPro; // Diperlukan untuk TextMeshPro

public class GoldUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI goldText; // Referensi ke objek TextMeshPro di UI

    void OnEnable()
    {
        // Daftarkan ke event OnGoldChanged ketika skrip aktif
        GameManager.OnGoldChangedCallback += UpdateGoldDisplay;
    }

    void OnDisable()
    {
        // Hentikan pendaftaran ke event ketika skrip dinonaktifkan
        GameManager.OnGoldChangedCallback -= UpdateGoldDisplay;
    }

    void Start()
    {
        if (goldText == null)
        {
            goldText = GetComponent<TextMeshProUGUI>();
        }
        // Perbarui tampilan emas saat mulai (untuk inisialisasi)
        UpdateGoldDisplay(GameManager.Gold);
    }

    // Metode ini akan dipanggil setiap kali emas berubah
    void UpdateGoldDisplay(int newGold)
    {
        if (goldText != null)
        {
            goldText.text = $"Koin: {newGold}"; // Contoh format tampilan
        }
    }
}
