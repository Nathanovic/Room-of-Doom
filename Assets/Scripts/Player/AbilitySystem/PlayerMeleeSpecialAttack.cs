using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerMeleeSpecialAttack")]
public class PlayerMeleeSpecialAttack : Ability{

    public GameObject projectile;

    private GameObject player;

    public override void Init(GameObject p){
        player = p;
    }

    public override IEnumerator TriggerAbility(){


        yield return null;
    }
}
