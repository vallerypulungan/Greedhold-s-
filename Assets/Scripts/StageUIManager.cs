using TMPro;
using UnityEngine;

public class StageUIManager : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public CanvasGroup stageCanvasGroup;

    private void Start()
    {
        stageCanvasGroup.alpha = 0;
        stageCanvasGroup.interactable = false;
        stageCanvasGroup.blocksRaycasts = false;
    }

    public void ShowStage(int stageNumber)
    {
        stageText.text = $"Stage {stageNumber}";

        stageCanvasGroup.alpha = 1;
        stageCanvasGroup.interactable = false;
        stageCanvasGroup.blocksRaycasts = false;
    }

    public void HideStage()
    {
        stageCanvasGroup.alpha = 0;
    }
}
