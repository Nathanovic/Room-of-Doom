using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerMeleeSpecialAttack")]
public class PlayerMeleeSpecialAttack : Ability{

    public GameObject projectile;
    public Vector3 spawnOffset;
    public Vector3 moveToOffset;

    private GameObject player;

    public override void Init(GameObject p){
        player = p;
    }

    public override IEnumerator TriggerAbility(){
        Debug.Log("PlayerMeleeSpecialAttack");

        GameObject p = Instantiate(projectile, player.transform.position + spawnOffset, Quaternion.identity);

        //while (p.transform.position. != player.transform.position + moveToOffset){
            p.transform.position = Vector3.MoveTowards(p.transform.position, player.transform.position + moveToOffset, 0.5f * Time.deltaTime);
        //}


        yield return null;
    }
}
