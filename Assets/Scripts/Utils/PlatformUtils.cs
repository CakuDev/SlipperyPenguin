using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUtils
{
    public static bool IsPlatformMobile()
    {
        return Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer;
    }
}
