using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    public string volumeType;
    private MusicController musicController;

    // Start is called before the first frame update
    void Start()
    {
        musicController = GameObject.FindWithTag(Tags.MUSIC_CONTROLLER).GetComponent<MusicController>();
        GetComponent<Slider>().value = musicController.GetVolumeLevel(volumeType);
    }

    public void SetVolume(float value)
    {
        musicController.SetVolume(volumeType, value);
    }
}
