using System.Collections;
using UnityEngine;

public class TimedAlarm : MonoBehaviour
{
    public AudioSource alarmSource;
    public float duration = 4.0f;

    private Coroutine activeAlarmRoutine;

    public void PlayAlarmUntilDropped()
    {
        if (activeAlarmRoutine != null)
        {
            StopCoroutine(activeAlarmRoutine);
        }
        activeAlarmRoutine = StartCoroutine(AlarmRoutine());
    }

    private IEnumerator AlarmRoutine()
    {
        if (alarmSource != null)
        {
            alarmSource.loop = true;
            alarmSource.Play();
        }

        yield return new WaitForSeconds(duration);

        if (alarmSource != null)
        {
            alarmSource.Stop();
            alarmSource.loop = false;
        }
    }
}
