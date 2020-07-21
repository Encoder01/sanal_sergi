using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{

    public RawImage rawimage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    void Start()
    {
        StartCoroutine(PlayVideo());
    }
    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        rawimage.texture = videoPlayer.texture;
        //videoPlayer.url = "Assets/Resources/Videos/a.mp4";
        videoPlayer.Play();

        audioSource.Play();
    }
}
