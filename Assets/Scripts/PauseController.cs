using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{

    public Canvas mainMenuCanvas;
    public Canvas settingsCanvas;

    private bool isPaused;
    private Stack<GameObject> pressedButtonHierarchy;
    private Stack<Canvas> canvasHierarchy;

    // Start is called before the first frame update
    void Start()
    {
        pressedButtonHierarchy = new();
        canvasHierarchy = new();
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPaused && Input.GetButtonDown("Start"))
        {
            OnClickPause();
        } else if(isPaused && Input.GetButtonDown("Start"))
        {
            if(canvasHierarchy.Count == 1)
            {
                OnClickContinue();
            } else
            {
                OnClickBack();
            }
           
        }
    }

    public void OnClickPause()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        isPaused = true;
        mainMenuCanvas.gameObject.SetActive(true);
        canvasHierarchy.Push(mainMenuCanvas);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(FindFirstUIElementChild(mainMenuCanvas.transform));
    }

    public void OnClickContinue()
    {
        canvasHierarchy.Clear();
        Time.timeScale = 1;
        AudioListener.pause = false;
        isPaused = false;
        mainMenuCanvas.gameObject.SetActive(false);
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

    public void EnterSettingsMenu(GameObject pressedButton)
    {
        pressedButtonHierarchy.Push(pressedButton);
        mainMenuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
        canvasHierarchy.Push(settingsCanvas);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(FindFirstUIElementChild(settingsCanvas.transform));
    }

    public void ExitSettingsMenu()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pressedButtonHierarchy.Pop());
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }

    public GameObject FindFirstUIElementChild(Transform parent)
    {
        foreach(Transform child in parent)
        {
            if(child.CompareTag(Tags.UI_ELEMENT))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    public void ExitGame()
    {
        GameObject.FindWithTag(Tags.MUSIC_CONTROLLER).GetComponent<MusicController>().SetIntroMusic();
        SceneManager.LoadScene(Scenes.GAME_MENU);
    }
}
