using UnityEngine;
using UnityEngine.UI;
using AI_UtilitySystem;

public class HealthBar : MonoBehaviour {

	private CharacterCombat combat;
	private RectTransform healthFill;

	private int maxHealth;
	public bool worldScale = true;
	public int fillChildIndex = 0;
	public float yOffset = 1f;
	private float hbWidth;

	private void Start(){
		healthFill = transform.GetChild (fillChildIndex).GetComponent<RectTransform> ();
		hbWidth = healthFill.sizeDelta.x;
	}

	//initialize the healthbar behaviour:
	public void Init(CharacterCombat combatScript, bool _worldScale = true){
		combat = combatScript;
		transform.name = "Health Bar";
		combat.onHealthChanged += HealthChanged;
		maxHealth = combat.health;
		worldScale = _worldScale;
	}

	//make sure the healthbar stays on top of the unit it belongs to:
	private void Update(){
		if (!worldScale)
			return;

		Vector2 pos = combat.Position ();
		Vector3 targetPos = new Vector3 (pos.x, pos.y + yOffset, 0f);
		transform.position = targetPos;
	}

	//make sure our health is properly filled:
	private void HealthChanged(int newHealth){ 
		if (newHealth == 0 && worldScale) {
			Destroy (gameObject);
			return;
		}

		float percentage = (float)newHealth / maxHealth;
		Vector2 healthBarSize = healthFill.sizeDelta;
		healthBarSize.x = hbWidth * percentage;
		healthFill.sizeDelta = healthBarSize;
	}
}
