using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadManager : MonoBehaviour {


    public static int deadCount;
    public static int winCount;

    private int playersAlive;

    private void Start(){
        playersAlive = CameraMovement.players.Count;

    }

    private void Update(){
        


    }



}
