using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellYellow : Shell
{
    public override string shellName { get => "ShellYellow"; }
    protected override void Pick(GameObject player)
    {
        for(int i=0;i<2;i++)
        {
            if (transform.position.x == GameObject.Find("Canvas").GetComponent<GetUI>().shells.data[i].transform.position.x &&
                transform.position.y == GameObject.Find("Canvas").GetComponent<GetUI>().shells.data[i].transform.position.y &&
                transform.position.z == GameObject.Find("Canvas").GetComponent<GetUI>().shells.data[i].transform.position.z)
            {
                GameObject.Find("Canvas").GetComponent<GetUI>().text.text = "这是由一位代号为"+
                    GameObject.Find("Canvas").GetComponent<GetUI>().shells.data[i].prefab+"的前辈留下来的忠告："+
                    GameObject.Find("Canvas").GetComponent<GetUI>().shells.data[i].text;
                GameObject.Find("Canvas").GetComponent<GetUI>().setActive1();
            }
        }
        base.Pick(player);
    }
}
