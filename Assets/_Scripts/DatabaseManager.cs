using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;
    private DatabaseReference dbReference;
    private string deviceId;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        deviceId = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
        if (instance == null) { instance = this; } else { Destroy(this.gameObject); }
    }

    public void CreateUser(string name)
    {
        User newUser = new User(name);
        string json=JsonUtility.ToJson(newUser);

        dbReference.Child("users").Child(deviceId).Child(name).SetRawJsonValueAsync(json);
    }

    public bool IsExistingUser(string name)
    {
        bool isExisting = false;
        StartCoroutine(IGetUser(name, (bool exists) => { isExisting = exists; }));

        return isExisting;
    }

    public IEnumerator IGetUser(string userName, Action<bool> onCallback)
    {
        var currentUser = dbReference.Child("users").Child(userName).GetValueAsync();
        yield return new WaitUntil(predicate: () => currentUser.IsCompleted);

        if(currentUser != null)
        {
            DataSnapshot snapshot = currentUser.Result;
            if (snapshot.Exists) 
            {
                onCallback.Invoke(true);
                Debug.Log("User existing!");
            }

        }
        else
        {
            onCallback.Invoke(false);
        }
    }

    public void SendData()
    {

    }
}
