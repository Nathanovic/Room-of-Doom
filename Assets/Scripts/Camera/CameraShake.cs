using UnityEngine;

//used to shake the camera
public class CameraShake : MonoBehaviour {

	public static CameraShake instance;

	private float shakeTimer;
	private float shakeAmount;

	private void Awake(){
		instance = this;
	}

	private void Start(){
		CameraMovement camScript = GetComponent<CameraMovement> ();
		camScript.shakeMovement += GetShaker;
	}

	private void Update(){
		if (shakeTimer >= 0f) {
			shakeTimer -= Time.deltaTime;
		}
		else {
			shakeAmount = 0f;
		}
	}

	private Vector2 GetShaker () {
		if (shakeTimer >= 0f) {
			Vector2 shakePos = Random.insideUnitCircle * shakeAmount;
			return shakePos;
		}
		else {
			return Vector2.zero;
		}
	}

	public void Shake(CameraShakeSettings shake, float damageValue = 1f){
		if (damageValue > 1f) {
			float extra = damageValue - 1f;
			extra *= 0.3f;
			damageValue = 1f + extra;
		}

		shakeTimer = shake.duration * damageValue;
		shakeAmount = shake.amount;
	}
}
