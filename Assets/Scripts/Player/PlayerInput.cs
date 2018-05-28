using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles all of the input for one player
public class PlayerInput : MonoBehaviour {

    public enum Button {A, B, Y, X, RB, LB}//A = jumping&continue, B = back, X = oppakken, RB = attack

    private string LhorizontalAxis;
    private string LverticalAxis;
    private string RhorizontalAxis;
    private string RverticalAxis;
    private string aButton;
    private string bButton;
    private string yButton;
    private string xButton;

    private string rbButton;
    private string lbButton;


    public int controllerNumber;

    [HideInInspector]
    public float Lhorizontal;
    [HideInInspector]
    public float Lvertical;
    [HideInInspector]
    public float Rhorizontal;
    [HideInInspector]
    public float Rvertical;

	public bool keyboardInput;

    private void Awake(){
        controllerNumber = PlayerPrefs.GetInt("CharacterPlayer" + controllerNumber);
		if (keyboardInput)
			controllerNumber = 1;
		SetControllerNumber(controllerNumber);
    }

    public bool ButtonIsDown(Button but){
        switch (but){
		    case Button.A:
			    //Debug.Log ("button A: " + aButton + ": " + Input.GetButtonDown(aButton).ToString() + "|||Any key: " + Input.anyKeyDown + "|||");
			    return Input.GetButtonDown(aButton);
            case Button.B:
			    return Input.GetButtonDown(bButton);
            case Button.Y:
                return Input.GetButtonDown(yButton);
            case Button.X:
			    return Input.GetButtonDown(xButton);
            case Button.RB:
			    return Input.GetButtonDown(rbButton);
            case Button.LB:
                return Input.GetButtonDown(lbButton);
            default: 
				Debug.LogWarning ("unkown button: " + but);
				break;
        }
        return false;
    }

    public void SetControllerNumber(int nr){
        controllerNumber = nr;
        LhorizontalAxis = "J" + controllerNumber + "LHorizontal";
        LverticalAxis = "J" + controllerNumber + "LVertical";
        RhorizontalAxis = "J" + controllerNumber + "RHorizontal";
        RverticalAxis = "J" + controllerNumber + "RVertical";
        aButton = "J" + controllerNumber + "A";
        bButton = "J" + controllerNumber + "B";
        yButton = "J" + controllerNumber + "Y";
        xButton = "J" + controllerNumber + "X";

        rbButton = "J" + controllerNumber + "RB";
        lbButton = "J" + controllerNumber + "LB";

    }

    private void Update(){
        if (controllerNumber > 0){
			Lhorizontal = Input.GetAxis(LhorizontalAxis);
            Lvertical = Input.GetAxis(LverticalAxis);
            Rhorizontal = Input.GetAxis(RhorizontalAxis);
            Rvertical = Input.GetAxis(RverticalAxis);
        }
    }
}
