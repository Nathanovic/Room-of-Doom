using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerDashAbility")]
public class PlayerDashAbility : Ability{

    public float dashDuration;
    public float power;

    private float dashTime;
    private float curPower;
    private GameObject player;
    private SpriteRenderer

    public override void Init(GameObject p){
        player = p;
    }

    public override IEnumerator TriggerAbility(){
        Debug.Log("Dash");
        Ghost();

        curPower = power;
        dashTime += Time.deltaTime;

        PlayerBase pb = player.GetComponent<PlayerBase>();
        pb.canControl = false;

        Vector2 dashVelocity = player.transform.localScale.x > 0 ? Vector2.right * curPower : Vector2.left * curPower;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        CharacterAbilitieBehaviour cb = player.GetComponent<CharacterAbilitieBehaviour>();
        bool stun = cb.isStunned;

        while (dashTime <= dashDuration && stun == false){
            stun = cb.isStunned;
            dashTime += Time.deltaTime;
            curPower = Mathf.Lerp(curPower, 0, 1f / dashTime);
            rb.velocity = dashVelocity;
            yield return new WaitForEndOfFrame();
        }

        pb.canControl = true;
        rb.velocity = Vector3.zero;
        dashTime = 0;
    }

    public void Ghost(){


    }
}
