using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool isTimerRunning = false;
    public bool isTimerPaused = false;
    public bool isTimerStopped = false;

    public virtual void StartTimer()
    {
        
    }

    public virtual void PauseTimer()
    {
        isTimerPaused = true;
    }

    public virtual void UnpauseTimer()
    {
        isTimerPaused = false;
    }

    public virtual void StopTimer()
    {
        isTimerStopped = true;
    }

    public virtual void RestartTimer()
    {

    }
}
