using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour {

    public float radius;
    public LayerMask layer;
    public PlayerCombat otherPlayerCombat;
    public PlayerInput.Button buttonToRevive;

    private PlayerCombat playerCombat;
    private PlayerInput playerInput;
    private Animator anim;

    private void Start(){
        playerInput = GetComponent<PlayerInput>();
        anim = otherPlayerCombat.gameObject.GetComponentInChildren<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
    }


    private void Update(){
        if (playerCombat.health > 0 && (playerInput.ButtonIsDown(buttonToRevive) || Input.GetKeyDown(KeyCode.L))){
            Collider2D[] hitCollider = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius, layer);

            foreach (var item in hitCollider){
                Debug.Log(item.name);
                if (item.transform.root.gameObject == otherPlayerCombat.gameObject){
                    Debug.Log(item.name);

                    if (otherPlayerCombat.health <= 0){
                        otherPlayerCombat.health = 10;
                        anim.SetBool("dead", false);

                        Debug.Log("revived");
                        otherPlayerCombat.HealthChangedEvent();

                    }
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
