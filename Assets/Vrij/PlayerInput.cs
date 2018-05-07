using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles all of the input for one player
public class PlayerInput : MonoBehaviour {

    public enum Button {A, B, RB}//A = oa jumping, RB = attack

    private string horizontalAxis;
    //private string verticalAxis;
    private string aButton;
    private string bButton;
    private string rbButton;

    public int controllerNumber;

    [HideInInspector]
    public float horizontal;

    private void Awake(){
        controllerNumber = PlayerPrefs.GetInt("CharacterPlayer" + controllerNumber);
        SetControllerNumber(controllerNumber);
    }

    public bool ButtonIsDown(Button but){
        switch (but){
            case Button.A:
				return Input.GetButtonDown(aButton);
            case Button.B:
				return Input.GetButtonDown(bButton);
            case Button.RB:
				return Input.GetButtonDown(rbButton);
            default: 
				Debug.LogWarning ("unkown button: " + but);
				break;
        }
        return false;
    }

    public void SetControllerNumber(int nr){
        controllerNumber = nr;
        horizontalAxis = "J" + controllerNumber + "Horizontal";
        //verticalAxis = "J" + controllerNumber + "Vertical";
        aButton = "J" + controllerNumber + "A";
        bButton = "J" + controllerNumber + "B";
        rbButton = "J" + controllerNumber + "RB";

    }

    private void Update(){
        if (controllerNumber > 0){
            horizontal = Input.GetAxis(horizontalAxis);
            //horizontal = Input.GetAxis("Horizontal");
        }
    }
}
