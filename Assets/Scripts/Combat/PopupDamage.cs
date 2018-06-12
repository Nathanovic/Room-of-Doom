using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDamage : MonoBehaviour {

    private Animator ani;
    private Text damageText;


    private void Start(){
        ani = transform.GetChild(0).GetComponent<Animator>();
        AnimatorClipInfo[] clipInfos = ani.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfos[0].clip.length);
        damageText = ani.GetComponent<Text>();
    }

    public void SetText(string text){
        damageText.text = text;


    }

}
