using System;
using System.Collections;
using UnityEngine;

public static class Utils
{
    public static IEnumerator WaitAndDo(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}