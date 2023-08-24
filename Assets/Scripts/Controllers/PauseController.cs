using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{

    public Canvas mainMenuCanvas;
    public Canvas settingsCanvas;

    [HideInInspector]
    public bool hasEnded;
    private bool isPaused;
    private Stack<GameObject> pressedButtonHierarchy;
    private Stack<Canvas> canvasHierarchy;
    private GameObject currentFirstElement;
    private InputType currentInputType;

    // Start is called before the first frame update
    void Start()
    {
        pressedButtonHierarchy = new();
        canvasHierarchy = new();
        isPaused = false;
        currentInputType = InputType.GAMEPAD;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckInputType();
        if (hasEnded) return;
        if (!isPaused && Input.GetButtonDown("Start"))
        {
            OnClickPause();
        } else if(isPaused && Input.GetButtonDown("Start"))
        {
            if(canvasHierarchy.Count == 1)
            {
                OnClickContinue();
            } else
            {
                OnClickBack();
            }
        }
    }

    public void OnClickPause()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        isPaused = true;
        mainMenuCanvas.gameObject.SetActive(true);
        canvasHierarchy.Push(mainMenuCanvas);
        // EventSystem.current.SetSelectedGameObject(null);
        currentFirstElement = FindFirstUIElementChild(mainMenuCanvas.transform);
        // if (currentInputType.Equals(InputType.GAMEPAD)) EventSystem.current.SetSelectedGameObject(FindFirstUIElementChild(mainMenuCanvas.transform));
    }

    public void OnClickContinue()
    {
        canvasHierarchy.Clear();
        Time.timeScale = 1;
        AudioListener.pause = false;
        isPaused = false;
        mainMenuCanvas.gameObject.SetActive(false);
    }

    public void OnClickBack()
    {
        Canvas currentCanvas = canvasHierarchy.Pop();
        Canvas previousCanvas = canvasHierarchy.Peek();
        currentCanvas.gameObject.SetActive(false);
        previousCanvas.gameObject.SetActive(true);
        // EventSystem.current.SetSelectedGameObject(null);
        currentFirstElement = FindFirstUIElementChild(mainMenuCanvas.transform);
        // if (currentInputType.Equals(InputType.GAMEPAD)) EventSystem.current.SetSelectedGameObject(pressedButtonHierarchy.Pop());
    }

    public void EnterSettingsMenu(GameObject pressedButton)
    {
        pressedButtonHierarchy.Push(pressedButton);
        mainMenuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
        canvasHierarchy.Push(settingsCanvas);
        // EventSystem.current.SetSelectedGameObject(null);
        currentFirstElement = FindFirstUIElementChild(settingsCanvas.transform);
        // if (currentInputType.Equals(InputType.GAMEPAD)) EventSystem.current.SetSelectedGameObject(currentFirstElement);
    }

    public void ExitSettingsMenu()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        currentFirstElement = FindFirstUIElementChild(mainMenuCanvas.transform);
        if (currentInputType.Equals(InputType.GAMEPAD)) EventSystem.current.SetSelectedGameObject(pressedButtonHierarchy.Pop());
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }

    public GameObject FindFirstUIElementChild(Transform parent)
    {
        foreach(Transform child in parent)
        {
            if(child.CompareTag(Tags.UI_ELEMENT))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    public void ExitGame()
    {
        GameObject.FindWithTag(Tags.MUSIC_CONTROLLER).GetComponent<MusicController>().SetIntroMusic();
        DontDestroyBehaviour.DestroyObject(GameObject.Find("GameController"));
        SceneManager.LoadScene(Scenes.GAME_MENU);
        Time.timeScale = 1;
    }

    //public void CheckInputType()
    //{
    //    if (HasInputTypeChanged())
    //    {
    //        if (currentInputType.Equals(InputType.KEYBOARD))
    //        {
    //            EventSystem.current.SetSelectedGameObject(null);
    //        }
    //        else
    //        {
    //            EventSystem.current.SetSelectedGameObject(currentFirstElement);
    //        }
    //    }
    //}

    //private bool HasInputTypeChanged()
    //{
    //    if (IsMouseKeyboard())
    //    {
    //        InputType current = currentInputType;
    //        currentInputType = InputType.KEYBOARD;
    //        return !current.Equals(currentInputType);
    //    }
    //    else if (IsControllerInput())
    //    {
    //        InputType current = currentInputType;
    //        currentInputType = InputType.GAMEPAD;
    //        return !current.Equals(currentInputType);
    //    }

    //    return false;
    //}

    //private bool IsMouseKeyboard()
    //{
    //    // mouse movement
    //    if (Input.GetAxis("Mouse X") != 0.0f ||
    //        Input.GetAxis("Mouse Y") != 0.0f)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //private bool IsControllerInput()
    //{
    //    // joystick buttons
    //    if (Input.GetKey(KeyCode.Joystick1Button0) ||
    //       Input.GetKey(KeyCode.Joystick1Button1) ||
    //       Input.GetKey(KeyCode.Joystick1Button2) ||
    //       Input.GetKey(KeyCode.Joystick1Button3) ||
    //       Input.GetKey(KeyCode.Joystick1Button4) ||
    //       Input.GetKey(KeyCode.Joystick1Button5) ||
    //       Input.GetKey(KeyCode.Joystick1Button6) ||
    //       Input.GetKey(KeyCode.Joystick1Button7) ||
    //       Input.GetKey(KeyCode.Joystick1Button8) ||
    //       Input.GetKey(KeyCode.Joystick1Button9) ||
    //       Input.GetKey(KeyCode.Joystick1Button10) ||
    //       Input.GetKey(KeyCode.Joystick1Button11) ||
    //       Input.GetKey(KeyCode.Joystick1Button12) ||
    //       Input.GetKey(KeyCode.Joystick1Button13) ||
    //       Input.GetKey(KeyCode.Joystick1Button14) ||
    //       Input.GetKey(KeyCode.Joystick1Button15) ||
    //       Input.GetKey(KeyCode.Joystick1Button16) ||
    //       Input.GetKey(KeyCode.Joystick1Button17) ||
    //       Input.GetKey(KeyCode.Joystick1Button18) ||
    //       Input.GetKey(KeyCode.Joystick1Button19))
    //    {
    //        return true;
    //    }

    //    // joystick axis
    //    if (Input.GetAxis("Horizontal") != 0.0f ||
    //       Input.GetAxis("Vertical") != 0.0f)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}
