using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour {

    public float cooldown;
    public float dashDuration;
    public float power;

    private float dashTime;
    private float curPower;
    private bool canDash = true;
    private PlayerInput input;
    private PlayerMovement pm;
    private Rigidbody2D rb;


    private void Start () {
        input = GetComponent<PlayerInput>();
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update () {
        if (canDash && input.horizontal != 0 && (input.ButtonIsDown(PlayerInput.Button.X)        || Input.GetKeyDown(KeyCode.D))){
            canDash = false;
            StartCoroutine(Dash());
        }
    }


    private IEnumerator Dash(){
        Debug.Log("Dash");

        curPower = power;
        dashTime += Time.deltaTime;
        pm.canMove = false;

        while (dashTime <= dashDuration){
            rb.velocity = input.horizontal > 0 ? Vector2.right * curPower : Vector2.left * curPower;
            dashTime += Time.deltaTime;
            curPower = Mathf.Lerp(curPower, 0, dashTime);

            yield return new WaitForEndOfFrame();
        }
        pm.canMove = true;

        yield return new WaitForSeconds(cooldown);

        canDash = true;
        dashTime = 0;
    }

}
