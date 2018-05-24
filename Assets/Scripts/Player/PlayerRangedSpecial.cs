using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedSpecial : MonoBehaviour {

    public GameObject projectile;
    public float cooldown;
    public int amount;
    public float yOffset;
    public float delayBetweenObjects;
    [Range(0, 1)]
    public float rangeAboveHead;

    private PlayerInput input;
    private bool canRangedSpecial = true;

    private Transform enemy;


    private void Start(){
        input = GetComponent<PlayerInput>();

    }

    private void Update(){
        if (canRangedSpecial && (input.ButtonIsDown(PlayerInput.Button.LB) || Input.GetKeyDown(KeyCode.S))){
            canRangedSpecial = false;
            StartCoroutine(SpecialAttack());
        }
    }

    private IEnumerator SpecialAttack(){
        Debug.Log("special");

        for (int i = 0; i < amount; i++){
            GameObject p = Instantiate(projectile, transform.position + new Vector3(0, yOffset, 0), Quaternion.identity);
            ProjectileMovement pMove = p.GetComponent<ProjectileMovement>();

            pMove.hasTarget = true;
            pMove.speed = 10;
            pMove.delayNextTarget = Random.Range(0.7f, 1);
            pMove.target.Add(new Vector2(transform.position.x, transform.position.y + yOffset +((amount - i) * rangeAboveHead)));
            pMove.target.Add(new Vector2(6, 5));
            yield return new WaitForSeconds(delayBetweenObjects);
        }

        yield return new WaitForSeconds(cooldown);

        canRangedSpecial = true;

    }
}
