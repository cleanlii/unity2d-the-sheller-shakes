using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCyan : Shell
{
    public override string shellName { get => "ShellCyan"; }
    protected override void Pick(GameObject player)
    {
        player.GetComponent<PlayerBattle>().Recovery(50.0f);
        player.GetComponent<PlayerSpriteManager>().SetSpritePlan("red");
        base.Pick(player);
    }
}