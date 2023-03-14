using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PunctuationController : MonoBehaviour
{

    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text timerText;
    
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        scoreText.text = "Score: " + gameController.score.ToString();
        levelText.text = "Level: " + gameController.currentLevel.ToString();
        timerText.text = "Time: " + gameController.GetTimerFormat();
        DontDestroyBehaviour.DestroyObject(gameController.gameObject);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Play Again Button"));
    }

    public void PlayAgain()
    {
        GameObject.FindWithTag(Tags.MUSIC_CONTROLLER).GetComponent<MusicController>().SetGameMusic();
        SceneManager.LoadScene(Scenes.MY_GAME);
    }

    public void Exit()
    {
        SceneManager.LoadScene(Scenes.GAME_MENU);
    }
}
