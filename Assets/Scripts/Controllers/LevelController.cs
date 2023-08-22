using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text timerText;
    public SpawnManager spawnManager;
    public GameController gameController;
    public PunctuationController punctuationController;

    private Coroutine timerCoroutine;
    [HideInInspector]
    public int currentLevel = 1;
    private int score = 0;
    private int timer = 0;
    private int scorePerLevel = 100;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTextOnScreen();
        timerCoroutine = StartCoroutine(IncreaseTimerCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateScore(int aditionalPoints)
    {
        score += aditionalPoints;
        if (score < 0) score = 0;
        
        CheckLevel();
        UpdateTextOnScreen();
    }

    public void UpdateTextOnScreen()
    {
        scoreText.text = "Score: " + score.ToString();
        levelText.text = "Level: " + currentLevel.ToString();
        timerText.text = "Time: " + GetTimerFormat();
    }

    void CheckLevel()
    {
        currentLevel = (score / scorePerLevel) + 1;

        spawnManager.SetSpawnIntervalVariablesByLevel(currentLevel);
        spawnManager.SetEnemiesProbabilitiesByLevel(currentLevel);
    }

    public IEnumerator IncreaseTimerCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            timer++;
            UpdateTextOnScreen();
        }
    }

    public string GetTimerFormat()
    {
        int minutes = timer / 60;
        int seconds = timer % 60;
        return string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
    }

    public void EndGame()
    {
        StopCoroutine(timerCoroutine);
        gameController.SetGameInfo(currentLevel, score, timer);
        string username = GameObject.FindWithTag(Tags.USER_ACCOUNT_CONTROLLER).GetComponent<UserAccountController>().username;
        if(username != null)
        {
            punctuationController.LoadPunctuation();
        } else
        {

        }
        
    }
}
