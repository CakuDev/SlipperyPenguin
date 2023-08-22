using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyActiveOnMobile : MonoBehaviour
{
    public bool showOnMobile = true;

    // Start is called before the first frame update
    void Start()
    {
        bool isMobile = PlatformUtils.IsPlatformMobile();
        gameObject.SetActive(showOnMobile ? isMobile : !isMobile);
    }
}
