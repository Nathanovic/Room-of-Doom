using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerMeleeSpecialAttack")]
public class PlayerMeleeSpecialAttack : Ability{

    public GameObject projectile;
    public float force;
    public float aimDuration;
    public Vector3 spawnOffset;
    public Vector3 moveToOffset;

    private GameObject player;
    private Vector3 direction;
    private PlayerInput playerInput;
    private CharacterAbilityBehaviour charBehaviour;

    private float endTime;
    private float xDir;
    private float yDir;
    private bool alReadySHot = false;

    public override void Init(GameObject p){
        player = p;
        playerInput = player.GetComponent<PlayerInput>();
        charBehaviour = player.GetComponent<CharacterAbilityBehaviour>();
        xDir = playerInput.Lhorizontal;
        yDir = playerInput.Lvertical;
    }

    public override IEnumerator TriggerAbility() {
        Debug.Log("PlayerMeleeSpecialAttack");

        GameObject p = Instantiate(projectile, player.transform.position + moveToOffset, Quaternion.identity);
        endTime = Time.time + aimDuration;
        xDir = player.transform.localScale.x > 0 ? 1 : -1;

        yield return new WaitForEndOfFrame();

        while (endTime > Time.time) {
            Aim(p.transform);
            if (playerInput.ButtonIsDown(button) || Input.GetKey(KeyCode.P)){
                if (alReadySHot == false){
                    Shoot(p.transform);
                    break;
                }
            }

            yield return null;
        }

        if (alReadySHot == false){
            Shoot(p.transform);
        }

        charBehaviour.isCasting = false;
        alReadySHot = false;
        yield return null;
    }

    private void Aim(Transform proj){
        Debug.Log("Aim");

        if (playerInput.Lhorizontal != 0 && playerInput.Lvertical != 0){
            xDir = playerInput.Lhorizontal;
            yDir = playerInput.Lvertical;
        }
        
        //proj.transform.rotation = Quaternion.LookRotation(proj.transform.position - new Vector3(xDir, yDir)) ;
        float angle = Mathf.Atan2(xDir, yDir) * Mathf.Rad2Deg;
        proj.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        charBehaviour.isCasting = true;
    }

    private void Shoot(Transform proj){
        if (alReadySHot == false){
            alReadySHot = true;

            if (playerInput.Lhorizontal != 0 && playerInput.Lvertical != 0){
                xDir = playerInput.Lhorizontal;
                yDir = playerInput.Lvertical;
            }

            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            rb.velocity = new Vector2(xDir, -yDir).normalized * force;
        }

    }
}
