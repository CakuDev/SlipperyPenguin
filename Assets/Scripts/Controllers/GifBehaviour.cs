using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class GifBehaviour : MonoBehaviour
{
    public Object folder;
    public bool playOnAwake;
    public int framesPerSecond;

    public Sprite[] frames;
    private Image image;
    private float timer;
    private bool isStopped;

    void Awake()
    {
        image = GetComponent<Image>();
        timer = 0;
        isStopped = !playOnAwake;
        string[] assetPaths = Directory.GetFiles("Assets/Resources/" + folder.name, "*.gif");
        frames = new Sprite[assetPaths.Length];
        for (int i = 0; i < assetPaths.Length; i++)
        {
            string[] path = assetPaths[i].Split(folder.name);
            frames[i] = Resources.Load<Sprite>(folder.name + path[1][0..^4]);
        }
    }

    void FixedUpdate()
    {
        if (isStopped) return;
        timer += Time.deltaTime * framesPerSecond;
        int index = (int)(timer % frames.Length);
        image.sprite = frames[index];
    }

    public void Stop()
    {
        isStopped = true;
        timer = 0;
    }

    public void Play()
    {
        isStopped = false;
    }
}
