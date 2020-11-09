using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DialogSystem : MonoBehaviour
{
    public string chatName;
    public GameObject Button;
    public Flowchart flowChart;

    private bool isChat;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(gameObject.tag == "NPC")
        {
            Button.SetActive(true);
        }
        isChat = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(gameObject.tag == "NPC")
        {
            Button.SetActive(false);
        }
        flowChart.StopAllBlocks();
        isChat = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Say();
        }
    }

    void Say()
    {
        if(isChat)
        {
            if(flowChart.HasBlock(chatName))
            {
                flowChart.ExecuteBlock(chatName);
            }
        }
    }
}
