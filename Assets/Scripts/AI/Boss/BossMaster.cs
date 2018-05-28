using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//this script handles the boss state 
public class BossMaster : MonoBehaviour {

	private List<MagmaWorm> worms;

	private void Start () {
		worms = GetComponentsInChildren<MagmaWorm> ().ToList();
	}

	private void Update () {
		
	}
}
