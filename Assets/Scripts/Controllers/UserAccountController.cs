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
    public TextMeshProUGUI errorText;
    public string serverError = "An internal error ocurred, try again later.";
    public string usernameAlreadyExistsError = "Username already exists.";
    public string passwordTooShortError = "The password must be at least 8 characters.";
    public string wrongUsernamePasswordError = "Wrong email and/or password";

    [HideInInspector]
    public string username;
    [HideInInspector]
    public string memberId;
    private readonly string usernameAlreadyExistsResponse = "user with that email already exists";
    private readonly string wrongUsernamePasswordResponse = "wrong email/password";

    // Start is called before the first frame update
    public void SignUp()
    {
        string email = emailInput.text; 
        string password = passwordInput.text;
        if(password.Length < 8)
        {
            errorText.text = passwordTooShortError;
            return;
        }
        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) => {
            Debug.Log("MESSAGE TEXT: " + response.Error);
            if(response.success)
            {
                Debug.Log("User created successfully");
                LogIn(true);
                return;
            }
            if(response.statusCode / 100 == 5)
            {
                errorText.text = serverError;
                return;
            }
            if(response.Error.Contains(usernameAlreadyExistsResponse))
            {
                errorText.text = usernameAlreadyExistsError;
                return;
            }
        });
    }

    public void LogIn(bool isFirstLogin)
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        bool rememberMe = true;
        LootLockerSDKManager.WhiteLabelLoginAndStartSession(email, password, rememberMe, (response) =>
        {
            if (response.success)
            {
                Debug.Log("User created successfully");
                username = email;
                memberId = response.SessionResponse.public_uid;
                userAccountCanvas.SetActive(false);
                if(isFirstLogin)
                {
                    LootLockerSDKManager.SetPlayerName(email, (response) => {
                        if (!response.success) Debug.Log("ERROR SETTING THE NAME");
                    });
                }
                return;
            }
            if (!response.LoginResponse.success)
            {
                if(response.Error.Contains(wrongUsernamePasswordResponse))
                {
                    errorText.text = wrongUsernamePasswordError;
                }
            }
            if (!response.SessionResponse.success)
            {
                Debug.Log("error while starting session");
            }
            return;
        });
    }

    private void OnDestroy()
    {
        if(userAccountCanvas != null) userAccountCanvas.SetActive(false);
    }
}
