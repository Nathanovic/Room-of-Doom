using UnityEngine;
using UnityEngine.UI;
using AI_UtilitySystem;
using System.Collections;

public class HealthBar : MonoBehaviour {

	private CharacterCombat combat;
	private RectTransform healthFill;
    private Image playerIcon;
	private Color hbColor = Color.white;
    public int blinkIconSpeed = 3;

    private int maxHealth;
	public bool worldScale = true;
	public bool useFill = false;
	public int fillChildIndex = 0;
    public int fillRedChildIndex = 0;
    public int playerIconIndex = 0;

    public float yOffset = 1f;
	private float hbWidth;

    private float currentHealthWidth;
    private float lastHealth;

	private void Awake(){
		healthFill = transform.GetChild (fillChildIndex).GetComponent<RectTransform> ();
        playerIcon = transform.GetChild(playerIconIndex).GetComponent<Image>();

        hbWidth = healthFill.sizeDelta.x;
	}

	//initialize the healthbar behaviour:
	public void Init(CharacterCombat combatScript, bool _worldScale = true){
		combat = combatScript;
		transform.name = "Health Bar";
		combat.onHealthChanged += HealthChanged;
		combat.onMaxHealthChanged += MaxHealthChanged;
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

        lastHealth = healthFill.sizeDelta.x;

		if (!useFill) {
			float percentage = (float)newHealth / maxHealth;
			Vector2 healthBarSize = healthFill.sizeDelta;
			healthBarSize.x = hbWidth * percentage;
			healthFill.sizeDelta = healthBarSize;
			currentHealthWidth = healthBarSize.x;

			StartCoroutine(LerpRedHealthbar());
		} 
		else {
			float fillAmount = (float)newHealth / maxHealth;
			playerIcon.fillAmount = fillAmount;
		}

        if (newHealth <= 0){
            StartCoroutine(IconBlink());
        }
	}

	private void MaxHealthChanged(int newMaxHealth, Color newHBColor = default(Color)){
		maxHealth = newMaxHealth;

		if (newHBColor != Color.clear) {
			hbColor = newHBColor;
			playerIcon.color = hbColor;
		}
	}

    private IEnumerator LerpRedHealthbar(){
		while (!Mathf.Approximately(healthFill.sizeDelta.x, currentHealthWidth)){
			Vector2 healthBarSize = healthFill.sizeDelta;
            healthBarSize.x = Mathf.Lerp(lastHealth, currentHealthWidth, 0.8f * Time.deltaTime);
			healthFill.sizeDelta = healthBarSize;

            lastHealth = healthBarSize.x;
            yield return null;
        }
    }


    private IEnumerator IconBlink(){
        while (healthFill.sizeDelta.x <= 0){
            var pingPong = Mathf.PingPong(Time.time * blinkIconSpeed, 1);
			var color = Color.Lerp(hbColor, Color.red, pingPong);
            playerIcon.color = color;
            yield return null;
        }

		playerIcon.color = hbColor;
    }

}
