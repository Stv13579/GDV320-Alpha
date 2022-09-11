using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornyBarb : Item
{
    //A multiplicative number (probably between 0 and 1)
    [SerializeField]
    float damageIncrease = 0;

    StatModifier.Modifier thornyMod;

    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);

        thornyMod = new StatModifier.Modifier();
        //eg if damageIncrease is 0.25, defense will now equal 1.25, multiplying incoming damage, resulting in the player taking 25% more damage
        thornyMod.modifierValue = 1 + damageIncrease;
        thornyMod.modifierSource = "Thorny Barb";

        StatModifier.AddModifier(player.GetDefenseStat().multiplicativeModifiers, thornyMod);
        StatModifier.UpdateValue(player.GetDefenseStat());
    }

    public override void SpawnTrigger(GameObject enemySpawning)
    {
        base.SpawnTrigger(enemySpawning);

        enemySpawning.GetComponent<BaseEnemyClass>().SetDamageResistance(enemySpawning.GetComponent<BaseEnemyClass>().GetDamageResistance() * 1 + damageIncrease);
    }

}
