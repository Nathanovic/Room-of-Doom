using UnityEngine;
using UnityEngine.UI;
using AI_UtilitySystem;

public class HealthBar : MonoBehaviour {

	private CharacterCombat combat;
	private RectTransform healthFill;

	private int maxHealth;
	public float yOffset = 1f;

	private void Start(){
		healthFill = transform.GetChild (0).GetComponent<RectTransform> ();
	}

	//initialize the healthbar behaviour:
	public void Init(CharacterCombat combatScript){
		combat = combatScript;
		transform.name = "Health Bar";
		combat.onHealthChanged += HealthChanged;
		maxHealth = combat.health;
	}

	//make sure the healthbar stays on top of the unit it belongs to:
	private void Update(){
		Vector2 pos = combat.Position ();
		Vector3 targetPos = new Vector3 (pos.x, pos.y + yOffset, 0f);
		transform.position = targetPos;
	}

	//make sure our health is properly filled:
	private void HealthChanged(int newHealth){ 
		if (newHealth == 0) {
			Destroy (gameObject);
			return;
		}

		float percentage = (float)newHealth / maxHealth;
		Vector2 healthBarSize = healthFill.sizeDelta;
		healthBarSize.x = 100f * percentage;
		healthFill.sizeDelta = healthBarSize;
	}
}
