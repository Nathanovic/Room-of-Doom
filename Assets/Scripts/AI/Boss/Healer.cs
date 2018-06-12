using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//can be used to heal a worm
public class Healer : MonoBehaviour {

	public float healDuration = 3f;

	private CharacterCombat combatScript;
	public event SimpleDelegate onHeal;

	private void Awake(){
		combatScript = GetComponent<CharacterCombat> ();
	}

	public void Heal(int newMaxHP, Color newColor = default(Color)){	
		combatScript.MakeImmune (healDuration);
		combatScript.PrepareRevive (newMaxHP, newColor);
		StartCoroutine (HealOverTime ());
		if (onHeal != null) {
			onHeal ();
		}
	}

	private IEnumerator HealOverTime(){
		float t = 0f;
		while (t < 1f) {
			t += Time.deltaTime / healDuration;
			t = Mathf.Clamp01 (t);
			combatScript.HealUp (t);
			yield return null;
		}
	}
}
