using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerMeleeSpecialAttack")]
public class PlayerMeleeSpecialAttack : Ability{

    public GameObject projectile;
    public float aimDuration;
    public Vector3 spawnOffset;
    public Vector3 moveToOffset;

    private GameObject player;
    private PlayerInput playerInput;
    private Vector3 direction;

    private float endTime;

    public override void Init(GameObject p){
        player = p;
        playerInput = player.GetComponent<PlayerInput>();
    }

    public override IEnumerator TriggerAbility(){
        Debug.Log("PlayerMeleeSpecialAttack");

        var wait = new WaitForEndOfFrame();
        GameObject p = Instantiate(projectile, player.transform.position + moveToOffset, Quaternion.identity);
        endTime = Time.time + aimDuration;

        while (endTime > Time.time){
            if (Input.GetKey(KeyCode.O)){
                break;
            }

            Aim();

            yield return wait;
        }

        yield return null;
    }

    private void Aim(){
        Debug.Log("Aim");

        //while (endTime > Time.time || Input.GetKey(KeyCode.O) == false){

        //}


    }
}
