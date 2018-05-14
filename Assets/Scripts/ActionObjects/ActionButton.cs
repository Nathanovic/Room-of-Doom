using UnityEngine;

public class ActionButton : ActionObject {

	public ActionObject[] buttonFeedbackObjects;

	public override void Trigger(){
		base.Trigger ();
		foreach (ActionObject resultObject in buttonFeedbackObjects) {
			resultObject.Trigger ();
		}
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player" || other.tag == "Enemy") {
			Trigger ();
		}
	}
}
