using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilitieBehaviour : MonoBehaviour {

    public bool isCasting;
    public bool isStunned;
    public KeyCode testBut;
    public Ability[] characterAbilities;

    private Ability[] cloneAbilities;
    private PlayerInput playerInput;
    private AudioSource audioSource;

    private void Start(){
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();

        cloneAbilities = new Ability[characterAbilities.Length];
        for (int i = 0; i < characterAbilities.Length; i++) {
            cloneAbilities[i] = characterAbilities[i].Clone();
        }
    }

    private void Update(){
        if (isCasting == false && isStunned == false){ 
            foreach (var ab in cloneAbilities){
                if (ab.readyAtTime <= Time.time && (playerInput.ButtonIsDown(ab.button) || Input.GetKeyDown(testBut))){
                    StartCoroutine(ab.TriggerAbility(gameObject));
                    ab.Cooldown();
                    AbilitySound(ab);
                    if (ab.castingTime != 0){
                        StartCoroutine(Casting(ab.castingTime));
                    }
                }           
            }
        }

    } 

    private IEnumerator Casting(float t){
        isCasting = true;

        yield return new WaitForSeconds(t);

        isCasting = false;
    }

    private void AbilitySound(Ability ab){
        audioSource.clip = ab.soundEffect;
        audioSource.Play();
    }

}
