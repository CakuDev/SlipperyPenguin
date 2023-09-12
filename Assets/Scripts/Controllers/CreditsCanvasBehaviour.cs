using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsCanvasBehaviour : MonoBehaviour
{
    public SceneAnimationController sceneAnimationController;
    public void HideSettingsCanvas()
    {
        sceneAnimationController.HideCreditsCanvas();
        sceneAnimationController.ShowMainMenuWithoutSubtitle();
    }
}
