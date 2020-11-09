using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellViolet : Shell
{
    public override string shellName { get => "ShellViolet"; }
    protected override void Pick(GameObject player)
    {
        player.GetComponent<PlayerBattle>().ChangeAttack(-50.0f);
        player.GetComponent<PlayerSpriteManager>().SetSpritePlan("violet");
        base.Pick(player);
    }
}