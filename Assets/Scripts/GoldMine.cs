// GoldMine.cs
// Skrip ini akan menangani tambang emas, termasuk pengumpulan dan peningkatan.
using UnityEngine;
using System.Collections;
using TMPro; // Tambahkan ini untuk menggunakan TextMeshPro

public class GoldMine : MonoBehaviour
{
    public GameObject infoPanel; // Panel UI untuk informasi tambang
    public TextMeshProUGUI mineNameText; // Referensi ke teks nama tambang di UI
    public TextMeshProUGUI goldFlowText; // Referensi ke teks aliran emas di UI
    public TextMeshProUGUI currentLevelText; // Referensi ke teks level saat ini di UI
    public TextMeshProUGUI upgradeCostText; // Referensi ke teks biaya upgrade di UI

    public string mineName = "Tambang Utama";
    public int goldFlowPerSecond = 5; // Aliran emas per detik, diubah dari per menit
    public int baseUpgradeCost = 100; // Biaya dasar untuk upgrade
    public float upgradeCostMultiplier = 1.5f; // Pengali biaya upgrade

    private float collectInterval = 1f; // Interval pengumpulan dalam detik (1 detik)
    private float timer;
    private int currentLevel = 1;

    void Start()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // Sembunyikan panel info saat mulai
        }
        timer = collectInterval;
        // Set initial gold flow based on level 1 requirements
        SetGoldFlowBasedOnLevel();
        Debug.Log($"Tambang {mineName} Level {currentLevel} dengan aliran {goldFlowPerSecond} emas/detik dimulai.");
        UpdateInfoPanel(); // Panggil saat mulai untuk inisialisasi tampilan
    }

    void Update()
    {
        timer -= Time.deltaTime; // Kurangi timer berdasarkan waktu yang berlalu
        if (timer <= 0)
        {
            GameManager.AddGold(goldFlowPerSecond); // Tambahkan emas ke pemain
            Debug.Log($"Tambang {mineName} mengumpulkan {goldFlowPerSecond} emas.");
            timer = collectInterval; // Reset timer
        }
    }

    // Dipanggil ketika GameObject ini diklik (membutuhkan Collider pada GameObject)
    void OnMouseDown()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(!infoPanel.activeSelf); // Toggle visibilitas panel info UI
            Debug.Log($"Tambang {mineName} diklik. Panel info visibilitas: " + infoPanel.activeSelf);
            if (infoPanel.activeSelf) // Jika panel diaktifkan, perbarui informasinya
            {
                UpdateInfoPanel();
            }
        }
    }

    // Metode untuk mengupgrade tambang
    public void UpgradeMine()
    {
        int upgradeCost = CalculateUpgradeCost();
        // Batasi upgrade hingga level 4 karena aliran emas maksimum yang ditentukan
        if (currentLevel >= 4)
        {
            Debug.Log("Tambang sudah mencapai level maksimum aliran emas.");
            return;
        }

        if (GameManager.SpendGold(upgradeCost))
        {
            currentLevel++;
            SetGoldFlowBasedOnLevel(); // Perbarui aliran emas berdasarkan level baru
            Debug.Log($"Tambang {mineName} diupgrade ke Level {currentLevel}. Aliran emas baru: {goldFlowPerSecond}/detik.");
            UpdateInfoPanel(); // Perbarui informasi di panel UI setelah upgrade
        }
        else
        {
            Debug.Log("Koin tidak cukup untuk mengupgrade tambang!");
        }
    }

    // Menghitung biaya upgrade
    private int CalculateUpgradeCost()
    {
        return (int)(baseUpgradeCost * Mathf.Pow(upgradeCostMultiplier, currentLevel - 1));
    }

    // Metode untuk mengatur aliran emas berdasarkan level saat ini
    private void SetGoldFlowBasedOnLevel()
    {
        switch (currentLevel)
        {
            case 1:
                goldFlowPerSecond = 5;
                break;
            case 2:
                goldFlowPerSecond = 7;
                break;
            case 3:
                goldFlowPerSecond = 9;
                break;
            case 4:
                goldFlowPerSecond = 10;
                break;
            default:
                goldFlowPerSecond = 10; // Untuk level di atas 4, pertahankan 10
                break;
        }
    }

    // Metode untuk memperbarui semua teks di panel informasi tambang
    private void UpdateInfoPanel()
    {
        if (mineNameText != null)
        {
            mineNameText.text = $"Nama: {mineName}";
        }
        if (goldFlowText != null)
        {
            goldFlowText.text = $"Aliran: {goldFlowPerSecond}/detik"; // Ubah teks menjadi per detik
        }
        if (currentLevelText != null)
        {
            currentLevelText.text = $"Level: {currentLevel}";
        }
        if (upgradeCostText != null)
        {
            // Hanya tampilkan biaya upgrade jika belum mencapai level maksimum aliran emas
            if (currentLevel < 4)
            {
                upgradeCostText.text = $"Biaya Upgrade: {CalculateUpgradeCost()}";
            }
            else
            {
                upgradeCostText.text = "Level Aliran Maksimal"; // Pesan ketika mencapai level maksimum
            }
        }
    }
}