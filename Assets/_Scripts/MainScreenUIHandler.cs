using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenUIHandler : MonoBehaviour
{
    public TMP_InputField UsernameField;
    public Slider DifficultySlider;
    [SerializeField]
    private LoginManager loginManager;
    [SerializeField]
    private DifficultyManager difficultyManager;

    public void Start()
    {
        LoginManager loginManager = FindObjectOfType<LoginManager>();
        difficultyManager = DifficultyManager.instance;
        if (difficultyManager != null)
        {
            DifficultySlider.value = difficultyManager.GetDifficulty();
        }
    }

    public void LoginUser()
    {
        loginManager.LoginUser(UsernameField.text);
    }

    public void SetDifficulty()
    {
        if(difficultyManager == null) {  difficultyManager = DifficultyManager.instance; }
        difficultyManager.SetDifficulty((int)(DifficultySlider.value));
    }
}
