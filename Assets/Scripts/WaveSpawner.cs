// WaveSpawner.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData {
    public string type;
    public int count;
}

[System.Serializable]
public class Wave {
    public int wave_id;
    public string spawn_time;
    public List<EnemySpawnData> enemies;
}

[System.Serializable]
public class StageData {
    public int stage;
    public List<Wave> waves;
}

[System.Serializable]
public class StageList {
    public List<StageData> stages;
}

public class WaveSpawner : MonoBehaviour {
    public TextAsset waveJson;
    public Transform[] spawnPoints;
    public GameObject swordmanPrefab;
    public GameObject knightPrefab, paladinPrefab;
    public GameObject archerPrefab, crossbowmanPrefab, eliteMarksmanPrefab;

    private StageList stageList;
    private int currentStage = 1;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private WaveNotifier waveNotifier;

    private void Start() {
        if (waveJson == null) {
            Debug.LogError("Wave JSON file not assigned!");
            return;
        }

        try {
            stageList = JsonUtility.FromJson<StageList>(waveJson.text);
            if (stageList == null || stageList.stages == null || stageList.stages.Count == 0) {
                Debug.LogError("Failed to parse wave JSON or no stages found.");
                return;
            }
        } catch {
            Debug.LogError("Error parsing wave JSON. Ensure it is wrapped as an object with a 'stages' field.");
            return;
        }

        waveNotifier = FindObjectOfType<WaveNotifier>();
        FindObjectOfType<StageUIManager>()?.ShowStage(currentStage);
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves() {
        yield return null;

        var stage = stageList.stages.Find(s => s.stage == currentStage);
        if (stage == null) {
            Debug.LogError($"Stage {currentStage} not found in wave data.");
            yield break;
        }

        foreach (var wave in stage.waves) {
            yield return new WaitUntil(() => activeEnemies.Count == 0);

            if (waveNotifier != null) {
                waveNotifier.ShowWaveMessage($"Wave {wave.wave_id} starting...");
            }

            foreach (var enemy in wave.enemies) {
                GameObject prefab = GetEnemyPrefab(enemy.type);
                if (prefab == null) {
                    Debug.LogWarning($"No prefab assigned for enemy type: {enemy.type}");
                    continue;
                }

                // Buat parent group object untuk tiap jenis
                GameObject groupObj = new GameObject($"Group_{enemy.type}_Wave{wave.wave_id}");
                groupObj.transform.position = GetRandomSpawnPoint().position;
                EnemyGroup group = groupObj.AddComponent<EnemyGroup>();
                group.Initialize(enemy.type);

                // Hitung kecepatan musuh berdasarkan wave
                float baseSpeed = 0.4f;
                float speedIncrement = 0.1f;
                float calculatedSpeed = baseSpeed + (wave.wave_id - 1) * speedIncrement;

                // Tambahkan script mover ke grup
                EnemyGroupMover mover = groupObj.AddComponent<EnemyGroupMover>();
                mover.speed = calculatedSpeed;

                // Spawn musuh dan atur posisi
                for (int i = 0; i < enemy.count; i++) {
                    Vector3 offset = new Vector3(i * 0.5f, 0, 0);
                    GameObject obj = Instantiate(prefab, groupObj.transform.position + offset, Quaternion.identity);
                    obj.transform.parent = groupObj.transform;

                    // Atur kecepatan khusus untuk RangedEnemy
                    RangedEnemy rangedScript = obj.GetComponent<RangedEnemy>();
                    if (rangedScript != null) {
                        rangedScript.normalSpeed = calculatedSpeed;
                    }

                    group.AddMember(obj);
                    activeEnemies.Add(obj);
                }

                // Tambahkan collider ke group agar bisa diklik
                BoxCollider2D collider = groupObj.AddComponent<BoxCollider2D>();
                float groupWidth = enemy.count * 0.5f;
                collider.size = new Vector2(groupWidth, 1.0f);
                collider.offset = new Vector2(groupWidth / 2f - 0.25f, 0);
                collider.isTrigger = false;

                // Tambahkan Rigidbody2D agar collider bisa ikut bergerak
                Rigidbody2D rb = groupObj.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.simulated = true;
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            }
        }

        Debug.Log("All waves in this stage are completed.");
    }

    private Transform GetRandomSpawnPoint() {
        return spawnPoints[Random.Range(0, Mathf.Min(2, spawnPoints.Length))];
    }

    private GameObject GetEnemyPrefab(string type) {
        switch (type) {
            case "Swordman": return swordmanPrefab;
            case "Knight": return knightPrefab;
            case "Paladin": return paladinPrefab;
            case "Archer": return archerPrefab;
            case "Crossbowman": return crossbowmanPrefab;
            case "Elite Marksman": return eliteMarksmanPrefab;
            default: return null;
        }
    }

    public void NotifyEnemyKilled(GameObject enemy) {
        if (activeEnemies.Contains(enemy)) {
            activeEnemies.Remove(enemy);
        }
    }
}
