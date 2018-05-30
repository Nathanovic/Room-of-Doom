using UnityEngine;

public class HitAnimationTrigger : MonoBehaviour {

	private PlayerCombat combatScript;

	private void Start () {
		combatScript = transform.parent.GetComponent<PlayerCombat> ();
	}

	public void DoDamage(){
		combatScript.TryDoDamage ();
	}
}
