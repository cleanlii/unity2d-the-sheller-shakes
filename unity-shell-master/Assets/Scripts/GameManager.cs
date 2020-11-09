using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    [Header("结局数据")]
    public int deathCount;
    public int shellCount;
    public int enemyCount;
    public int gameCount;
    
    [Header("结局条件")]
    public bool finalBoss;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Statistics();

        DontDestroyOnLoad(gameObject);
    }
    public static void Statistics()
    {
        Flowchart flowChart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
        flowChart.SetIntegerVariable("showDeath", instance.deathCount);
        flowChart.SetIntegerVariable("showShell", instance.shellCount);
        flowChart.SetIntegerVariable("showEnemy", instance.enemyCount);
        flowChart.SetIntegerVariable("showNum", 17121904 + instance.deathCount);
    }
    public static void EnemyCalculator()
    {
        instance.enemyCount++;
    }
    public static void ShellCalculator()
    {
        instance.shellCount++;
    }
    public static void Defeat()
    {
        if(instance.deathCount < 10)
        {
            BadEnd();
        }
        if(instance.deathCount > 10)
        {
            LostEnd();
        }
    }

    public static void Victory()
    {
        if(instance.finalBoss == false)
        {
            instance.finalBoss = true;
            instance.NormalEnd();
        }
    }

    public static void FinalVictory()
    {
        if(instance.deathCount > 3 && instance.finalBoss == true)
        {
            instance.FakeEnd();
        }
        else if(instance.deathCount < 3 && instance.finalBoss == true)
        {
            instance.TrueEnd();
        }
    }

    private static void BadEnd()
    {
        instance.deathCount++;
        FindObjectOfType<SceneFader>().FadeTo("BadEnding", 2f);
    }

    private static void LostEnd()
    {
        instance.deathCount++;
        FindObjectOfType<SceneFader>().FadeTo("LostEnding", 2f);
    }

    private void FakeEnd()
    {
        instance.gameCount++;
        FindObjectOfType<SceneFader>().FadeTo("FakeEnding", 2f);
    }
    private void NormalEnd()
    {
        instance.gameCount++;
        FindObjectOfType<SceneFader>().FadeTo("NormalEnding", 2f);
    }
    private void TrueEnd()
    {
        instance.deathCount = 0;
        instance.shellCount = 0;
        instance.gameCount = 0;
        FindObjectOfType<SceneFader>().FadeTo("TrueEnding", 5f);
    }
}
