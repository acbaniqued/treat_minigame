using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{

    public void LoginUser(string username)
    {
        if (string.IsNullOrEmpty(username)) { Debug.Log("Please enter a username"); }
        UserManager.instance.LogUsername(username);
    }

    public void CreateUser(string username)
    {
        if (!string.IsNullOrEmpty(username))
        {
            DatabaseManager.instance.CreateUser(username);
            Debug.Log("User created : " + username);
        }
    }

}
