using UnityEngine;
using TMPro;
using System.Collections;

public class WaveNotifier : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;
    public float displayTime = 1.5f;

    private void Start()
    {
        canvasGroup.alpha = 0;
        messageText.text = "";
    }

    public void ShowWaveMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        messageText.text = message;
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(displayTime);

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        messageText.text = "";
    }
}
