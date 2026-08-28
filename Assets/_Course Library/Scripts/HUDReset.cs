using UnityEngine;
using TMPro;

public class HUDReset : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI statusText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Start()
    {
        ResetHUD();
    }

    void LateUpdate()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
        }

        string minutes = ((int)elapsedTime / 60).ToString("00");
        string seconds = ((int)elapsedTime % 60).ToString("00");

        if (timerText != null)
        {
            timerText.text = "Time: " + minutes + ":" + seconds;
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void ResetHUD()
    {
        isRunning = false;
        elapsedTime = 0f;
        if (timerText != null) timerText.text = "Time: 00:00";
        if (counterText != null) counterText.text = "Boxes Delivered: 0 / 10";
        if (statusText != null) statusText.text = "Status: Delivering";
    }
}