using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour {

    public PlayerCombat otherPlayer;
    public PlayerInput.Button buttonToRevive;
    private PlayerInput playerInput;

    private void Start(){
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (otherPlayer != null && collision.transform.root.tag == "Player" && otherPlayer.health <= 0) {
            if (playerInput.ButtonIsDown(buttonToRevive) || Input.GetKeyDown(KeyCode.L)){
                Debug.Log("revived");
            }
        }

    }


}
