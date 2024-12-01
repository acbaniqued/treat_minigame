using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;
    [SerializeField]
    private int SelectedDifficulty;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (instance == null) { instance = this; } else { Destroy(this.gameObject); }
    }

    public void SetDifficulty(int level)
    {
        SelectedDifficulty = level;
    }

    public int GetDifficulty()
    {
        return SelectedDifficulty;
    }
}
