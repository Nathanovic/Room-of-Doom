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
    private PlayerBase playerbase;
    private Rigidbody2D rb;


    private void Start () {
        input = GetComponent<PlayerInput>();
        playerbase = GetComponent<PlayerBase>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update () {
        if (canDash && input.Lhorizontal != 0 && (input.ButtonIsDown(PlayerInput.Button.X)        || Input.GetKeyDown(KeyCode.D))){
            canDash = false;
            StartCoroutine(Dash());
        }
    }


    private IEnumerator Dash(){
        Debug.Log("Dash");

        curPower = power;
        dashTime += Time.deltaTime;
        playerbase.canControl = false;

        while (dashTime <= dashDuration){
            rb.velocity = input.Lhorizontal > 0 ? Vector2.right * curPower : Vector2.left * curPower;
            dashTime += Time.deltaTime;
            curPower = Mathf.Lerp(curPower, 0, dashTime);

            yield return new WaitForEndOfFrame();
        }
        playerbase.canControl = true;

        yield return new WaitForSeconds(cooldown);

        canDash = true;
        dashTime = 0;
    }

}
