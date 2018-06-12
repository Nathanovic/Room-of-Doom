using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour {

    public int damage;
    public float hitDelay = 0.2f;
    public float projectileAutoDestroy;
    public bool destroyOnHit = true;

    private float startTime;
	private bool didDamage;

	private void Start(){
		Invoke ("DestroySelf", projectileAutoDestroy);
        startTime = Time.time;
	}


	private void OnTriggerEnter2D(Collider2D other){
	    if (!didDamage && other.transform.root.tag == "Enemy"){
		    IAttackable attackable = other.GetComponent<IAttackable>();
			if (attackable != null && attackable.ValidTarget()){
			    didDamage = true;
				Vector3 hitDir = other.transform.position - transform.position;
				attackable.ApplyDamage(damage, transform.position, hitDir);
            }
            if (destroyOnHit){
		        DestroySelf ();

            }
        }

    }

	private void OnCollisionEnter2D(Collision2D coll){
        if (destroyOnHit){
            DestroySelf();

        }
        if (startTime - Time.time >= hitDelay){
            if (destroyOnHit){
                DestroySelf();

            }
        }        
	}

	private void DestroySelf(){
		Destroy (gameObject);
	}
}
