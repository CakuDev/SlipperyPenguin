using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.SceneManagement;

public class UserAccountController : MonoBehaviour
{
    public GameObject userAccountCanvas;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField repeatPasswordInput;
    public TextMeshProUGUI errorText;
    public TextMeshProUGUI usernameText;
    public string serverError = "An internal error ocurred, try again later.";
    public string usernameAlreadyExistsError = "Username already exists.";
    public string passwordTooShortError = "The password must be at least 8 characters.";
    public string passwordsDontMatchError = "The passwords don't match.";
    public string wrongUsernamePasswordError = "Wrong email and/or password";
    public string usernameRequiredError = "The username is required";
    public string usernameGuestError = "The username can't start with 'guest'";
    public bool connectedOnline = false;
    [HideInInspector]
    public List<GameObject> showOnline;
    [HideInInspector]
    public List<GameObject> showOffline;

    //[HideInInspector]
    public string username;
    //[HideInInspector]
    public string memberId;
    private readonly string usernameAlreadyExistsResponse = "user with that email already exists";
    private readonly string wrongUsernamePasswordResponse = "wrong email/password";

    public void Awake()
    {
        // Line to delete guest account
        PlayerPrefs.DeleteKey("LootLockerGuestPlayerID");
        ResetShowLists();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        ResetShowLists();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        ManageOnlineGameObjects(connectedOnline);
        ManageOfflineGameObjects(!connectedOnline);
    }

    private void ResetShowLists()
    {
        showOnline = new();
        showOffline = new();
    }

    private void ManageOnlineGameObjects(bool active)
    {
        connectedOnline = active;
        showOnline.ForEach(go => go.SetActive(active));
    }

    private void ManageOfflineGameObjects(bool active)
    {
        showOffline.ForEach(go => go.SetActive(active));
    }

    public void SignUp(SceneAnimationController sceneAnimationController, TMP_InputField emailInput, TMP_InputField passwordInput)
    {
        string email = emailInput.text; 
        string password = passwordInput.text;
        string repeatPassword = repeatPasswordInput.text;
        if(email.Substring(0,6).Contains("guest"))
        {
            errorText.text = usernameGuestError;
            sceneAnimationController.loadingCanvas.SetActive(false);
            return;
        }

        if(password.Length < 8)
        {
            errorText.text = passwordTooShortError;
            sceneAnimationController.loadingCanvas.SetActive(false);
            return;
        }
        if(!repeatPassword.Equals(password))
        {
            errorText.text = passwordsDontMatchError;
            sceneAnimationController.loadingCanvas.SetActive(false);
            return;
        }
        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) => {
            if(response.success)
            {
                LogIn(true, sceneAnimationController, emailInput, passwordInput);
                return;
            }
            if(response.Error.Contains(usernameAlreadyExistsResponse))
            {
                errorText.text = usernameAlreadyExistsError;
                sceneAnimationController.loadingCanvas.SetActive(false);
                return;
            }
            errorText.text = serverError;
            sceneAnimationController.loadingCanvas.SetActive(false);
            return;
        });
    }

    public void LogIn(bool isFirstLogin, SceneAnimationController sceneAnimationController, TMP_InputField emailInput, TMP_InputField passwordInput)
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        bool rememberMe = true;
        LootLockerSDKManager.WhiteLabelLoginAndStartSession(email, password, rememberMe, (response) =>
        {
            if (response.success)
            {
                username = email;
                memberId = response.SessionResponse.public_uid;
                ManageOfflineGameObjects(false);
                ManageOnlineGameObjects(true);
                if (usernameText == null) usernameText = GameObject.Find("Username Text").GetComponent<TextMeshProUGUI>();
                usernameText.text = username;
                if (isFirstLogin)
                {
                    LootLockerSDKManager.SetPlayerName(email, (response) => {
                        if (!response.success) errorText.text = serverError;
                        else
                        {
                            sceneAnimationController.HideLogInMenu();
                            sceneAnimationController.ShowMainMenu();
                            sceneAnimationController.loadingCanvas.SetActive(false);
                            ResetInputs();
                        }
                    });
                } else
                {
                    sceneAnimationController.HideLogInMenu();
                    sceneAnimationController.ShowMainMenu();
                    sceneAnimationController.loadingCanvas.SetActive(false);
                    ResetInputs();
                }
                return;
            }
            sceneAnimationController.loadingCanvas.SetActive(false);
            if (response.LoginResponse != null && !response.LoginResponse.success)
            {
                if (response.Error.Contains(wrongUsernamePasswordResponse))
                {
                    errorText.text = wrongUsernamePasswordError;
                }
            }
            if (response.SessionResponse != null && !response.SessionResponse.success)
            {
                Debug.Log("error while starting session");
            }
            return;
        });
    }

    public void CheckExistingSession(SceneAnimationController sceneAnimationController, GameObject loadingCanvas)
    {
        LootLockerSDKManager.CheckWhiteLabelSession(response =>
        {
            if (response)
            {
                LootLockerSDKManager.StartWhiteLabelSession(responseSession =>
                {
                    if (!responseSession.hasError)
                    {
                        LootLockerSDKManager.GetPlayerName(response =>
                        {
                            memberId = responseSession.public_uid;
                            connectedOnline = true;
                            ManageOfflineGameObjects(false);
                            ManageOnlineGameObjects(true);
                            username = response.name;
                            if (usernameText == null) usernameText = GameObject.Find("Username Text").GetComponent<TextMeshProUGUI>();
                            usernameText.text = username;
                        });
                    }
                });
            } else
            {
                ManageOfflineGameObjects(true);
                ManageOnlineGameObjects(false);
            }
            sceneAnimationController.ShowMainMenu();
            sceneAnimationController.HideTitleCanvas();
            sceneAnimationController.loadingCanvas.SetActive(false);
        });
    }

    public void LogOut(SceneAnimationController sceneAnimationController)
    {
        username = "";
        memberId = "";
        sceneAnimationController.loadingCanvas.SetActive(true);
        LootLockerSDKManager.EndSession(response =>
        {
            if(!response.hasError)
            {
                sceneAnimationController.loadingCanvas.SetActive(false);
                ManageOfflineGameObjects(true);
                ManageOnlineGameObjects(false);
            }
        });
    }

    public void LogInAsGuest(SceneAnimationController sceneAnimationController)
    {
        LootLockerSDKManager.StartGuestSession((guestResponse) =>
        {
            if (!guestResponse.success)
            {
                errorText.text = serverError;
            } else
            {
                memberId = guestResponse.public_uid;
                string email = "guest" + Random.Range(1000, 9999);
                LootLockerSDKManager.SetPlayerName(email, (nameResponse) =>
                {
                    if (!nameResponse.success) errorText.text = serverError;
                    else
                    {
                        ManageOfflineGameObjects(false);
                        ManageOnlineGameObjects(true);
                        username = email;
                        if (usernameText == null) usernameText = GameObject.Find("Username Text").GetComponent<TextMeshProUGUI>();
                        usernameText.text = username;
                        sceneAnimationController.HideLogInMenu();
                        sceneAnimationController.ShowMainMenu();
                        ResetInputs();
                    }
                });
                sceneAnimationController.loadingCanvas.SetActive(false);
            }
        });
    }

    public void SetNameAndLogIn(SceneAnimationController sceneAnimationController, TMP_InputField emailInput)
    {
        string email = emailInput.text;
        if (email.Equals(""))
        {
            errorText.text = usernameRequiredError;
            sceneAnimationController.loadingCanvas.SetActive(false);
            return;
        }
        LootLockerSDKManager.SetPlayerName(email, (nameResponse) =>
        {
            if (!nameResponse.success) errorText.text = serverError;
            else
            {
                ManageOfflineGameObjects(false);
                ManageOnlineGameObjects(true);
                username = email;
                if (usernameText == null) usernameText = GameObject.Find("Username Text").GetComponent<TextMeshProUGUI>();
                usernameText.text = username;
                sceneAnimationController.HideLogInMenu();
                sceneAnimationController.ShowMainMenu();
                ResetInputs();
            }
        });
        sceneAnimationController.loadingCanvas.SetActive(false);
    }

    void ResetInputs()
    {
        emailInput.text = "";
        passwordInput.text = "";
        repeatPasswordInput.text = "";
    }
}
