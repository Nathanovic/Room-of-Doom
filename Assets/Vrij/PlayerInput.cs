using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public enum Button {A, B}

    private string horizontalAxis;
    private string verticalAxis;
    private string aButton;
    private string bButton;
    private int controllerNumber;

    [HideInInspector]
    public float horizontal;

    private void Awake(){


    }

    internal bool ButtonIsDown(Button but){
        switch (but){
            case Button.A:
                return Input.GetButton(aButton);
            case Button.B:
                return Input.GetButton(bButton);
        }
        return false;
    }

    internal void SetControllerNumber(int nr){
        controllerNumber = nr;
        horizontalAxis = "J" + controllerNumber + "Horizontal";
        verticalAxis = "J" + controllerNumber + "Vertical";
        aButton = "J" + controllerNumber + "A";
        bButton = "J" + controllerNumber + "B";
    }

    private void FixedUpdate(){
        if (controllerNumber > 0){
            horizontal = Input.GetAxis(horizontalAxis);
            horizontal = Input.GetAxis("Horizontal");
        }
    }

}
