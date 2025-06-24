using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public string enemyType;
    public int count => members.Count;
    public List<GameObject> members = new List<GameObject>();

    private void OnMouseDown()
    {
        Debug.Log($"Clicked group: {enemyType}");

        EnemyGroupUI ui = FindObjectOfType<EnemyGroupUI>();
        if (ui != null)
        {
            ui.ShowPopup(this);
        }
        else
        {
            Debug.LogWarning("No UI Manager (EnemyGroupUI) found in scene.");
        }
    }

    public void Initialize(string type)
    {
        enemyType = type;
        members = new List<GameObject>();
    }

    public void AddMember(GameObject enemy)
    {
        members.Add(enemy);
    }

    public void RemoveMember(GameObject enemy)
    {
        if (members.Contains(enemy))
        {
            members.Remove(enemy);
        }

        // Jika grup sudah kosong, bisa dihancurkan otomatis (opsional)
        if (members.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}
