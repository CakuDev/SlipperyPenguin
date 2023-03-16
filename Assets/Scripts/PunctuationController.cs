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
    private GameObject currentFirstElement;
    private InputType currentInputType;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        scoreText.text = "Score: " + gameController.score.ToString();
        levelText.text = "Level: " + gameController.currentLevel.ToString();
        timerText.text = "Time: " + gameController.GetTimerFormat();
        DontDestroyBehaviour.DestroyObject(gameController.gameObject);

        EventSystem.current.SetSelectedGameObject(null);
        currentFirstElement = GameObject.Find("Play Again Button");
        EventSystem.current.SetSelectedGameObject(currentFirstElement);
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
            Debug.Log("CHANGE TO: " + currentInputType.ToString());
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
        if (Input.GetAxis("Mouse X") != 0.0f ||
            Input.GetAxis("Mouse Y") != 0.0f)
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
        if (Input.GetAxis("Horizontal") != 0.0f ||
           Input.GetAxis("Vertical") != 0.0f)
        {
            return true;
        }

        return false;
    }
}
