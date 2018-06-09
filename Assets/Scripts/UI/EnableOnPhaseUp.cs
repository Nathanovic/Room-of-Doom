using UnityEngine;

public class EnableOnPhaseUp : MonoBehaviour {

	public WormBoss boss;

	private void Start(){
		boss.onPhaseUp += OnPhaseUp;
		gameObject.SetActive (false);
	}

	private void OnPhaseUp (int stateIndex) {
		if (!gameObject.activeInHierarchy) {
			gameObject.SetActive (true);
		}
	}
}
