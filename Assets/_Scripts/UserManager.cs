using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;
    [SerializeField]
    private string currentUser;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        if (instance == null) { instance = this; } else { Destroy(this.gameObject); }
    }

    public void LogUsername(string username)
    {
        currentUser = username;
    }
}
