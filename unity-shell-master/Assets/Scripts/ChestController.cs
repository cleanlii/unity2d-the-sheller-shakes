using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ChestController : MonoBehaviour
{
    private float probRecovery = 50, probDamage = 30, probChange = 20;
    private readonly Random random = new Random();

    private bool _isOpen = false;

    private static readonly int animOpenHash = Animator.StringToHash("open");

    public bool isOpen
    {
        get => _isOpen;
        set
        {
            if (value)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    private void Awake()
    {
        var sum = probRecovery + probDamage + probChange;
        probRecovery /= sum;
        probDamage /= sum;
        probChange /= sum;
    }

    public void Open(GameObject obj = null)
    {
        if (_isOpen) return;
        _isOpen = true;
        if (obj == null) return;
        if (TryGetComponent<Animator>(out var anim))
        {
            anim.SetBool(animOpenHash, _isOpen);
        }
        var pos = (float) random.NextDouble();
        Debug.LogFormat("Probability: {0}", pos);
        if (!obj.TryGetComponent<PlayerBattle>(out var playerBattle)) return;
        if ((pos -= probRecovery) <= 0.0f)
        {
            playerBattle.Recovery(50.0f);
        }
        else if ((pos -= probDamage) <= 0.0f)
        {
            playerBattle.Damage(50.0f);
        }
        else if (obj.TryGetComponent<PlayerSpriteManager>(out var manager))
        {
            manager.SetSpritePlan("red");
        }
    }

    public void Close(GameObject obj = null)
    {
        if (!_isOpen) return;
        _isOpen = false;
        if (TryGetComponent<Animator>(out var anim))
        {
            anim.SetBool(animOpenHash, _isOpen);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isOpen && other.gameObject.CompareTag("Player"))
        {
            Open(other.gameObject);
        }
    }
}