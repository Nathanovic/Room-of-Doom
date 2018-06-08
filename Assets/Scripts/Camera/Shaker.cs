using UnityEngine;
using XInputDotNetPure;

//used to shake the camera
public class Shaker : MonoBehaviour {

	public static Shaker instance;

	private float camShakeTimer;
	private float camShakeAmount;

	private float contrShakeTimer;
	private float contrShakeAmount;

	private void Awake(){
		instance = this;
	}

	private void Start(){
		CameraMovement camScript = GetComponent<CameraMovement> ();
		camScript.shakeMovement += GetCameraShaker;
	}

	private void Update(){
		if (camShakeTimer > 0f) {
			camShakeTimer -= Time.deltaTime;
		}
	}

	private void FixedUpdate(){
		if (contrShakeTimer > 0f) {
			contrShakeTimer -= Time.fixedDeltaTime;

			if (contrShakeTimer > 0f) {
				UpdateControllerShake (contrShakeAmount);				
			}	
			else {
				UpdateControllerShake (0f);
			}
		}
	}

	private void UpdateControllerShake(float shakeAmount){
		for (int i = 0; i < 2; i++) {
			PlayerIndex playerIndex = (PlayerIndex)i;
			GamePad.SetVibration (playerIndex, shakeAmount, 0f);
		}		
	}

	private Vector2 GetCameraShaker () {
		if (camShakeTimer > 0f) {
			Vector2 shakePos = Random.insideUnitCircle * camShakeAmount;
			return shakePos;
		}
		else {
			return Vector2.zero;
		}
	}

	public void CameraShake(ShakeSettings shake, float damageValue = 1f){
		if (damageValue > 1f) {
			float extra = damageValue - 1f;
			extra *= 0.3f;
			damageValue = 1f + extra;
		}

		camShakeTimer = shake.duration * damageValue;
		camShakeAmount = shake.amount;
	}

	//used to shake both controllers
	public void ControllerShake(ShakeSettings shake){
		contrShakeTimer = shake.duration;
		contrShakeAmount = shake.amount;
	}

	private void OnApplicationQuit(){
		UpdateControllerShake (0f);
	}
}
