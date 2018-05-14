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
        if (canRangedAtteck && (input.ButtonIsDown(PlayerInput.Button.X) || Input.GetKeyDown(KeyCode.D))){
            canRangedAtteck = false;
            StartCoroutine(RangedAtteck());

            aim = new Vector2(input.horizontal, input.vertical);
            Debug.Log(aim);

        }


        
    }

    private IEnumerator RangedAtteck(){
        Debug.Log("RangedAtteck");



        yield return new WaitForSeconds(cooldown);

        canRangedAtteck = true;

    }

}
