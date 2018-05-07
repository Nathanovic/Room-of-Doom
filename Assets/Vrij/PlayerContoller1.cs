using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerContoller1 : MonoBehaviour {

    public int controllerNumber;

    public float speed;
    public Sprite[] characterList;

    private PlayerInput input;
    private Rigidbody2D rb;

    private void Awake(){
        GetComponent<SpriteRenderer>().sprite = characterList[PlayerPrefs.GetInt("CharacterPlayer" + controllerNumber)];
    }

    void Start () {
        CameraMovement.players.Add(gameObject);

        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        input.SetControllerNumber(controllerNumber);

        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    void Update () {
        float moveHorizontal = 0;
        float moveVertical = 0;

        moveHorizontal = input.horizontal * speed;

        //if (Input.GetKey(SO.rightKey)){
        //    moveHorizontal = speed;
        //}

        //if (Input.GetKeyDown(input.aButton) || Input.GetKeyDown(KeyCode.Space)){
        //   moveVertical = speed;
        //}

        if (input.ButtonIsDown(PlayerInput.Button.A) || Input.GetKeyDown(KeyCode.Space)){
            moveVertical = speed * speed;
        }

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.AddForce(movement);

    }
/*
    public void SetPlayer(Player player){
        this.player = player;
        input = player.GetComponent<PlayerInput>();


    }*/
}
