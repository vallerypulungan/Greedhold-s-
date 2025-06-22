using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject[] canvases;
    private int currentIndex = 0;

    void Start()
    {
        ShowCanvas(0);
    }

    public void ShowCanvas(int index)
    {
        // Matikan semua canvas dulu
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].SetActive(false);
        }

        // Aktifkan hanya canvas yang sesuai
        if (index >= 0 && index < canvases.Length)
        {
            canvases[index].SetActive(true);
            currentIndex = index;
        }
    }

    public void NextCanvas()
    {
        int nextIndex = currentIndex + 1;
        if (nextIndex < canvases.Length)
        {
            ShowCanvas(nextIndex);
        }
    }

    public void PreviousCanvas()
    {
        int prevIndex = currentIndex - 1;
        if (prevIndex >= 0)
        {
            ShowCanvas(prevIndex);
        }
    }

    public void Done()
    {
        // Nonaktifkan semua canvas
        foreach (GameObject canvas in canvases)
        {
            canvas.SetActive(false);
        }
    }
}
