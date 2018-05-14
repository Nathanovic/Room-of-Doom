using UnityEngine;
using System.Collections.Generic;

//this script is used for picking up items
public class PlayerPickup : MonoBehaviour {

	private PlayerInput input;
	public List<Collider2D> availablePickups;
	private Pickup currentPickup;
	private PlayerBase baseScript;

	private void Start(){
		input = transform.parent.GetComponent<PlayerInput> ();
		availablePickups = new List<Collider2D> ();
		baseScript = GetComponentInParent<PlayerBase> ();
	}

	//try to pickup if we push the pickup button
	private void Update(){
		if (!baseScript.canControl)
			return;

		int lastPickupIndex = availablePickups.Count - 1;
		if (input.ButtonIsDown(PlayerInput.Button.Y) && lastPickupIndex >= 0) {
            Debug.Log("Pickup");
			if (currentPickup != null) {
				currentPickup.DropMe (transform.position);
			}

			Pickup pickup = availablePickups [lastPickupIndex].GetComponent <Pickup> ();
			currentPickup = pickup;
			pickup.PickMeUp ();
		}
	}

	//check for pickup available:
	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Pickup" && !availablePickups.Contains(other)) { 
			availablePickups.Add (other);
			Vector3 targetPos = new Vector3 (other.transform.position.x, transform.position.y, 0f);
			NotificationPanel.instance.ShowNotification (targetPos, "(<color=red>Y</color>) apple");
		}
	}

	//check for pickup left
	private void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Pickup" && availablePickups.Contains(other)) { 
			availablePickups.Remove (other);
			if (availablePickups.Count == 0) {
				NotificationPanel.instance.DeactivateNotification ();
			}
		}
	}
}
