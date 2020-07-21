using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public AudioManager audioManager;

    public Text currentScoreText;
    public Text maxScoreText;
    
    [Header("Volume button")]
    public Sprite volumeOn;
    public Sprite volumeOff;
    public Button volume;

    public GameObject mockery;
    public GameObject mainMenu;

    private float _maxScore;
    private float _currentScore = 0;

    private bool _speedCh = false;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetFloat("MaxScore", 0);

        if (!PlayerPrefs.HasKey("Volume"))
            PlayerPrefs.SetInt("Volume", 1);

        _maxScore = PlayerPrefs.GetFloat("MaxScore");
    
        player.onScoreChange.AddListener(delegate { });
        player.onDeath.AddListener(delegate { });

        if (PlayerPrefs.GetInt("Volume") == 1)
            audioManager.Play("MainTheme");

        player.speedWalk = 0f;
        
        VolumeImageSwitch();
    }
    
    void Update()
    {
        if (_currentScore > _maxScore)
            _maxScore = _currentScore;
        maxScoreText.text = _maxScore.ToString();

        if (_currentScore % 5 == 0 && _currentScore != 0 && _speedCh == false) StartCoroutine(SpeedUp());
    }

    private IEnumerator SpeedUp()
    {
        _speedCh = true;
        if (player.speedWalk <= 8.3f)
            player.speedWalk += 0.2f;
        yield return new WaitForSeconds(5f);
        _speedCh = false;
    }

    public void ScoreChange()
    {
        _currentScore++;
        currentScoreText.text = _currentScore.ToString();
    }

    public void ScoreReset()
    {
        _maxScore = 0;
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetFloat("MaxScore", 0);
    }

    public void StartButton()
    {
        player.animator.SetTrigger("Run");
        player.speedWalk = 6f;
        player.isDeath = false;
    }

    public void VolumeButton()
    {
        if (PlayerPrefs.GetInt("Volume") == 1)
        {
            PlayerPrefs.SetInt("Volume", 0);
            audioManager.Stop("MainTheme");
            VolumeImageSwitch();
        }
        else
        {
            PlayerPrefs.SetInt("Volume", 1);
            audioManager.Play("MainTheme");
            VolumeImageSwitch();
        }
    }

    private void VolumeImageSwitch()
    {
        if (PlayerPrefs.GetInt("Volume") == 0) volume.image.sprite = volumeOff;
        else volume.image.sprite = volumeOn;
    }

    public void Restart()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        PlayerPrefs.SetFloat("MaxScore", _maxScore);
        PlayerPrefs.Save();
    }
}