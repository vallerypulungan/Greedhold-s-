using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public string enemyType;
    public int count;
    public List<GameObject> members = new List<GameObject>();

    public void Initialize(string type)
    {
        enemyType = type;
        count = 0;
    }
    
    public void AddMember(GameObject enemy)
    {
        members.Add(enemy);
        count++;
    }

    // Misalnya untuk UI interaksi
    public void OnClick()
    {
        Debug.Log($"Clicked on group: {enemyType}, Total: {count}");
        // TODO: Panggil UI panel, tombol serang, dsb.
    }

    public void RemoveMember(GameObject enemy)
    {
        members.Remove(enemy);
        count--;

        if (count <= 0)
        {
            Destroy(gameObject); // Hapus group object jika semua mati
        }
    }
}
