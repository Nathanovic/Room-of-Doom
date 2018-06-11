using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityBehaviour : MonoBehaviour {

    public bool isCasting;
    public bool isStunned;
    public KeyCode testBut;
    public Ability[] characterAbilities;
    [HideInInspector]
    public Ability[] cloneAbilities;

    private PlayerInput playerInput;
    private AudioSource audioSource;
    private PlayerMovement playerMovement;
    private PlayerCombat playercombat;

    private void Awake(){
        cloneAbilities = new Ability[characterAbilities.Length];
        for (int i = 0; i < characterAbilities.Length; i++) {
            cloneAbilities[i] = characterAbilities[i].Clone();
            cloneAbilities[i].Init(gameObject);
        }

    }

    private void Start(){
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playercombat = GetComponent<PlayerCombat>();
    }

    private void Update(){
        if (isCasting == false && isStunned == false && playercombat.health > 0){ 
            foreach (var ab in cloneAbilities){
                if (ab.readyAtTime <= Time.time && (playerInput.ButtonIsDown(ab.button) || Input.GetKeyDown(testBut))){
                    StartCoroutine(ab.TriggerAbility());
                    ab.Cooldown();
                    AbilitySound(ab);
                    if (ab.castingTime != 0){
                        StartCoroutine(Casting(ab));
                    }
                }           
            }

        }
        else{
            WhileCasting();
        }

    } 

    private IEnumerator Casting(Ability a){
        isCasting = true;

        yield return new WaitForSeconds(a.castingTime);

        isCasting = false;

    }

    private void WhileCasting(){
        playerMovement.StartCasting();

    }

    private void AbilitySound(Ability ab){
        audioSource.clip = ab.soundEffect;
        audioSource.Play();

    }

}
