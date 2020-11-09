using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image blackImage;
    [SerializeField] private float alpha;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string _SceneName,float _speed)
    {
        StartCoroutine(FadeOut(_SceneName,_speed));
    }

    IEnumerator FadeOut(string SceneName,float speed)
    {
        alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * speed;
            if (alpha > 1)
            {
                alpha = 1;
            }
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
        SceneManager.LoadScene(SceneName);
    }

    IEnumerator FadeIn()
    {
        alpha = 1;
        while(alpha>0)
        {
            alpha -= Time.deltaTime;
            if(alpha<0)
            {
                alpha = 0;
            }
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
    }
}
