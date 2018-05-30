using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerMeleeSpecialAttack")]
public class PlayerMeleeSpecialAttack : Ability{

    public GameObject projectile;
    public Vector3 offset;

    private GameObject player;

    public override void Init(GameObject p){
        player = p;
    }

    public override IEnumerator TriggerAbility(){
        Debug.Log("PlayerMeleeSpecialAttack");

        GameObject p = Instantiate(projectile, player.transform.position + offset, Quaternion.identity);

        yield return null;
    }
}
