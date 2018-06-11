using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour {

    public float radius;
    public LayerMask layer;
    public PlayerCombat otherPlayer;
    public PlayerInput.Button buttonToRevive;
    private PlayerInput playerInput;

    private void Start(){
        playerInput = GetComponent<PlayerInput>();
    }


    private void Update(){
        if (playerInput.ButtonIsDown(buttonToRevive) || Input.GetKeyDown(KeyCode.L)){
            Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), radius, layer);

            if (hitCollider != null){
                if (otherPlayer.health <= 0){

                    Debug.Log("revived");
                }
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (otherPlayer != null && collision.transform.root.tag == "Player" && otherPlayer.health <= 0) {
            if (playerInput.ButtonIsDown(buttonToRevive) || Input.GetKeyDown(KeyCode.L)){
                Debug.Log("revived");
            }
        }

    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
