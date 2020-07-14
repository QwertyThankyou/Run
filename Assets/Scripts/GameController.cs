using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController player;

    public Text currentScoreText;
    public Text maxScoreText;

    private float maxScore;
    private float currentScore = 0;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetFloat("MaxScore", 0);

        maxScore = PlayerPrefs.GetFloat("MaxScore");
    }

    private void Start()
    {
        player.onScoreChange.AddListener(delegate {  });
        player.onDeath.AddListener(delegate {  });

        player.speedWalk = 0f;
    }
    
    void Update()
    {
        if (currentScore > maxScore)
            maxScore = currentScore;
        maxScoreText.text = maxScore.ToString();
    }

    public void ScoreChange()
    {
        currentScore++;
        currentScoreText.text = currentScore.ToString();
    }

    public void ScoreReset()
    {
        maxScore = 0;
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetFloat("MaxScore", 0);
    }

    public void StartButton()
    {
        player.speedWalk = 6f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("MaxScore", maxScore);
        PlayerPrefs.Save();
    }
}