using UnityEngine;

public class AttackMoment : MonoBehaviour {

	private PlayerCombat combatScript;

	private void Start(){
		combatScript = transform.parent.GetComponent<PlayerCombat> ();
	}

	public void DoDamage(){
		Debug.Log ("damage now!");
		//combatScript.TryDamage ();
	} 
}
