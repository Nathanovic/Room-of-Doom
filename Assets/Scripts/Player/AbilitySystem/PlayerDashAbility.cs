using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerDashAbility")]
public class PlayerDashAbility : Ability{

    public float dashDuration;
    public float power;

    private float dashTime;
    private float curPower;

    public override void TriggerAbility(GameObject player){
        Debug.Log("Dash");

        curPower = power;
        dashTime += Time.deltaTime;

        PlayerBase pb = player.GetComponent<PlayerBase>();
        pb.canControl = false;

        Vector2 dashVelocity = player.transform.localScale.x > 0 ? Vector2.right * curPower : Vector2.left * curPower;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        while (dashTime <= dashDuration){
            dashTime += Time.deltaTime;
            curPower = Mathf.Lerp(curPower, 0, 1f / dashTime);
            rb.velocity = dashVelocity;
            Debug.Log("while");
        }

        pb.canControl = true;
        rb.velocity = Vector3.zero;
        dashTime = 0;
    }

}
