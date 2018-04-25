using UnityEngine;
using System.Collections.Generic;

//this script is used for picking up items
public class PickUpBehaviour : MonoBehaviour {

	public List<Collider2D> availablePickups;
	private Pickup currentPickup;

	private void Start(){
		availablePickups = new List<Collider2D> ();
	}

	//check for pickup available:
	private void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("pickup enter: " + other.tag);
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

	private void Update(){
		//try to pickup if we push the pickup button:
		int lastPickupIndex = availablePickups.Count - 1;
		if (Input.GetButtonDown ("Pickup") && lastPickupIndex >= 0) {
			if (currentPickup != null) {
				currentPickup.DropMe (transform.position);
			}

			Pickup pickup = availablePickups [lastPickupIndex].GetComponent <Pickup> ();
			currentPickup = pickup;
			pickup.PickMeUp ();
		}
	}
}
