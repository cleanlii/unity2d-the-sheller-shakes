using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellManager : MonoBehaviour
{
    private static Dictionary<string, GameObject> shellDict = null;
    private static int lastUUID = -1;
    public Shell[] shells;

    public static bool GetShellPrefab(string name, out GameObject rst)
    {
        rst = null;
        var instance = GameObject.FindObjectOfType<ShellManager>();
        if (instance == null) return false;
        if (instance.GetInstanceID() != lastUUID)
        {
            lastUUID = instance.GetInstanceID();
            shellDict = new Dictionary<string, GameObject>();
            foreach (var shell in instance.shells)
            {
                shellDict[shell.shellName] = shell.gameObject;
            }
        }
        if (!shellDict.ContainsKey(name)) return false;
        rst = shellDict[name];
        return true; ;
    }
}
