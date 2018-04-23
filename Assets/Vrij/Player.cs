using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerSO SO;
    public float speed;
    public GameObject characterList;

    private Rigidbody2D rb;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        CameraMovement.players.Add(gameObject);
        GetComponent<SpriteRenderer>().sprite = characterList.transform.GetChild(PlayerPrefs.GetInt(("CharacterPlayer" + SO.player).ToString())).GetComponent<SpriteRenderer>().sprite;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    void Update () {
        float moveHorizontal = 0;
        float moveVertical = 0;

        if (Input.GetKey(SO.leftKey)){
            moveHorizontal = -speed;
        }

        if (Input.GetKey(SO.rightKey)){
            moveHorizontal = speed;
        }

        if (Input.GetKey(SO.jumpKey)){
            moveVertical = speed;
        }

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.AddForce(movement);

    }
}
