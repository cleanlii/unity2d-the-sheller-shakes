using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LostFader : MonoBehaviour
{
    public Image blackImage;
    [SerializeField] private float alpha;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime*1.5f;
            if (alpha < 0)
            {
                alpha = 0;
            }
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
        blackImage.enabled = false;
    }
}
