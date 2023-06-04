using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOnTouch : MonoBehaviour
{
    public GameObject objectToManage;

    // Update is called once per frame
    void Update()
    {
        objectToManage.SetActive(Input.touchCount == 0);
    }
}
