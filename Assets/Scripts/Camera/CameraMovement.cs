using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	private Camera cam;

    public float minY = -1.5f;

	public float playerOffset = 2f;
	public float minCamViewSize = 7f;
	public float maxCamViewSize = 12f;

    public static List<GameObject> players = new List<GameObject>();

	public delegate Vector2 ShakeMoveDelegate ();
	public ShakeMoveDelegate shakeMovement;
    
	private void Awake(){
		players.Clear ();
	}

	private void Start(){
		cam = GetComponent<Camera> ();
	}

	private void Update () {
        if (players.Count > 0){
            SetCameraPos();  
			SetCameraViewSize ();
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

		Vector2 camPos = Vector2.zero;

		float minYCamPos = cam.orthographicSize + minY;
		Debug.DrawLine (transform.position, transform.up * minYCamPos, Color.red);
		camPos.y = Mathf.Min (middle.y, minYCamPos);
        camPos.x = middle.x;

		if (shakeMovement != null) {
			camPos += shakeMovement();
		}

		Vector3 newPos = new Vector3 (camPos.x, camPos.y, transform.position.z);
		transform.position = newPos;
    }

	private void SetCameraViewSize(){
		float playerXDist = Mathf.Abs(players [0].transform.position.x - players [1].transform.position.x);
		float preferredViewWidth = playerXDist + playerOffset * 2;//cam.aspect * 2 * cam.orthographicSize;
		float xOrthoSize = preferredViewWidth / 2f / cam.aspect;

		float playerYDist = Mathf.Abs(players [0].transform.position.y - players [1].transform.position.y);
		float preferredViewHeight = playerYDist + playerOffset * 2;//cam.aspect * 2 * cam.orthographicSize;
		float yOrthoSize = preferredViewHeight / 2f;

		float greatestOrthoSize = Mathf.Max (xOrthoSize, yOrthoSize);
		float preferredOrthoSize = Mathf.Clamp (greatestOrthoSize, minCamViewSize, maxCamViewSize);
		cam.orthographicSize = preferredOrthoSize;
	}
}
