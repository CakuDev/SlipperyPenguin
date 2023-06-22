using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTextInPlatformBehaviour : MonoBehaviour
{

    public TextMeshProUGUI textUI;
    public string computerText;
    public string mobileText;
    
    void Start()
    {
        textUI.text = PlatformUtils.IsPlatformMobile() ? mobileText : computerText;
    }
}
