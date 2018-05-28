using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour {

    public int damage;
    public float hitDelay = 0.2f;
    public float projectileAutoDestroy;

    private float startTime;
	private bool didDamage;

	private void Start(){
		Invoke ("DestroySelf", projectileAutoDestroy);
        startTime = Time.time;
	}


	private void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("trigger: " + other.transform.root.name + "_tag: " + other.transform.root.tag);
	    if (!didDamage && other.transform.root.tag == "Enemy"){
			Debug.Log ("can attack");
		    IAttackable attackable = other.GetComponent<IAttackable>();
		    if (attackable != null){
			    didDamage = true;
			    attackable.ApplyDamage(damage, transform.position);
            }

		    DestroySelf ();
        }

    }

	private void OnCollisionEnter2D(Collision2D coll){
        if (startTime - Time.time >= hitDelay){
            DestroySelf();
        }        
	}

	private void DestroySelf(){
		Destroy (gameObject);
	}
}
