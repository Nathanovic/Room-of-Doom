using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//holds the control state for the player
public class PlayerBase : MonoBehaviour {

	public bool canControl;

	private void Start(){
        canControl = true;

		PlayerCombat combatScript = GetComponent<PlayerCombat> ();
		combatScript.onHealthChanged += OnHealthChanged;

        CameraMovement.players.Add(gameObject);
	}

	//make sure to know when we are dead
	private void OnHealthChanged(int newHealth){
		if (newHealth == 0) {
			canControl = false;
			//StartCoroutine (RestartGame ());
		}
	}

	//restart the game after time
	private IEnumerator RestartGame(){
		yield return new WaitForSeconds (2.5f);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}
}
