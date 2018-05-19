using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedSpecial : MonoBehaviour {

    public GameObject projectile;
    public float cooldown;


    private PlayerInput input;
    private bool casting;


    private void Start(){
        input = GetComponent<PlayerInput>();


    }

    private void Update(){
        if (casting && (input.ButtonIsDown(PlayerInput.Button.LB) || Input.GetKeyDown(KeyCode.S))){
            casting = false;
            StartCoroutine(SpecialAttack());

        }

    }

    private IEnumerator SpecialAttack(){


        yield return null;
    }



    }
