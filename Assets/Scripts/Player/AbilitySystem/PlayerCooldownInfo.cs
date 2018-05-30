using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCooldownInfo : MonoBehaviour {

    public Text cdTime;
    public Image cdEffect;
    public Ability ab;

    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;

    private void Update(){
        bool coolDownComplete = (Time.time > ab.readyAtTime);

        if (coolDownComplete){
            AbilityReady();
        }
        else{
            if (coolDownTimeLeft == 0){
                AbilityTriggered();
            }

            CoolDown();
        }
    }


    private void AbilityReady(){
        coolDownTimeLeft = 0;
        cdTime.enabled = false;
        cdEffect.enabled = false;
    }

    private void CoolDown(){
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCd = Mathf.Round(coolDownTimeLeft);
        cdTime.text = roundedCd.ToString();
        cdEffect.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    private void AbilityTriggered(){
        Debug.Log("trigger");
        coolDownDuration = ab.cooldown;
        coolDownTimeLeft = ab.cooldown;
        cdTime.enabled = true;
        cdEffect.enabled = true;
    }
}
