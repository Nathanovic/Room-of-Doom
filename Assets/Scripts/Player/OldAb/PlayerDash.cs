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
        if (canDash && (input.ButtonIsDown(PlayerInput.Button.X) || Input.GetKeyDown(KeyCode.D))){
            canDash = false;
            StartCoroutine(Dash());
        }
    }


    private IEnumerator Dash(){
        //Debug.Log("Dash");

        curPower = power;
        dashTime += Time.deltaTime;
		playerbase.canControl = false;

		Vector2 dashVelocity = transform.localScale.x > 0 ? Vector2.right * curPower : Vector2.left * curPower;
		Debug.Log (dashVelocity);

        while (dashTime <= dashDuration){
            dashTime += Time.deltaTime;
			curPower = Mathf.Lerp(curPower, 0, 1f / dashTime);
			rb.velocity = dashVelocity;

            yield return new WaitForEndOfFrame();
        }
        playerbase.canControl = true;
		rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(cooldown);

        canDash = true;
        dashTime = 0;
    }

}
