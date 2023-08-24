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
    public Animator leaderboardCanvas;
    public TextMeshProUGUI usernameText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Animator iceberg;
    public UserAccountController userAccountController;
    public GameObject loadingCanvas;
    [field: SerializeField] public GameObject EventSystem { get; private set; }

    private SceneAnimationDataController sceneAnimationDataController;
    private bool alreadySkipped = false;
    private bool titleDelay = false;

    private void Awake()
    {
        sceneAnimationDataController = GameObject.FindWithTag(Tags.SCENE_ANIMATION_DATA_CONTROLLER).GetComponent<SceneAnimationDataController>();
        if (!sceneAnimationDataController.isFirstTimeInMenu)
        {
            EventSystem.SetActive(false);
            ShowMainMenu();
            HideTitleCanvas();
            userAccountController = GameObject.FindWithTag(Tags.USER_ACCOUNT_CONTROLLER).GetComponent<UserAccountController>();
            usernameText.text = userAccountController.username;
        } else
        {
            Invoke(nameof(TitleDelay), 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (titleDelay && !alreadySkipped && (Input.anyKeyDown || ControllerButtons()))
        {
            CheckExistingSessionAndChangeCanvas();
        }
    }

    void TitleDelay()
    {
        titleDelay = true;
    }

    void CheckExistingSessionAndChangeCanvas()
    {
        EventSystem.SetActive(false);
        loadingCanvas.SetActive(true);
        userAccountController.CheckExistingSession(this, loadingCanvas);
        sceneAnimationDataController.isFirstTimeInMenu = false;
        alreadySkipped = true;
    }

    public void ShowLogInMenu()
    {
        EventSystem.SetActive(false);
        logInCanvas.SetTrigger("spawn");
        alreadySkipped = true;
    }

    public void ShowGuestInMenu()
    {
        EventSystem.SetActive(false);
        logInCanvas.SetTrigger("showGuest");
    }

    public void ShowSignUpInputs()
    {
        EventSystem.SetActive(false);
        logInCanvas.SetTrigger("showSignUp");
    }

    public void ShowLogInInputs()
    {
        EventSystem.SetActive(false);
        logInCanvas.SetTrigger("showLogIn");
    }

    public void HideTitleCanvas()
    {
        EventSystem.SetActive(false);
        titleCanvas.SetTrigger("close");
    }

    public void ShowMainMenu()
    {
        EventSystem.SetActive(false);
        subtitleCanvas.SetTrigger("spawn");
        iceberg.SetTrigger("spawn");
        gameMenuCanvas.SetTrigger("spawn");
        alreadySkipped = true;
    }

    public void HideMainMenu()
    {
        EventSystem.SetActive(false);
        iceberg.SetTrigger("hide");
        gameMenuCanvas.SetTrigger("hide");
    }

    public void ShowSubtitle()
    {
        EventSystem.SetActive(false);
        subtitleCanvas.SetTrigger("spawn");
    }

    public void HideSubtitle()
    {
        EventSystem.SetActive(false);
        subtitleCanvas.SetTrigger("hide");
    }

    public void HideLogInMenu()
    {
        EventSystem.SetActive(false);
        logInCanvas.SetTrigger("hide");
    }

    public void LogOutAndChangeScene()
    {
        EventSystem.SetActive(false);
        loadingCanvas.SetActive(true);
        userAccountController.LogOut(this);
    }

    public void LogInAndChangeScene()
    {
        EventSystem.SetActive(false);
        loadingCanvas.SetActive(true);
        userAccountController.LogIn(false, this, emailInput, passwordInput);
    }

    public void SignUpAndChangeScene()
    {
        EventSystem.SetActive(false);
        loadingCanvas.SetActive(true);
        userAccountController.SignUp(this, emailInput, passwordInput);
    }

    public void LogInAsGuestAndChangeScene()
    {
        EventSystem.SetActive(false);
        loadingCanvas.SetActive(true);
        userAccountController.LogInAsGuest(this);
    }

    public void SetNameAndChangeScene()
    {
        EventSystem.SetActive(false);
        loadingCanvas.SetActive(true);
        userAccountController.SetNameAndLogIn(this, emailInput);
    }

    public void ShowSettingsMenu()
    {
        EventSystem.SetActive(false);
        settingsCanvas.SetTrigger("spawn");
    }

    public void HideSettingsMenu()
    {
        EventSystem.SetActive(false);
        settingsCanvas.SetTrigger("hide");
    }

    public void ShowCreditsCanvas()
    {
        EventSystem.SetActive(false);
        creditsCanvas.SetTrigger("spawn");
    }

    public void HideCreditsCanvas()
    {
        EventSystem.SetActive(false);
        creditsCanvas.SetTrigger("hide");
    }

    public void ShowTutorialCanvas()
    {
        EventSystem.SetActive(false);
        tutorialCanvas.SetTrigger("spawn");
    }

    public void HideTutorialCanvas()
    {
        EventSystem.SetActive(false);
        tutorialCanvas.SetTrigger("hide");
    }

    public void ShowLeaderboardCanvas()
    {
        EventSystem.SetActive(false);
        leaderboardCanvas.SetTrigger("spawn");
    }

    public void HideLeaderboardCanvas()
    {
        EventSystem.SetActive(false);
        leaderboardCanvas.SetTrigger("hide");

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

    private bool ControllerButtons()
    {
        return (Input.GetKey(KeyCode.Joystick1Button0) ||
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
           Input.GetKey(KeyCode.Joystick1Button19));
    }
}
