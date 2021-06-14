using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject countDownPage;
    public GameObject gameOverPage;
    public Text scoreText;

    enum PageState
    {
        None,
        Start,
        Countdown,
        GameOver
    }

    int score = 0;
    bool gameOver = false;

    public bool GameOver { get { return gameOver; } }

    void Awake()
    {
        Instance = this;    
    }

    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerScored += OnPlayerScored;
        TapController.OnyPlayerDied += OnPlayerDied;
    }

    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerScored -= OnPlayerScored;
        TapController.OnyPlayerDied -= OnPlayerDied;
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
         OnGameStarted(); // Event sent to TapController
        score = 0;
        gameOver = false;
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("Highscore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }

        SetPageState(PageState.GameOver);
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                countDownPage.SetActive(false);
                gameOverPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                countDownPage.SetActive(false);
                gameOverPage.SetActive(false);

                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                countDownPage.SetActive(true);
                gameOverPage.SetActive(false);

                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                countDownPage.SetActive(false);
                gameOverPage.SetActive(true);

                break;
        }
    }

    public void StartGame()
    {
        SetPageState(PageState.Countdown);
    }

    public void ConfirmGameOver()
    {
        OnGameOverConfirmed(); // Event sent to TapController
        score = 0;
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }
}
