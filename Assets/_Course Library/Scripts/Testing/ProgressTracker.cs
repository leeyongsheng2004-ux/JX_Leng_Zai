using UnityEngine;
using TMPro;

public class ProgressTracker : MonoBehaviour
{
    [Header("UI Displays")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI promptText; 

    [Header("Goal")]
    public int totalBoxesRequired = 10;
    private int boxesDelivered = 0;

    [Header("Feedback Effects")]
    public ParticleSystem successParticles;
    public AudioSource audioSource;
    public AudioClip correctDropSound;
    public AudioClip victorySound;

    [Header("Card Prompts")]
    public AudioClip cardInsertSound;
    public AudioClip cardWarningSound;

    private float elapsedTime = 0f;
    private bool isComplete = false;
    private bool isTimerRunning = false;

    void Start()
    {
        // Initialize UI displays
        if (timerText != null)
        {
            timerText.text = "<color=#00E5FF>TIME:</color> 00:00";
        }

        if (counterText != null)
        {
            counterText.text = $"<color=#8EA4C8>DELIVERED:</color> <color=#FFD000>{boxesDelivered}</color> <color=#556677>/</color> {totalBoxesRequired}";
        }

        if (statusText != null)
        {
            statusText.text = "<color=#8EA4C8>STATUS:</color> <color=#FFA500>Delivering</color>";
        }

        if (promptText != null)
        {
    promptText.text = "<color=#FF3333>⚠ INSERT KEYCARD TO OPERATE</color>";
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
                timerText.text = $"<color=#00E5FF>TIME:</color> {minutes:00}:{seconds:00}";
            }
        }
    }

    public void OnCardInserted()
    {
        if (audioSource != null && cardInsertSound != null)
        {
            audioSource.PlayOneShot(cardInsertSound);
        }

        if (promptText != null)
        {
            promptText.text = "<color=#00E676>● CARD ACCEPTED</color>";
        }
    }

    public void OnCardRemoved()
    {
        if (audioSource != null && cardWarningSound != null)
        {
            audioSource.PlayOneShot(cardWarningSound);
        }

        if (promptText != null && !isComplete)
        {
            promptText.text = "<color=#FF3333>⚠ INSERT KEYCARD TO OPERATE</color>";
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
                counterText.text = $"<color=#8EA4C8>DELIVERED:</color> <color=#FFD000>{boxesDelivered}</color> <color=#556677>/</color> {totalBoxesRequired}";
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
            statusText.text = "<color=#00E676><b>MISSION COMPLETE!</b></color>";
        }

        if (promptText != null)
        {
            promptText.text = "";
        }

        if (audioSource != null && victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
        }
    }
}