using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerMeleeNormalAttack")]
public class PlayerMeleeNormalAttack : Ability{

    public float attackRange;
    public int attackDamage;

    private GameObject player;

    public override void Init(GameObject p){
        player = p;
    }

    public override IEnumerator TriggerAbility(){  
        Debug.Log("PlayerMeleeNormalAttack");
        PlayerCombat pc = player.GetComponent<PlayerCombat>();
        Animator ani = player.GetComponentInChildren<Animator>();

        ani.SetTrigger("attack");

        yield return null;
    }
}
