using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public Text timer;
    
    private float timerFloat = 0;
    private int min = 0;
    private int sec = 0;

    void Start () {

    }
	
	void Update () {
        TimerUI();

    }

    private void TimerUI(){
        timerFloat += Time.deltaTime;

        min = (int)Mathf.FloorToInt(timerFloat / 60);
        sec = (int)Mathf.FloorToInt(timerFloat - (min * 60));

        timer.text = (min < 10? "0":"") + min + ":" + (sec < 10 ? "0" : "") + + sec;
    }
}
