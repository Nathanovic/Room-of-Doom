using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadManager : MonoBehaviour {

    public static DeadManager instance;

    public int deadCount;
    public int winCount;
    public int playersRevived;

    public int playersAlive;

    private void Awake(){
        instance = this;

    }

    private void Start(){
        StartCoroutine(GetPlayersAliveDelay());

    }

    private void Update(){
        


    }

    public void OnPlayerRevive(){
        playersRevived++;
        playersAlive++;
    }

    public void OnPlayerDead(){
        deadCount++;
        playersAlive--;
    }

    private IEnumerator GetPlayersAliveDelay(){
        yield return new WaitForSeconds(0.3f);
        playersAlive = CameraMovement.players.Count;

    }



}
