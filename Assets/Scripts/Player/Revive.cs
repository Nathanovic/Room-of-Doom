using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour {

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
        if (canRevive && playerCombat.health > 0 && otherPlayerCombat.health <= 0 && (playerInput.ButtonIsDown(buttonToRevive) || Input.GetKeyDown(KeyCode.L))){
            canRevive = false;
            Collider2D[] hitCollider = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius, layer);

            foreach (var item in hitCollider){
                if (item.transform.root.gameObject == otherPlayerCombat.transform.root.gameObject){
                    otherPlayerCombat.health = 10;
                    anim.SetBool("dead", false);
                    
                    Debug.Log("revived");
                    otherPlayerCombat.HealthChangedEvent();
                    DeadManager.instance.OnPlayerRevive();
                    break;
                }
            }
            StartCoroutine(ReviveDelay());
        }

    }

    private IEnumerator ReviveDelay(){
        yield return new WaitForSeconds(0.3f);
        canRevive = true;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (otherPlayerCombat != null && collision.transform.root.tag == "Player" && otherPlayerCombat.health <= 0) {
            if (playerInput.ButtonIsDown(buttonToRevive) || Input.GetKeyDown(KeyCode.L)){
                Debug.Log("revived");
            }
        }

    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
