using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lost : MonoBehaviour
{
    public bool isLost = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("you lost");
            //SceneManager.LoadScene("LostEnding");
            isLost = true;
            FindObjectOfType<PlayerBattle>().ShellConnect();
            AudioManeger.playerDeathAudio();
            //FindObjectOfType<SceneFader>().FadeTo("LostEnding",1.5f);
        }
        isLost = false;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        isLost = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
