using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {

    public float speed;
    public bool hasTarget = false;
    public List<Vector3> target = new List<Vector3>();
    public float margeToEndPos;
    public float delayNextTarget;

    private Rigidbody2D rb;
    public int currentTarget = 0;
    private Vector3 startPos;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update(){
        if (hasTarget == false){
            Move();
        }
        else if(hasTarget && currentTarget < target.Count ){
            MoveToTarget();
        }            

    }

    private void Move(){
        rb.AddForce(new Vector2(speed, 0));
        rb.velocity = new Vector3(rb.velocity.x, 0);
    }

    private void MoveToTarget(){
        startPos = transform.position;
        transform.position = Vector2.MoveTowards(startPos, target[currentTarget], speed * Time.deltaTime);     

        if (ReachedEndPos(target[currentTarget])){
            delayNextTarget -= Time.deltaTime;
            if (delayNextTarget < 0){
                currentTarget++;
            }
        }
    }


    private bool ReachedEndPos(Vector2 end){
        float x = transform.position.x - end.x;
        float y = transform.position.y - end.y;

        if (x < margeToEndPos && y < margeToEndPos && x > -margeToEndPos && y > -margeToEndPos){
            return true;
        }

        return false;
    }

}
