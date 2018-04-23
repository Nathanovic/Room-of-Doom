using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    public MovePlayerSO movementKeys;
    public float speed;

    private Rigidbody rb;

    void Start () {
        rb = GetComponent<Rigidbody>();
        CameraMovement.players.Add(gameObject);
    }

    void Update () {
        float moveHorizontal = 0;
        float moveVertical = 0;

        if (Input.GetKey(movementKeys.leftKey)){
            moveHorizontal = -speed;
        }

        if (Input.GetKey(movementKeys.rightKey)){
            moveHorizontal = speed;
        }

        if (Input.GetKey(movementKeys.jumpKey)){
            moveVertical = speed;
        }

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        rb.AddForce(movement);

    }
}
