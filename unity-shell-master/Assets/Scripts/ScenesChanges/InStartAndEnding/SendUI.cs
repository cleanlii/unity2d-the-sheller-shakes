using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SendUI : MonoBehaviour
{
    public GameObject sendUI;
    public static string name;
    public static string words;
    public InputField inputName;
    public InputField inputWords;

    public void Start()
    {
        //调用监听输入框变化与结果执行
        inputName.onValueChanged.AddListener(delegate { ValueNameCheck(); });

        inputWords.onValueChanged.AddListener(delegate{ ValueWordsCheck(); });

        inputName.onEndEdit.AddListener(delegate { EndNameCheck(); });

        inputWords.onEndEdit.AddListener(delegate { EndWordsCheck(); });

    }
    public void ValueNameCheck()
    {
        name = inputName.text;
        Debug.Log(name);
    }
    public void ValueWordsCheck()
    {
        words= inputWords.text;
        Debug.Log(words);
    }
    public void EndNameCheck()
    {
        inputWords.ActivateInputField();
 
    }
    public void EndWordsCheck()
    {
        sendUI.SetActive(false);
        Time.timeScale = 1f;
        var shell = GameObject.Find("Player").GetComponent<PlayerBattle>().deadShell;
        StartCoroutine(Front.IEPostCreateShell(words, name, "BUG4", shell.GetComponent<Shell>().shellName, shell));
        GameObject.Find("Player").gameObject.SetActive(false);
        GameManager.Defeat();
    }
    public void setActive()
    {
        sendUI.SetActive(true);
        inputName.ActivateInputField();//高亮Name输入框
        Time.timeScale = 0.1f;
    }

}
