using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Firebase.Extensions;
using Firebase.Database;
using Firebase;
using Firebase.Auth;

public class FirebaseServerManager : ServerManager
{
    [SerializeField]
    private string DatabaseURL;
    private DependencyStatus fDependencyStatus;
    private FirebaseAuth fAuth;
    private FirebaseUser fUser;
    private FirebaseApp firebaseApp;

    private void Awake()
    {
        DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            fDependencyStatus = task.Result;
            if (fDependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                firebaseApp = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                InitializeFirebase();

            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", fDependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    private bool IsFirebaseConnectionEstablished()
    {
        bool isReady = false;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            fDependencyStatus = task.Result;
            if (fDependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                firebaseApp = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                isReady = true;

            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", fDependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                isReady = false;
            }
        });
        return isReady;
    }



    private void InitializeFirebase()
    {
        fAuth = FirebaseAuth.DefaultInstance;

        fAuth.StateChanged += AuthStateChange;
        AuthStateChange(this, null);
    }

    private void AuthStateChange(object sender, System.EventArgs args)
    {
        if (fAuth.CurrentUser != fUser)
        {
            bool signedIn = fUser != fAuth.CurrentUser && fAuth.CurrentUser != null;

            if (signedIn && fUser != null)
            {
                Debug.Log("Signed out : " + fUser.UserId);
            }

            fUser = fAuth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in : " + fUser.UserId);
            }
        }
    }
    }
