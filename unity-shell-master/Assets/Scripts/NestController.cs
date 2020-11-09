using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NestController : MonoBehaviour
{
    public Sprite openSprite, closeSprite;
    public bool status;
    [SerializeField] private float alpha;
    public Image fader;

    public EnemyManager manager = null;

    public void Start()
    {
        Open();
    }

    public void Open()
    {
        status = true;
        GetComponent<SpriteRenderer>().sprite = openSprite;
        GetComponent<Collider2D>().enabled = true;
    }

    public void Close()
    {
        status = false;
        GetComponent<SpriteRenderer>().sprite = closeSprite;
        GetComponent<Collider2D>().enabled = false;
    }

    private void Pick(GameObject player)
    {
        // if (!status) return;
        var battle = player.GetComponent<PlayerBattle>();
        StartCoroutine(Relax(fader));
        battle.Recovery(battle.MaxHealth());
        Close();
        player.GetComponent<PlayerMovement>().OnSquat -= Pick;
        Debug.Log("Remove Event");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject;
        if (!status || !player.CompareTag("Player")) return;
        if (other.TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            playerMovement.OnSquat += Pick;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.gameObject;
        if (!status || !player.CompareTag("Player")) return;
        if (other.TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            playerMovement.OnSquat -= Pick;
        }
    }

    IEnumerator Relax(Image blackImage)
    {
        alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            if (alpha > 1)
            {
                alpha = 1;
            }
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
        // reset enemies
        manager.ResetEnemy();

        yield return new WaitForSeconds(1);
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            if (alpha < 0)
            {
                alpha = 0;
            }
            blackImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0);
        }
    }
}