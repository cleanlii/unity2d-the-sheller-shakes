using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class GetUI : MonoBehaviour
{
    public GameObject getUI;
    public GameObject shellGet;
    // Start is called before the first frame update
    public Text text;
    public Rootobject shells = Front.HttpGet("BUG4", 2);
    public void Start()
    {
        shellCreate();
    }

    public void setActive1()
    {
        getUI.SetActive(true);
    }
    public void shellCreate()
    {
        foreach (var data in shells.data)
        {
            if (ShellManager.GetShellPrefab(data.prefab, out var prefab))
            {
                var shell = GameObject.Instantiate(prefab);
                shell.transform.localPosition = new Vector3(data.transform.position.x, data.transform.position.y, data.transform.position.z);
                shell.transform.localRotation = new Quaternion(data.transform.rotation.x, data.transform.rotation.y, data.transform.rotation.z, data.transform.rotation.w);
                shell.transform.localScale = new Vector3(data.transform.scale.x, data.transform.scale.y, data.transform.scale.z);
            }
        }
    }
    void Update()
    {
        if (getUI.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.Return) || GameObject.Find("Player").GetComponent<PlayerBattle>().currentHp <= 0.0f)
            {
                getUI.SetActive(false);
            }
        }
    }
}
