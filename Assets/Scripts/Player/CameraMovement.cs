using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float minY;
    public float maxY;

    public static List<GameObject> players = new List<GameObject>();
    private Camera cam;
    
	private void Awake(){
		players.Clear ();
	}

    private void Start () {
        cam = Camera.main;
	}	

	private void Update () {
        if (players.Count > 0){
            SetCameraPos();  
        }
        
    }

    private void SetCameraPos(){
        Vector3 middle = Vector3.zero;
        int numPlayers = 0;

        for (int i = 0; i < players.Count; ++i){
            middle += players[i].transform.position;
            numPlayers++;
        }

        middle /= numPlayers;

        Vector3 camPos = cam.transform.position;

        if (middle.y > minY && middle.y < maxY){
            camPos.y = middle.y;
        }
        camPos.x = middle.x;
        cam.transform.position = camPos;
    }

}
