using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour {

    public int damage;
	private bool didDamage;

	private void Start(){
		Invoke ("DestroySelf", 1f);
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (!didDamage && other.transform.root.tag == "Enemy"){
			IAttackable attackable = other.GetComponent<IAttackable>();
			if (attackable != null){
				didDamage = true;
				attackable.ApplyDamage(damage, transform.position);
            }

			DestroySelf ();
        }
    }

	private void OnCollisionEnter2D(Collision2D coll){
		DestroySelf ();
	}

	private void DestroySelf(){
		Destroy (gameObject);
	}
}
