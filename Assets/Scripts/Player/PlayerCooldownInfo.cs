using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCooldownInfo : MonoBehaviour {

    public Text cdTime;
    public Image cdEffect;

    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;

    private void Update(){
        bool coolDownComplete = (Time.time > nextReadyTime);

        if (coolDownComplete){
            AbilityReady();
            //if (Input.GetButtonDown(abilityButtonAxisName)){
                AbilityTriggered();
            //}
        }
        else{
            CoolDown();
        }
    }


    private void AbilityReady(){
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
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        cdTime.enabled = true;
        cdEffect.enabled = true;
    }
}
