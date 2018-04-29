using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PressToStartMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Fire1") > 0){
            Play();
        }
	}

    public void Play(){
        SceneManager.LoadScene(1);
    }

    public void Instructions(){
        Debug.Log("Instructions pressed");
    }

    public void Quit(){
        Application.Quit();
    }
}
