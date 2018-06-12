using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupDamage : MonoBehaviour {

    private Animator ani;


    private void Start(){
        ani = transform.GetChild(0).GetComponent<Animator>();

    }



}
