using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    public void Start()
    {
        if (instance == null) { instance = this; } else { Destroy(this.gameObject); }
        DontDestroyOnLoad(instance);
    }
    public void LoadMinigame(Minigame minigame)
    {
        
    }
}
