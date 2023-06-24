using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneAnimationController : MonoBehaviour
{
    public Animator titleCanvas;
    public Animator subtitleCanvas;
    public Animator gameMenuCanvas;
    public Animator settingsCanvas;
    public Animator creditsCanvas;
    public Animator tutorialCanvas;
    public Animator logInCanvas;
    public TextMeshProUGUI usernameText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Animator iceberg;
    public UserAccountController userAccountController;
    public GameObject loadingCanvas;

    private SceneAnimationDataController sceneAnimationDataController;
    private bool alreadySkipped = false;
    private void Awake()
    {
        sceneAnimationDataController = GameObject.FindWithTag(Tags.SCENE_ANIMATION_DATA_CONTROLLER).GetComponent<SceneAnimationDataController>();
        if (!sceneAnimationDataController.isFirstTimeInMenu)
        {
            ShowMainMenu();
            HideTitleCanvas();
            userAccountController = GameObject.FindWithTag(Tags.USER_ACCOUNT_CONTROLLER).GetComponent<UserAccountController>();
            usernameText.text = userAccountController.username;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!alreadySkipped && (Input.anyKeyDown || IsControllerInput()))
        {
            CheckExistingSessionAndChangeCanvas();
        }
    }

    void CheckExistingSessionAndChangeCanvas()
    {
        loadingCanvas.SetActive(true);
        userAccountController.CheckExistingSession(this, loadingCanvas);
        sceneAnimationDataController.isFirstTimeInMenu = false;
        alreadySkipped = true;
    }

    public void ShowLogInMenu()
    {
        logInCanvas.SetTrigger("spawn");
        alreadySkipped = true;
    }

    public void ShowSignUpInputs()
    {
        logInCanvas.SetTrigger("showSignUp");
    }

    public void ShowLogInInputs()
    {
        logInCanvas.SetTrigger("showLogIn");
    }

    public void HideTitleCanvas()
    {
        titleCanvas.SetTrigger("close");
    }

    public void ShowMainMenu()
    {
        subtitleCanvas.SetTrigger("spawn");
        iceberg.SetTrigger("spawn");
        gameMenuCanvas.SetTrigger("spawn");
        alreadySkipped = true;
    }

    public void HideMainMenu()
    {
        iceberg.SetTrigger("hide");
        gameMenuCanvas.SetTrigger("hide");
    }

    public void ShowSubtitle()
    {
        subtitleCanvas.SetTrigger("spawn");
    }

    public void HideSubtitle()
    {
        subtitleCanvas.SetTrigger("hide");
    }

    public void HideLogInMenu()
    {
        logInCanvas.SetTrigger("hide");
    }

    public void LogOutAndChangeScene()
    {
        loadingCanvas.SetActive(true);
        userAccountController.LogOut(this);
    }

    public void LogInAndChangeScene()
    {
        loadingCanvas.SetActive(true);
        userAccountController.LogIn(false, this, emailInput, passwordInput);
    }

    public void SignUpAndChangeScene()
    {
        loadingCanvas.SetActive(true);
        userAccountController.SignUp(this, emailInput, passwordInput);
    }

    public void LogInAsGuestAndChangeScene()
    {
        loadingCanvas.SetActive(true);
        userAccountController.LogInAsGuest(this);
    }

    public void SetNameAndChangeScene()
    {
        loadingCanvas.SetActive(true);
        userAccountController.SetNameAndLogIn(this, emailInput);
    }

    public void ShowSettingsMenu()
    {
        settingsCanvas.SetTrigger("spawn");
    }

    public void HideSettingsMenu()
    {
        settingsCanvas.SetTrigger("hide");
    }

    public void ShowCreditsCanvas()
    {
        creditsCanvas.SetTrigger("spawn");
    }

    public void HideCreditsCanvas()
    {
        creditsCanvas.SetTrigger("hide");
    }

    public void ShowTutorialCanvas()
    {
        tutorialCanvas.SetTrigger("spawn");
    }

    public void HideTutorialCanvas()
    {
        tutorialCanvas.SetTrigger("hide");
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
