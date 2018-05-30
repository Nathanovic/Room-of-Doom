using UnityEngine;
using UnityEngine.Events;

//action objects zijn interactable objecten (en geen pickup* zijn)
//[[[* = miss zouden pickups ook action objects moeten zijn?]]]
public class ActionObject : MonoBehaviour {

	public UnityEvent[] actions;

	public virtual void Trigger(){
		for (int i = 0; i < actions.Length; i++) {
			actions [i].Invoke ();
		}
	}
}
