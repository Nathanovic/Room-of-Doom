using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour {

    public float cooldown;
    public GameObject projectile;
    public float projectileStartDis;

    private bool canRangedAtteck = true;
    private PlayerInput input;
    private Vector2 aim;

    public void Start () {
        input = GetComponent<PlayerInput>();

    }

    private void Update () {
        if (canRangedAtteck && (input.ButtonIsDown(PlayerInput.Button.B) || Input.GetKeyDown(KeyCode.A))){
            canRangedAtteck = false;
            StartCoroutine(RangedAtteck());

        }
        
    }

    private IEnumerator RangedAtteck(){
        Debug.Log("RangedAtteck");

        Vector2 spawnPos = new Vector2((transform.localScale.x > 0 ? projectileStartDis : -projectileStartDis) + gameObject.transform.position.x, gameObject.transform.position.y);
        GameObject p = Instantiate(projectile, spawnPos, Quaternion.identity);

        if (transform.localScale.x < 0){
            ProjectileMovement projectileBul = p.GetComponent<ProjectileMovement>();
            projectileBul.speed = -projectileBul.speed;
        }

        yield return new WaitForSeconds(cooldown);

        canRangedAtteck = true;

    }

}
