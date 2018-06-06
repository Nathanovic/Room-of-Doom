using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//can be used to heal a worm
public class Healer : MonoBehaviour {

	public int healCount = 1;
	public float healDuration = 3f;

	private CharacterCombat combatScript;
	public event SimpleDelegate onHeal;

	private void Start(){
		combatScript = GetComponent<CharacterCombat> ();
	}

	public void Heal(){	
		healCount--;
		combatScript.MakeImmune (healDuration);
		StartCoroutine (HealOverTime ());
		if (onHeal != null) {
			onHeal ();
		}
	}

	private IEnumerator HealOverTime(){
		float t = 0f;
		while (t < 1f) {
			t += Time.deltaTime / healDuration;
			combatScript.HealUp (t);
			yield return null;
		}
			
		if (healCount == 0) {
			Destroy (this);
		}
	}
}
