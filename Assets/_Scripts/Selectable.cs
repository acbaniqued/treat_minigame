using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private bool isActive = true;
    [SerializeField]
    private float selectionInterval;
    public List<UnityEvent> OnSelection = new List<UnityEvent>();
    public List<UnityEvent> OnDeselection = new List<UnityEvent>();

    private bool forcedDisable;

    private bool isOnInterval = false;

    public void Update()
    {
       
    }

    public void OnSelect()
    {
        foreach(UnityEvent e in OnSelection)
        {
            e.Invoke();
        }
        StartSelectionInterval(selectionInterval);
    } 
    
    public void OnDeselect()
    {
        foreach(UnityEvent e in OnDeselection)
        {
            e.Invoke();
        }
        StartSelectionInterval(selectionInterval);
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void SetActive(bool state)
    {
        isActive = state;
    }

    public void StartSelectionInterval(float time)
    {
        StartCoroutine(IStartSelectionInterval(time));
    }

    public IEnumerator IStartSelectionInterval(float time)
    {
        if (isOnInterval) yield break;
        isOnInterval = true;
        isActive = false;
        yield return new WaitForSeconds(time);
        if (!forcedDisable) 
        {
            isActive = true;
        }
        isOnInterval =  false;    
    }

    public void ForceDisable()
    {
        forcedDisable = true;
    }
}
