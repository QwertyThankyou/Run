using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text text;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetFloat("MaxScore", 0);

        Helper.MaxScore = PlayerPrefs.GetFloat("MaxScore");

        text.text = Helper.CurrentScore.ToString();
    }
    
    void Update()
    {
        if (Helper.CurrentScore > Helper.MaxScore)
            SetMaxScore();
    }

    private void FixedUpdate()
    {
        if (!Helper.Lose)
        { 
            Helper.CurrentScore += 1f;
            text.text = Helper.CurrentScore.ToString();
        }
    }

    private void SetMaxScore()
    {
        Helper.MaxScore = Helper.CurrentScore;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("MaxScore", Helper.MaxScore);
        PlayerPrefs.Save();
    }
}
