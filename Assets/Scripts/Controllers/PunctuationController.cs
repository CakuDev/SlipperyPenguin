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
    public Animator setNameCanvas;
    public PauseController pauseController;
    public TMP_InputField usernameText;
    public TextMeshProUGUI errorText;
    public string serverError = "An internal error ocurred, try again later.";


    private GameController gameController;
    private GameObject currentFirstElement;
    private InputType currentInputType;
    private readonly string leaderboardKey = "score_leaderboard";
    private bool nameAlreadySet = false;

    // Start is called before the first frame update
    public void LoadPunctuation()
    {
        UserAccountController userAccountController = GameObject.FindWithTag(Tags.USER_ACCOUNT_CONTROLLER).GetComponent<UserAccountController>();
        string username = userAccountController.username;
        if(setNameCanvas != null && (username == "" || (!userAccountController.connectedOnline && !nameAlreadySet)))
        {
            usernameText.text = username;
            setNameCanvas.SetTrigger("spawn");
            return;
        }
        nameAlreadySet = false;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        RowInfo row = new(username, gameController.currentLevel, gameController.score, gameController.timer);
        scoreText.text = "Score: " + gameController.score.ToString();
        levelText.text = "Level: " + gameController.currentLevel.ToString();
        timerText.text = "Time: " + gameController.GetTimerFormat();
        Destroy(gameController.gameObject);
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
                //EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                //EventSystem.current.SetSelectedGameObject(currentFirstElement);
            }
        }
    }

    private bool HasInputTypeChanged()
    {
        if (IsMouseKeyboard())
        {
            InputType current = currentInputType;
            currentInputType = InputType.KEYBOARD;
            return !current.Equals(currentInputType);
        }
        else if (IsControllerInput())
        {
            InputType current = currentInputType;
            currentInputType = InputType.GAMEPAD;
            return !current.Equals(currentInputType);
        }

        return false;
    }

    private bool IsMouseKeyboard()
    {
        // mouse movement
        return Input.GetAxis("Mouse X") != 0.0f ||
            Input.GetAxis("Mouse Y") != 0.0f;
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
        if ((Input.GetAxis("HorizontalUI") > 0.3f || Input.GetAxis("HorizontalUI") < -0.3f) ||
           (Input.GetAxis("VerticalUI") > 0.3f || Input.GetAxis("VerticalUI") < -0.3f))
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
        RectTransform localLeaderboardRect = localLeaderboardList.GetComponent<RectTransform>();
        localLeaderboardRect.anchoredPosition = new(0, -3000);
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
        RectTransform localLeaderboardRect = localLeaderboardList.GetComponent<RectTransform>();
        float width = localLeaderboardRect.sizeDelta.x;
        Vector2 newSize = new(width, leaderboardRowPrefab.GetComponent<RectTransform>().sizeDelta.y * localLeaderboardList.childCount + 10 * localLeaderboardList.childCount);
        localLeaderboardRect.sizeDelta = newSize;
        localLeaderboardRect.anchoredPosition = new(0, -3000);
    }

    private void ManageOnlineLeaderboard(RowInfo row)
    {
        UserAccountController userAccountController = GameObject.FindWithTag(Tags.USER_ACCOUNT_CONTROLLER).GetComponent<UserAccountController>();
        if (!userAccountController.connectedOnline) return;
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
                errorText.text = serverError;
                return;
            }
            LoadOnlineLeaderboardData(row);
        });
    }

    public void LoadOnlineLeaderboardData()
    {
        RectTransform onlineLeaderboardRect = onlineLeaderboardList.GetComponent<RectTransform>();
        onlineLeaderboardRect.anchoredPosition = new(0, -3000);
        LoadOnlineLeaderboardData(null);
    }

    private void LoadOnlineLeaderboardData(RowInfo row)
    {
        errorText.text = "";
        UserAccountController userAccountController = GameObject.FindWithTag(Tags.USER_ACCOUNT_CONTROLLER).GetComponent<UserAccountController>();
        if (!userAccountController.connectedOnline) return;
        if (onlineLeaderboardList.childCount > 0) return;
        int count = 50;
        LootLockerSDKManager.GetScoreList(leaderboardKey, count, 0, (response) =>
        {
            if (response.statusCode == 200)
            {
                List<RowInfo> rows = new();
                for (int i = 0; i < response.items.Length; i++)
                {
                    if (i == 50) break;
                    LootLockerLeaderboardMember currentRow = response.items[i];
                    string name = currentRow.player.name;
                    string[] levelTime = currentRow.metadata.Split(",");
                    int level = int.Parse(levelTime[0]);
                    int time = int.Parse(levelTime[1]);
                    int score = currentRow.score;
                    if(rows.Count > 0 && rows[^1].score == score && rows[^1].time > time)
                    {
                        rows.Insert(rows.Count - 1, new RowInfo(name, level, score, time));
                    } else
                    {
                        rows.Add(new RowInfo(name, level, score, time));
                    }
                }

                for (int i = 0; i < rows.Count; i++)
                {
                    RowInfo row = rows[i];
                    LeaderboardRowBehaviour rowBehaviour = Instantiate(leaderboardRowPrefab, onlineLeaderboardList).GetComponent<LeaderboardRowBehaviour>();
                    rowBehaviour.OnCreate(i + 1, row.name, row.level, row.score, row.time);
                    if (row?.name == name) rowBehaviour.HighlightRow();
                }
                RectTransform onlineLeaderboardRect = onlineLeaderboardList.GetComponent<RectTransform>();
                float width = onlineLeaderboardRect.sizeDelta.x;
                float height = onlineLeaderboardRect.sizeDelta.y;
                Vector2 newSize = new(width, leaderboardRowPrefab.GetComponent<RectTransform>().sizeDelta.y * onlineLeaderboardList.childCount + 10 * onlineLeaderboardList.childCount);
                onlineLeaderboardRect.sizeDelta = newSize;
                onlineLeaderboardRect.anchoredPosition = new(0, -3000);
            }
            else
            {
                errorText.text = serverError;
            }
        });
    }

    public void HideSetNameCanvas()
    {
        setNameCanvas.SetTrigger("hide");
    }

    public void SetUsername()
    {
        nameAlreadySet = true;
        GameObject.FindWithTag(Tags.USER_ACCOUNT_CONTROLLER).GetComponent<UserAccountController>().username = usernameText.text;
    }
}
