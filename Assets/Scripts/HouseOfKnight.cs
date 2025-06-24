// HouseOfKnight.cs
// Skrip ini akan menangani interaksi dengan House of Knight.
using UnityEngine;
using System.Collections; // Diperlukan untuk IEnumerator

public class HouseOfKnight : MonoBehaviour
{
    public GameObject uiPanel; // Referensi ke panel UI untuk pemilihan unit
    public Transform spawnPoint; // Titik di mana unit akan spawn
    public GameObject archerPrefab; // Prefab untuk unit Archer
    public GameObject knightPrefab; // Prefab untuk unit Knight

    // Biaya pelatihan dan waktu pelatihan
    public int archerCost = 50;
    public float archerTrainingTime = 5f;
    public int knightCost = 100;
    public float knightTrainingTime = 10f;

    private bool isTraining = false; // Status apakah sedang ada pelatihan

    void Start()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false); // Sembunyikan panel UI saat mulai
        }
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn Point untuk House Of Knight belum diatur!");
        }
    }

    // Dipanggil ketika GameObject ini diklik (membutuhkan Collider pada GameObject)
    void OnMouseDown()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(!uiPanel.activeSelf); // Toggle visibilitas panel UI
            Debug.Log("House of Knight diklik. Panel UI visibilitas: " + uiPanel.activeSelf);
        }
    }

    // Metode untuk memulai pelatihan Archer
    public void TrainArcher()
    {
        if (!isTraining)
        {
            if (GameManager.SpendGold(archerCost)) // Coba kurangi emas
            {
                StartCoroutine(TrainUnit(archerPrefab, archerTrainingTime, "Archer"));
            }
            else
            {
                Debug.Log("Koin tidak cukup untuk melatih Archer!");
                // Tampilkan pesan di UI jika koin tidak cukup
            }
        }
        else
        {
            Debug.Log("Sedang melatih unit lain. Harap tunggu.");
        }
    }

    // Metode untuk memulai pelatihan Knight
    public void TrainKnight()
    {
        if (!isTraining)
        {
            if (GameManager.SpendGold(knightCost)) // Coba kurangi emas
            {
                StartCoroutine(TrainUnit(knightPrefab, knightTrainingTime, "Knight"));
            }
            else
            {
                Debug.Log("Koin tidak cukup untuk melatih Knight!");
                // Tampilkan pesan di UI jika koin tidak cukup
            }
        }
        else
        {
            Debug.Log("Sedang melatih unit lain. Harap tunggu.");
        }
    }

    // Coroutine untuk menangani proses pelatihan unit
    IEnumerator TrainUnit(GameObject unitPrefab, float trainingTime, string unitTypeName)
    {
        isTraining = true;
        Debug.Log($"Memulai pelatihan {unitTypeName} selama {trainingTime} detik...");

        // Anda bisa menambahkan animasi atau progress bar di sini

        yield return new WaitForSeconds(trainingTime); // Tunggu selama waktu pelatihan

        // Setelah pelatihan selesai, spawn unit
        if (spawnPoint != null && unitPrefab != null)
        {
            Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
            GameManager.AddUnit(unitTypeName); // Tambahkan unit ke GameManager
            Debug.Log($"{unitTypeName} selesai dilatih dan telah spawn!");
        }
        else
        {
            Debug.LogError("Gagal spawn unit: spawnPoint atau unitPrefab tidak diatur.");
        }
        isTraining = false;
    }
}