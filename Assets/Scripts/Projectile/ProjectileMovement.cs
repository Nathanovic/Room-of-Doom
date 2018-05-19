using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {

    public float speed;
    public bool hasTarget = false;
    public List<Vector3> target = new List<Vector3>();

    private Rigidbody2D rb;
    private int currentTarget = 0;
    private Vector3 startPos;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update(){
        if (hasTarget){
            MoveToTarget();
        }
        else{
            Move();
        }
    }

    private void MoveToTarget(){
        if (currentTarget > target.Count - 1){
            return;
        }

        //while (MoveTo(startPos, target[currentTarget])){
        //    startPos = transform.position;
            //StartCoroutine(MoveTo(startPos, target[currentTarget]));
        //}
    }

    private void Move(){
        rb.AddForce(new Vector2(speed, 0));
        rb.velocity = new Vector3(rb.velocity.x, 0);
    }

    private bool MoveTo(Vector3 startPos, Vector3 endPos){


        //if (transform.position - endPos >= new Vector3(0.1f, 0.1f)){
            transform.position = Vector2.Lerp(startPos, endPos, speed * Time.deltaTime);

       // }

        return true;
    }

}
