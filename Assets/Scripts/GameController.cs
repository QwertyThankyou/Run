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

    private bool _speedCh = false;

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

        FindObjectOfType<AudioManager>().Play("MainTheme");

        player.speedWalk = 0f;
    }
    
    void Update()
    {
        if (currentScore > maxScore)
            maxScore = currentScore;
        maxScoreText.text = maxScore.ToString();

        if (currentScore % 10 == 0 && currentScore != 0 && _speedCh == false) StartCoroutine(SpeedUp());
    }

    private IEnumerator SpeedUp()
    {
        _speedCh = true;
        if (player.speedWalk <= 8.2f) 
            player.speedWalk += 0.1f;
        yield return new WaitForSeconds(5f);
        _speedCh = false;
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
        player._animator.SetTrigger("Run");
        player.speedWalk = 6f;
        player.isDeath = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("MaxScore", maxScore);
        PlayerPrefs.Save();
    }
}