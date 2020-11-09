using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextInExtra : MonoBehaviour
{
    private bool isUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && isUsed)
        {
            FindObjectOfType<SceneFader>().FadeTo("MainLevel", 1f);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" )
        {
            isUsed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isUsed = false;
        }
    }
}
