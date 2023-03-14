using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{

    public Canvas gameMenuCanvas;
    public Canvas settingsCanvas;

    private Stack<GameObject> pressedButtonHierarchy;
    private Stack<Canvas> canvasHierarchy;

    void Start()
    {
        pressedButtonHierarchy = new();
        canvasHierarchy = new();

        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(FindFirstUIElementChild(gameMenuCanvas.transform));
        canvasHierarchy.Push(gameMenuCanvas);
    }

    void Update()
    {
        if (canvasHierarchy.Count > 1 && Input.GetButtonDown("Start"))
        {
            OnClickBack();
        }
    }

    public GameObject FindFirstUIElementChild(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(Tags.UI_ELEMENT))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    public void OnClickBack()
    {
        Canvas currentCanvas = canvasHierarchy.Pop();
        Canvas previousCanvas = canvasHierarchy.Peek();
        currentCanvas.gameObject.SetActive(false);
        previousCanvas.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pressedButtonHierarchy.Pop());
    }

    public void StartGame()
    {
        GameObject.FindWithTag(Tags.MUSIC_CONTROLLER).GetComponent<MusicController>().SetGameMusic();
        SceneManager.LoadScene(Scenes.MY_GAME);
    }

    public void EnterSettingsMenu(GameObject pressedButton)
    {
        pressedButtonHierarchy.Push(pressedButton);
        settingsCanvas.gameObject.SetActive(true);
        canvasHierarchy.Push(settingsCanvas);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(FindFirstUIElementChild(settingsCanvas.transform));
    }

    public void Exit()
    {
    #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
    #endif
    #if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
    #elif (UNITY_STANDALONE)
        Application.Quit();
    #endif
    }
}
