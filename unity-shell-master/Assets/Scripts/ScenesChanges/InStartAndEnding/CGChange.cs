using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CGChange : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void Update()
    {
        if(videoPlayer.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("StartMenu");
            }
        }
    }

    private void EndReached(VideoPlayer videoPlayer)
    {
        //Debug.Log("End reached!");
        SceneManager.LoadScene("StartMenu");
    }
}
