using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEventSystemBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject eventSystem;

    public void EnableEventSystem()
    {
        eventSystem.SetActive(true);
    }
}
