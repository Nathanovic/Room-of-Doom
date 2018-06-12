using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour {

    public int newHealth;
    public float radius;
    public LayerMask layer;
    public PlayerCombat otherPlayerCombat;
    public PlayerInput.Button buttonToRevive;
    private PlayerInput playerInput;
    private Animator anim;
    private PlayerCombat playerCombat;
    private bool canRevive = true;

    private void Start(){
        playerInput = GetComponent<PlayerInput>();
        anim = otherPlayerCombat.gameObject.GetComponentInChildren<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update(){
        if (playerCombat.health > 0 && otherPlayerCombat.health <= 0){
            Collider2D[] hitCollider = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius, layer);

            if (playerInput.ButtonIsDown(buttonToRevive) || Input.GetKeyDown(KeyCode.L)){
                foreach (var item in hitCollider){
					if (item.transform.root.gameObject == otherPlayerCombat.transform.root.gameObject){
                        otherPlayerCombat.health = newHealth;
                        anim.SetBool("dead", false);
                    
                        otherPlayerCombat.HealthChangedEvent();
                        DeadManager.instance.OnPlayerRevive();
                        break;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
