using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{

    public Canvas gameMenuCanvas;
    public Canvas settingsCanvas;

    private Stack<GameObject> pressedButtonHierarchy;
    private Stack<Canvas> canvasHierarchy;
    private GameObject currentFirstElement;
    public InputType currentInputType { get; private set; }

    void Start()
    {
        pressedButtonHierarchy = new();
        canvasHierarchy = new();

        //EventSystem.current.SetSelectedGameObject(null);
        currentFirstElement = FindFirstUIElementChild(gameMenuCanvas.transform);
        currentInputType = InputType.GAMEPAD;
        //EventSystem.current.SetSelectedGameObject(currentFirstElement);
        canvasHierarchy.Push(gameMenuCanvas);
    }

    void Update()
    {
        if (canvasHierarchy.Count > 1 && Input.GetButtonDown("Start") && gameMenuCanvas.isActiveAndEnabled)
        {
            OnClickBack();
        }

        CheckInputType();
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

    public void OnClickBack()
    {
        Canvas currentCanvas = canvasHierarchy.Pop();
        Canvas previousCanvas = canvasHierarchy.Peek();
        currentCanvas.gameObject.SetActive(false);
        previousCanvas.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        currentFirstElement = FindFirstUIElementChild(gameMenuCanvas.transform);
        if (currentInputType.Equals(InputType.GAMEPAD)) EventSystem.current.SetSelectedGameObject(pressedButtonHierarchy.Pop());
    }

    public void StartGame()
    {
        GameObject.FindWithTag(Tags.MUSIC_CONTROLLER).GetComponent<MusicController>().SetGameMusic();
        SceneManager.LoadScene(Scenes.MY_GAME);
    }

    public void EnterSettingsMenu(GameObject pressedButton)
    {
        pressedButtonHierarchy.Push(pressedButton);
        settingsCanvas.gameObject.SetActive(true);
        canvasHierarchy.Push(settingsCanvas);
        EventSystem.current.SetSelectedGameObject(null);
        currentFirstElement = FindFirstUIElementChild(settingsCanvas.transform);
        if(currentInputType.Equals(InputType.GAMEPAD)) EventSystem.current.SetSelectedGameObject(currentFirstElement);
    }

    public void Exit()
    {
    #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
    #endif
    #if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void CheckInputType()
    {
        if (HasInputTypeChanged())
        {
            if(currentInputType.Equals(InputType.KEYBOARD))
            {
                //EventSystem.current.SetSelectedGameObject(null);
            } else
            {
                //EventSystem.current.SetSelectedGameObject(currentFirstElement);
            }
        }
    }

    private bool HasInputTypeChanged()
    {
        Debug.Log($"Horizontal: {Input.GetAxis("Horizontal")}, UI: {Input.GetAxis("HorizontalUI")}");
        if (IsMouseKeyboard())
        {
            InputType current = currentInputType;
            currentInputType = InputType.KEYBOARD;
            return !current.Equals(currentInputType);
        } else if (IsControllerInput())
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
}
