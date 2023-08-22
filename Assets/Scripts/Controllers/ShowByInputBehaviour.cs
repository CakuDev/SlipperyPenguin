using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowByInputBehaviour : MonoBehaviour
{
    [SerializeField] private GameMenuController gameMenuController;
    [SerializeField] private List<GameObject> showWithKeyboard;
    [SerializeField] private List<GameObject> showWithGamepad;

    private InputType previousInputType;

    // Start is called before the first frame update
    void Start()
    {
        previousInputType = InputType.KEYBOARD;
        ShowKeyboard();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousInputType == gameMenuController.currentInputType) return;
        if (gameMenuController.currentInputType == InputType.KEYBOARD) ShowKeyboard();
        if (gameMenuController.currentInputType == InputType.GAMEPAD) ShowGamepad();
        previousInputType = gameMenuController.currentInputType;
    }

    private void ShowKeyboard()
    {
        showWithKeyboard.ForEach(go => go.SetActive(true));
        showWithGamepad.ForEach(go => go.SetActive(false));
    }

    private void ShowGamepad()
    {
        showWithKeyboard.ForEach(go => go.SetActive(false));
        showWithGamepad.ForEach(go => go.SetActive(true));
    }
}
