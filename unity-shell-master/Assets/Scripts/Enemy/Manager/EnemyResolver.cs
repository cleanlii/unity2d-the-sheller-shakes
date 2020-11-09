using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResolver : MonoBehaviour
{
    public EnemyManager manager = null;
    public GameObject baseObject = null;
    public EnemyManager.EnemyType classify = EnemyManager.EnemyType.UNKNOWN;

    private void Start()
    {
        if (baseObject == null) // 逻辑上似乎不需要这个if判断，因为Manager内部判了，但安全起见还是多判定一下
        {
            if (manager != null)
            {
                manager.Register(this);
            }
        }
    }
}
