using UnityEngine;
using TMPro;

public class ProgressTracker : MonoBehaviour
{
    [Header("UI Displays")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI statusText;

    [Header("Goal")]
    public int totalBoxesRequired = 10;
    private int boxesDelivered = 0;

    [Header("Feedback Effects")]
    public ParticleSystem successParticles;
    public AudioSource audioSource;
    public AudioClip correctDropSound;
    public AudioClip victorySound;

    private float elapsedTime = 0f;
    private bool isComplete = false;
    private bool isTimerRunning = false;

    void Start()
    {
        // Initialize timer text display at 00:00
        if (timerText != null)
        {
            timerText.text = "Time: 00:00";
        }
    }

    void Update()
    {
        if (isTimerRunning && !isComplete)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);
            if (timerText != null)
            {
                timerText.text = $"Time: {minutes:00}:{seconds:00}";
            }
        }
    }

    // Call this function from your Spawn/Dump Box Button event
    public void StartTimer()
    {
        if (!isTimerRunning && !isComplete)
        {
            isTimerRunning = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isComplete) return;

        // Matches existing box tag
        if (other.CompareTag("Box"))
        {
            boxesDelivered++;
            
            // Untag so the same box doesn't register multiple times
            other.tag = "Untagged";

            // Update UI
            if (counterText != null)
            {
                counterText.text = $"Boxes Delivered: {boxesDelivered} / {totalBoxesRequired}";
            }

            // Trigger visual & audio feedback
            if (successParticles != null)
            {
                successParticles.transform.position = other.transform.position;
                successParticles.Play();
            }

            if (audioSource != null && correctDropSound != null)
            {
                audioSource.PlayOneShot(correctDropSound);
            }

            // Check if goal reached
            if (boxesDelivered >= totalBoxesRequired)
            {
                CompleteTraining();
            }
        }
    }

    private void CompleteTraining()
    {
        isComplete = true;
        isTimerRunning = false;

        if (statusText != null)
        {
            statusText.text = "<color=green>Training Complete!</color>";
        }

        if (audioSource != null && victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
        }
    }
}