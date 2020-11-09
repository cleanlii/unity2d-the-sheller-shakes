using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellGrey : Shell
{
    public override string shellName { get => "ShellGrey"; }
    protected override void Pick(GameObject player)
    {
        player.GetComponent<PlayerBattle>().Damage(50.0f);
        player.GetComponent<PlayerSpriteManager>().SetSpritePlan("grey");
        base.Pick(player);
    }
}