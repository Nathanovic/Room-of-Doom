using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour {

    public int damage;
    public bool destroyPosible = true;
    public LayerMask canHit;

    private CharacterCombat charCombat;


    private void OnCollisionEnter2D(Collision2D collision){
        if (destroyPosible &&                   (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Enemy")           ){
            if (collision.gameObject.GetComponent<CharacterCombat>()){
                charCombat = collision.gameObject.GetComponent<CharacterCombat>();
                charCombat.ApplyDamage(damage, transform.position.x);
            }

            Destroy(gameObject);
        }
    }
}
