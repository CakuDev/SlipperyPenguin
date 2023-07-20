using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class PunctuationController : MonoBehaviour
{

    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text timerText;
    public GameObject leaderboardRowPrefab;
    public Transform localLeaderboardList;
    public Transform onlineLeaderboardList;
    public Animator punctuationCanvas;
    public Animator gameUI;
    public PauseController pauseController;
    
    private GameController gameController;
    private GameObject currentFirstElement;
    private InputType currentInputType;
    private readonly string leaderboardKey = "score_leaderboard";

    // Start is called before the first frame update
    public void LoadPunctuation()
    {
        string username = GameObject.FindWithTag(Tags.USER_ACCOUNT_CONTROLLER).GetComponent<UserAccountController>().username;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        RowInfo row = new(username, gameController.currentLevel, gameController.score, gameController.timer);
        scoreText.text = "Score: " + gameController.score.ToString();
        levelText.text = "Level: " + gameController.currentLevel.ToString();
        timerText.text = "Time: " + gameController.GetTimerFormat();
        DontDestroyBehaviour.DestroyObject(gameController.gameObject);
        EventSystem.current.SetSelectedGameObject(null);
        currentFirstElement = GameObject.Find("Play Again Button");
        EventSystem.current.SetSelectedGameObject(currentFirstElement);
        punctuationCanvas.SetTrigger("spawn");
        gameUI.SetTrigger("hide");
        pauseController.hasEnded = true;
        ManageLeaderboard(row);
    }

    void Update()
    {
        CheckInputType();
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

    public GameObject FindFirstUIElementChild(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(Tags.UI_ELEMENT))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    public void CheckInputType()
    {
        if (HasInputTypeChanged())
        {
            if (currentInputType.Equals(InputType.KEYBOARD))
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(currentFirstElement);
            }
        }
    }

    private bool HasInputTypeChanged()
    {
        if (IsMouseKeyboard())
        {
            InputType current = currentInputType;
            currentInputType = InputType.KEYBOARD;
            if (!current.Equals(currentInputType)) Debug.Log(currentInputType);
            return !current.Equals(currentInputType);
        }
        else if (IsControllerInput())
        {
            InputType current = currentInputType;
            currentInputType = InputType.GAMEPAD;
            if (!current.Equals(currentInputType)) Debug.Log(currentInputType);
            return !current.Equals(currentInputType);
        }

        return false;
    }

    private bool IsMouseKeyboard()
    {
        // mouse movement
        if (Input.GetAxis("Mouse X") != 0.0f ||
            Input.GetAxis("Mouse Y") != 0.0f ||
            Input.GetAxis("HorizontalUI") == 0.0f ||
            Input.GetAxis("VerticalUI") == 0.0f)
        {
            return true;
        }
        return false;
    }

    private bool IsControllerInput()
    {
        // joystick buttons
        if (Input.GetKey(KeyCode.Joystick1Button0) ||
           Input.GetKey(KeyCode.Joystick1Button1) ||
           Input.GetKey(KeyCode.Joystick1Button2) ||
           Input.GetKey(KeyCode.Joystick1Button3) ||
           Input.GetKey(KeyCode.Joystick1Button4) ||
           Input.GetKey(KeyCode.Joystick1Button5) ||
           Input.GetKey(KeyCode.Joystick1Button6) ||
           Input.GetKey(KeyCode.Joystick1Button7) ||
           Input.GetKey(KeyCode.Joystick1Button8) ||
           Input.GetKey(KeyCode.Joystick1Button9) ||
           Input.GetKey(KeyCode.Joystick1Button10) ||
           Input.GetKey(KeyCode.Joystick1Button11) ||
           Input.GetKey(KeyCode.Joystick1Button12) ||
           Input.GetKey(KeyCode.Joystick1Button13) ||
           Input.GetKey(KeyCode.Joystick1Button14) ||
           Input.GetKey(KeyCode.Joystick1Button15) ||
           Input.GetKey(KeyCode.Joystick1Button16) ||
           Input.GetKey(KeyCode.Joystick1Button17) ||
           Input.GetKey(KeyCode.Joystick1Button18) ||
           Input.GetKey(KeyCode.Joystick1Button19))
        {
            return true;
        }

        // joystick axis
        if ((Input.GetAxis("Horizontal") != 0.0f && Input.GetAxis("HorizontalUI") == 0.0f) ||
           (Input.GetAxis("Vertical") != 0.0f && Input.GetAxis("VerticalUI") == 0.0f))
        {
            return true;
        }

        return false;
    }

    private void ManageLeaderboard(RowInfo row)
    {
        ManageLocalLeaderboard(row);
        ManageOnlineLeaderboard(row);
    }

    private void ManageLocalLeaderboard(RowInfo row)
    {
        SaveLocalRow(row);
        LoadLocalLeaderboard(row);
    }

    private void SaveLocalRow(RowInfo row)
    {
        LocalLeaderboardRepository repository = GameObject.FindWithTag(Tags.LEADERBOARD_REPOSITORY).GetComponent<LocalLeaderboardRepository>();
        repository.SaveLeaderboardRow(row);
    }

    public void LoadLocalLeaderboard()
    {
        LoadLocalLeaderboard(null);
    }

    private void LoadLocalLeaderboard(RowInfo row)
    {
        if (localLeaderboardList.childCount > 0) return;
        LocalLeaderboardRepository repository = GameObject.FindWithTag(Tags.LEADERBOARD_REPOSITORY).GetComponent<LocalLeaderboardRepository>();
        List<RowInfo> rows = repository.LoadLeaderboardData();
        for (int i = 0; i < rows.Count; i++)
        {
            if (i == 50) break;
            RowInfo currentRow = rows[i];
            LeaderboardRowBehaviour rowBehaviour = Instantiate(leaderboardRowPrefab, localLeaderboardList).GetComponent<LeaderboardRowBehaviour>();
            rowBehaviour.OnCreate(i + 1, currentRow.name, currentRow.level, currentRow.score, currentRow.time);
            if (currentRow.Equals(row)) rowBehaviour.HighlightRow();
        }
    }

    private void ManageOnlineLeaderboard(RowInfo row)
    {
        if (row.score != 0)
            SaveOnlineLeaderboardData(row);
        else
            LoadOnlineLeaderboardData(row);
    }

    private void SaveOnlineLeaderboardData(RowInfo row)
    {
        UserAccountController userAccountController = GameObject.FindWithTag(Tags.USER_ACCOUNT_CONTROLLER).GetComponent<UserAccountController>();
        string memberId = userAccountController.memberId;
        LootLockerSDKManager.SubmitScore(memberId, row.score, leaderboardKey, row.level.ToString() + "," + row.time.ToString(), (response) =>
        {
            if (!response.success)
            {
                Debug.Log("Error while submitting the score");
                return;
            }
            LoadOnlineLeaderboardData(row);
        });
    }

    public void LoadOnlineLeaderboardData()
    {
        LoadOnlineLeaderboardData(null);
    }

    private void LoadOnlineLeaderboardData(RowInfo row)
    {
        if (onlineLeaderboardList.childCount > 0) return;
        int count = 50;
        LootLockerSDKManager.GetScoreList(leaderboardKey, count, 0, (response) =>
        {
            if (response.statusCode == 200)
            {
                for (int i = 0; i < response.items.Length; i++)
                {
                    if (i == 50) break;
                    LootLockerLeaderboardMember currentRow = response.items[i];
                    string name = currentRow.player.name;
                    string[] levelTime = currentRow.metadata.Split(",");
                    int level = int.Parse(levelTime[0]);
                    int time = int.Parse(levelTime[1]);
                    int score = currentRow.score;
                    LeaderboardRowBehaviour rowBehaviour = Instantiate(leaderboardRowPrefab, onlineLeaderboardList).GetComponent<LeaderboardRowBehaviour>();
                    rowBehaviour.OnCreate(i + 1, name, level, score, time);
                    if (row?.name == name) rowBehaviour.HighlightRow();
                }
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }
}
