using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
}
