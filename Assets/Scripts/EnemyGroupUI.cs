using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyGroupUI : MonoBehaviour
{
    public CanvasGroup popupGroup;                // Gantilah dari GameObject ke CanvasGroup
    public TextMeshProUGUI groupTypeText;
    public TextMeshProUGUI groupCountText;
    public Button attackButton;
    public Camera mainCamera;

    private EnemyGroup currentGroup;

    private void Start()
    {
        HidePopup(); // Pastikan popup disembunyikan saat start
    }

    public void ShowPopup(EnemyGroup group)
    {
        currentGroup = group;

        groupTypeText.text = $"Type: {group.enemyType}";
        groupCountText.text = $"Count: {group.count}";

        // Tampilkan popup
        popupGroup.alpha = 1;
        popupGroup.interactable = true;
        popupGroup.blocksRaycasts = true;

        // Tempatkan popup di tengah layar
        RectTransform popupRect = popupGroup.GetComponent<RectTransform>();
        popupRect.anchorMin = popupRect.anchorMax = new Vector2(0.5f, 0.5f);
        popupRect.pivot = new Vector2(0.5f, 0.5f);
        popupRect.anchoredPosition = Vector2.zero;

        popupGroup.transform.SetAsLastSibling(); // Pindahkan ke atas layer UI

        // Siapkan tombol
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() => AttackGroup());
    }

    public void HidePopup()
    {
        popupGroup.alpha = 0;
        popupGroup.interactable = false;
        popupGroup.blocksRaycasts = false;
        currentGroup = null;
    }

    private void AttackGroup()
    {
        if (currentGroup != null)
        {
            Debug.Log($"Attacking group: {currentGroup.enemyType}");

            WaveSpawner waveSpawner = FindObjectOfType<WaveSpawner>();

            foreach (GameObject enemy in currentGroup.members)
            {
                waveSpawner?.NotifyEnemyKilled(enemy);
                Destroy(enemy);
            }

            Destroy(currentGroup.gameObject);
            HidePopup();
        }
    }
}
