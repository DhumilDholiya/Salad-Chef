using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private CustomerSpawner spawner;
    private float timeBtwnSpawn = 2f;
    private float currTime;
    private float maxScore;

    public GameObject restartPanel;
    public GameObject pausePanel;
    public Player player1;
    public Player_New player2;

    //scoring system
    public static float _scorePlayer1;
    public static float _scorePlayer2;
    //Main Timer
    public float maxTime = 10;
    public static float _timePlayer1;
    public static float _timePlayer2;



    //UI for Score and Timer
    public Text scorePlayer1;
    public Text scorePlayer2;
    public Text timePlayer1;
    public Text timePlayer2;
    public Text winnerPlayer;

    private void Start()
    {
        spawner = GetComponent<CustomerSpawner>();
        maxScore = float.MinValue;
        _scorePlayer1 = 0f;
        _scorePlayer2 = 0f;
        _timePlayer1 = maxTime;
        _timePlayer2 = maxTime;

    }

    private void Update()
    {
        OnPause();

        if (Time.time >= currTime)
        {
            currTime = Time.time + timeBtwnSpawn;
         //   Debug.Log("Spawn.");
            spawner.SpawnCustomer();
        }
        //updating Time
        if (_timePlayer1 >= 0)
        {
            _timePlayer1 = LoseTime(_timePlayer1);
        }
        if (_timePlayer2 >= 0)
        {
            _timePlayer2 = LoseTime(_timePlayer2);
        }

        //update UI && Time.
        UpdateUI();

        maxScore = Mathf.Max(_scorePlayer1, _scorePlayer2);

        if((int)_timePlayer1 <= 0)
        {
            player1.speed = 0f;
        }
        if((int)_timePlayer2 <= 0)
        {
            player2.speed = 0f;
        }
        if((int)_timePlayer1 == 0 && (int)_timePlayer2 == 0)
        {
            if (maxScore == _scorePlayer1) winnerPlayer.text = "Winner is Player1 with Score : " + maxScore;
            else if (maxScore == _scorePlayer2) winnerPlayer.text = "Winner is Player2 with Score : " + maxScore;
            else winnerPlayer.text = "Game Draw is by score : " + maxScore;
            restartPanel.SetActive(true);
        }
    }

    void UpdateUI()
    {
        scorePlayer1.text = "Score : " + _scorePlayer1;
        scorePlayer2.text = "Score : " + _scorePlayer2;
        timePlayer1.text = "Time : " +(int) _timePlayer1;
        timePlayer2.text = "Time : " + (int)_timePlayer2;
    }

    void OnPause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void OnResume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    float LoseTime(float time)
    {
        time -= Time.deltaTime;
        return time;
    }


    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    public void GoToMainMenu()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene("MainMenu");
    }

}

