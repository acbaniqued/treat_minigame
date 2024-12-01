using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class SimpleCountdownTimer : Timer
{
    public TMP_Text TextDisplay;
    [SerializeField]
    private float timerDuration = 15f;
    private int intTimeRemaining = 0;

    public void SetTimerDuration(float time)
    {
        timerDuration = time;
    }

    public override void StartTimer()
    {
        StartCoroutine(IStartTimer(timerDuration));
    }

    public override void RestartTimer()
    {
        StartCoroutine(IRestartTimer(timerDuration));
    }

    public IEnumerator IRestartTimer(float duration)
    {
        isTimerStopped = true;
        
        yield return null;
        StartTimer();
        yield break;
    }

    public IEnumerator IStartTimer(float duration)
    {
        if (isTimerRunning) yield break;
        isTimerRunning = true;
        isTimerStopped = false;
        intTimeRemaining = Mathf.CeilToInt(duration);
        float currentTime = 0;
        float counter = 0;
        while(counter <= duration)
        {
            if (!isTimerPaused)
            {
                counter += Time.deltaTime;
                currentTime = duration - counter;
                intTimeRemaining = Mathf.CeilToInt(currentTime);
                TextDisplay.text = intTimeRemaining.ToString();
                yield return null;
            }
            else
            {
                yield return null;
            }

            if (isTimerStopped)
            {
                Debug.Log("Timer forced stop!");
                isTimerRunning = false;
                yield break;
            }
        }
        isTimerRunning = false;
        Debug.Log("Timer finished!");
        yield break;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            StopTimer();
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            StartTimer();
        }
    }

    public void Start()
    {
        StartTimer();
    }

    public int GetTimeRemaining()
    {
        return intTimeRemaining;
    }
}
