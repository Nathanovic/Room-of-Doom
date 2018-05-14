using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour {

    public float circleRange;
    public float cooldown;

    private bool canRangedAtteck = true;
    private PlayerInput input;
    private PlayerBase playerbase;
    private Vector2 aim;

    public void Start () {
        input = GetComponent<PlayerInput>();
        playerbase = GetComponent<PlayerBase>();

    }

    private void Update () {
        if (canRangedAtteck && input.horizontal != 0 && (input.ButtonIsDown(PlayerInput.Button.X) || Input.GetKeyDown(KeyCode.D))){
            canRangedAtteck = false;
            StartCoroutine(RangedAtteck());

            Debug.Log(aim);
            aim = new Vector2(input.horizontal, 0);
        }


        
    }

    private IEnumerator RangedAtteck(){
        Debug.Log("RangedAtteck");



        yield return new WaitForSeconds(cooldown);

        canRangedAtteck = true;

    }

}
