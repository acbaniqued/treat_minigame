using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickCounter : MonoBehaviour
{
    public TMP_Text DisplayText;
    public static ClickCounter instance;
    [SerializeField]
    private int ClickCount = 0;


    public void Start()
    {
        if (instance == null) { instance = this; } else { Destroy(this.gameObject); }
    }

    public void ResetClickCount()
    {
        ClickCount = 0;
        if (DisplayText != null)
        {
            DisplayText.text = "0";
        }
    }

    public void AddClick()
    {
        ClickCount += 1;
        if (DisplayText != null)
        {
            DisplayText.text = ClickCount.ToString();
        }
    }

    public int GetClickCount()
    {
        return ClickCount;
    }
}
