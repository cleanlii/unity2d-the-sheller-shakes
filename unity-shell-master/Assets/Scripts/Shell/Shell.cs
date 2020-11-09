using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(order = 400, menuName = "ShellPrefabList")]
public class ShellContainer: ScriptableObject
{
    [SerializeField]
    public List<GameObject> shells = new List<GameObject>();
}

public abstract class Shell : MonoBehaviour
{
    public abstract string shellName {  get;  }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject;
        if (!player.CompareTag("Player")) return;
        if (other.TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            playerMovement.OnSquat += Pick;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        var player = other.gameObject;
        if (!player.CompareTag("Player")) return;
        if (other.TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            playerMovement.OnSquat -= Pick;
        }
    }

    protected virtual void Pick(GameObject player)
    {
        GameManager.ShellCalculator();
        Destroy(gameObject);
    }
}