using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;
    public float speed;

    private Rigidbody2D rb;
    private CharacterCombat charCombat;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update(){
        rb.AddForce(new Vector2(speed, 0));
        rb.velocity = new Vector3(rb.velocity.x, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.GetComponent<CharacterCombat>()){
            charCombat = collision.gameObject.GetComponent<CharacterCombat>();
            charCombat.ApplyDamage(damage, transform.position.x);
        }

        Destroy(gameObject);
    }

}
