using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerMeleeNormalAttack")]
public class PlayerMeleeNormalAttack : Ability{

    public float attackRange;
    public int attackDamage;

    private GameObject player;

    public override void Init(GameObject player){
        this.player = player;

    }

    public override IEnumerator TriggerAbility(){  
        Debug.Log("PlayerMeleeNormalAttack");
        PlayerCombat pc = player.GetComponent<PlayerCombat>();
        Animator ani = player.GetComponentInChildren<Animator>();
        Collider2D other = Physics2D.OverlapCircle(pc.weaponCheck.position, attackRange, pc.enemyLM);

        if (other != null){
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, (other.transform.position - player.transform.position), attackRange * 2f, pc.enemyLM);
            IAttackable combatScript = other.GetComponent<IAttackable>();
            combatScript.ApplyDamage(attackDamage, hit.point, (Vector3)hit.point - player.transform.transform.position);
        }

        ani.SetTrigger("attack");

        yield return null;

    }

}
