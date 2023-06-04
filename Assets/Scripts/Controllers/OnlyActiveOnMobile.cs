using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyActiveOnMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(PlatformUtils.IsPlatformMobile());
    }
}
