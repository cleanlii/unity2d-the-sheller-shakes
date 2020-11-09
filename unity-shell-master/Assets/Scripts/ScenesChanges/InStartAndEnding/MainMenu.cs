using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fungus;
public class MainMenu : MonoBehaviour
{
    public Transform pick;
    public void PlayGame()
    {
        AudioManeger.closeLevelAudio();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    void Start()
    {
        AudioManeger.StartAudio();
    }
    private void Update()
    {
        var PositionX = new float[3] { 6.50f,6.50f,6.77f};
        var PositionY = new float[3] { 1.88f, -3.18f, -4.84f };
        Vector2 pic = pick.position;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AudioManeger.PickAudio();
            if (pic.y == PositionY[0] && pic.x == PositionX[0])
            {
                pic.x = PositionX[2];
                pic.y = PositionY[2];
                pick.position = pic;
            }
            else
            {
                for (int j = 2; j > 0; j--)
                {
                    if (pick.position.y == PositionY[j] && pick.position.x == PositionX[j])
                    {
                        j--;
                        pic.y = PositionY[j];
                        pic.x = PositionX[j];
                        pick.position = pic;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AudioManeger.PickAudio();
            if (pic.y == PositionY[2] && pic.x == PositionX[2])
            {
                pic.x = PositionX[0];
                pic.y = PositionY[0];
                pick.position = pic;
            }
            else
            {
                for (int j = 0; j <2; j++)
                {
                    if (pick.position.y == PositionY[j] && pick.position.x == PositionX[j])
                    {
                        j++;
                        pic.y = PositionY[j];
                        pic.x = PositionX[j];
                        pick.position = pic;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AudioManeger.PickAudio();
            if (pic.y == PositionY[0] && pic.x == PositionX[0])
            {
                PlayGame();
            }
            else if (pic.y == PositionY[2] && pic.x == PositionX[2])
            {
                QuitGame();
            }
 
        }
    }
}
