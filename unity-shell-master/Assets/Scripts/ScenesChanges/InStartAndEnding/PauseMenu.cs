using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public RectTransform pick;
    public GameObject pauseMenu;
    public void QuitGame()
    {
        Application.Quit();
    }
    private void Update()
    {
        var PositionX = new float[4] { -48.7f, -41.5f, -47.9f ,-22.2f};
        var PositionY = new float[4] { 92.0f, 46.3f, -45.6f,-91.6f };
        Vector2 pic = pick.anchoredPosition;
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeInHierarchy == true)
        {
            AudioManeger.MenuAudio();
            pauseMenu.SetActive(false);
            pic.x = 1000;
            pic.y = 1000;
            pick.anchoredPosition = pic;
            Time.timeScale = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManeger.MenuAudio();
            pauseMenu.SetActive(true);
            pic.x = PositionX[0];
            pic.y = PositionY[0];
            pick.anchoredPosition = pic;
            Time.timeScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && pauseMenu.activeInHierarchy == true)
        {
            AudioManeger.PickAudio();
            if (pic.y == PositionY[0] && pic.x == PositionX[0])
            {
                pic.x = PositionX[3];
                pic.y = PositionY[3];
                pick.anchoredPosition= pic;
            }
            else
            {
                for (int j = 3; j > 0; j--)
                {
                    if (pick.anchoredPosition.y == PositionY[j] && pick.anchoredPosition.x == PositionX[j])
                    {
                        j--;
                        pic.y = PositionY[j];
                        pic.x = PositionX[j];
                        pick.anchoredPosition = pic;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && pauseMenu.activeInHierarchy == true)
        {
            AudioManeger.PickAudio();
            if (pic.y == PositionY[3] && pic.x == PositionX[3])
            {
                pic.x = PositionX[0];
                pic.y = PositionY[0];
                pick.anchoredPosition= pic;
            }
            else
            {
                for (int j = 0; j < 3; j++)
                {
                    if (pick.anchoredPosition.y == PositionY[j] && pick.anchoredPosition.x == PositionX[j])
                    {
                        j++;
                        pic.y = PositionY[j];
                        pic.x = PositionX[j];
                        pick.anchoredPosition = pic;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) && pauseMenu.activeInHierarchy == true )
        {
            AudioManeger.MenuAudio();
            if (pic.y == PositionY[0] && pic.x == PositionX[0])
            {
                pauseMenu.SetActive(false);
                pic.x = 1000;
                pic.y = 1000;
                pick.anchoredPosition = pic;
                Time.timeScale = 1f;
            }
            else if (pic.y == PositionY[1] && pic.x == PositionX[1])
            {
                FindObjectOfType<SceneFader>().FadeTo("GuideLevel",1f);
                Time.timeScale = 1f;
            }
            else if (pic.y == PositionY[3] && pic.x == PositionX[3])
            {
                FindObjectOfType<SceneFader>().FadeTo("StartMenu",1.5f);
                Time.timeScale = 1f;
            }

        }
    }
}
