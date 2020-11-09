using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteController : MonoBehaviour
{
    private static readonly int Error = Shader.PropertyToID("_Error");

    public bool getChild = false;
    public Material material = null;
    private bool _isWhite = false;
    public bool status = false;

    private int changeID = 0;

    void Start()
    {
        var renders = getChild ? GetComponentsInChildren<SpriteRenderer>() : GetComponents<SpriteRenderer>();
        foreach (var render in renders)
        {
            render.material = material;
        }
    }

    public void setWhite(bool flag)
    {
        status = flag;
        ++changeID;
    }

    public void setWhite(float time)
    {
        var id = ++changeID;
        status = true;
        StartCoroutine(Utils.WaitAndDo(time, () =>
            {
                if (id != changeID) return;
                status = false;
            }
        ));
    }

    void Update()
    {
        if (_isWhite != status)
        {
            _isWhite = status;
            var renders = getChild ? GetComponentsInChildren<SpriteRenderer>() : GetComponents<SpriteRenderer>();
            foreach (var render in renders)
            {
                render.material.SetInt(Error, status ? 1 : 0);
            }
        }
    }
}