using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubscribeToUserAccount : MonoBehaviour
{
    public bool showOnline;
    public bool showOffline;

    void Start()
    {
        UserAccountController userAccountController = GameObject.FindWithTag(Tags.USER_ACCOUNT_CONTROLLER).GetComponent<UserAccountController>();
        if (showOffline)
        {
            userAccountController.showOffline.Add(gameObject);
            gameObject.SetActive(!userAccountController.connectedOnline);
        }
        if (showOnline)
        {
            userAccountController.showOnline.Add(gameObject);
            gameObject.SetActive(userAccountController.connectedOnline);
        }
    }
}
