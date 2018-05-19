using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedSpecial : MonoBehaviour {

    public GameObject projectile;
    public float cooldown;
    public int amount; 

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
            GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);

        }

        yield return new WaitForSeconds(cooldown);

        canRangedSpecial = true;

    }
}
