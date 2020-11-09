using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthPointImage;
    public Image healthPointEffect;
    [SerializeField] private float hurtSpeed;

    private PlayerBattle player;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBattle>();     
    }    
    private void Update()
    {
        healthPointImage.fillAmount = player.currentHp / player.maxHp;
        if (healthPointEffect.fillAmount >= healthPointImage.fillAmount)
        {
            healthPointEffect.fillAmount -= hurtSpeed;
            //Debug.Log("hurt");
        }
        else
        {
            healthPointEffect.fillAmount = healthPointImage.fillAmount;
        }
    }

}
