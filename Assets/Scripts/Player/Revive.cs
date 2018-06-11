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

    private void Start(){
        playerInput = GetComponent<PlayerInput>();
        anim = otherPlayerCombat.gameObject.GetComponentInChildren<Animator>();

    }


    private void Update(){
        if (playerInput.ButtonIsDown(buttonToRevive) || Input.GetKeyDown(KeyCode.L)){
            Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), radius, layer);

            if (hitCollider != null){
                if (otherPlayerCombat.health <= 0){
                    otherPlayerCombat.health = 10;
                    otherPlayerCombat.HealthChangedEvent();
                    anim.SetBool("dead", false);

                    Debug.Log("revived");
                }
            }
        }

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
