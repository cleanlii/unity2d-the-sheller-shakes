using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioTrick : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            GameManager.Statistics();
            AudioManeger.GuideAudio();
            Destroy(gameObject);
        }
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            AudioManeger.ExtraAudio();
            Destroy(gameObject);
        }
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            AudioManeger.BattleAudio();
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
