using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

	private static CombatManager _instance;
	public static CombatManager Instance{
		get{
			if (_instance == null) {
				GameObject go = new GameObject ("Combat Manager");
				_instance = go.AddComponent<CombatManager> ();
			}

			return _instance;
		}
	}

	public List<CharacterCombat> potentialTargets = new List<CharacterCombat>();

	[SerializeField]private Transform worldCanvas;
	[SerializeField]private HealthBar healthBar;

	public void RegisterPotentialTarget(CharacterCombat t){
		potentialTargets.Add (t);
	}

	private void Start(){
		worldCanvas = GameObject.FindGameObjectWithTag ("WorldCanvas").transform;
		HealthBar healthBarPrefab = Resources.Load<HealthBar> ("HealthBar");

		//spawn the healthbars and initialize them:
		foreach (CharacterCombat attackable in potentialTargets) {
			HealthBar healthBar = GameObject.Instantiate (healthBarPrefab, worldCanvas) as HealthBar;
			healthBar.Init (attackable);
		}
	}

	public void PotentialTargetDied(CharacterCombat t){
		potentialTargets.Remove (t);
	}
}
