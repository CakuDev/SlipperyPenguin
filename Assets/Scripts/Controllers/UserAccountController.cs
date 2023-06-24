using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;
using LootLocker.Requests;
using TMPro;

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

    //[HideInInspector]
    public string username;
    //[HideInInspector]
    public string memberId;
    private readonly string usernameAlreadyExistsResponse = "user with that email already exists";
    private readonly string wrongUsernamePasswordResponse = "wrong email/password";

    // Start is called before the first frame update
    public void SignUp(SceneAnimationController sceneAnimationController, TMP_InputField emailInput, TMP_InputField passwordInput)
    {
        string email = emailInput.text; 
        string password = passwordInput.text;
        string repeatPassword = repeatPasswordInput.text;
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
                usernameText.text = username;
                memberId = response.SessionResponse.public_uid;
                if(isFirstLogin)
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
                LootLockerSDKManager.StartWhiteLabelSession(response =>
                {
                    if (response.hasError)
                    {
                        sceneAnimationController.ShowLogInMenu();
                        sceneAnimationController.ShowSubtitle();
                        sceneAnimationController.HideTitleCanvas();
                        sceneAnimationController.loadingCanvas.SetActive(false);
                    }
                    else
                    {
                        LootLockerSDKManager.GetPlayerName(response =>
                        {
                            username = response.name;
                            usernameText.text = username;
                            sceneAnimationController.ShowMainMenu();
                            sceneAnimationController.HideTitleCanvas();
                            sceneAnimationController.loadingCanvas.SetActive(false);
                        });
                        memberId = response.public_uid;
                    }
                });
            }
            else
            {
                sceneAnimationController.ShowLogInMenu();
                sceneAnimationController.ShowSubtitle();
                sceneAnimationController.HideTitleCanvas();
                sceneAnimationController.loadingCanvas.SetActive(false);
                loadingCanvas.SetActive(false);
            }
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
                sceneAnimationController.ShowLogInMenu();
                sceneAnimationController.HideMainMenu();
                sceneAnimationController.loadingCanvas.SetActive(false);
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
                if (guestResponse.seen_before)
                {
                    LootLockerSDKManager.GetPlayerName(response =>
                    {
                        username = response.name;
                        usernameText.text = username;
                        sceneAnimationController.ShowMainMenu();
                        sceneAnimationController.HideTitleCanvas();
                        sceneAnimationController.HideLogInMenu();
                        
                    });
                } else
                {
                    // TODO: Hide menu and show username input and set name button
                }
            }
            sceneAnimationController.loadingCanvas.SetActive(false);
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
                username = email;
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
