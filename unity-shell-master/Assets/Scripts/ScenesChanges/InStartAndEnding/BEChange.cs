﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BEChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManeger.BadAudio();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}
