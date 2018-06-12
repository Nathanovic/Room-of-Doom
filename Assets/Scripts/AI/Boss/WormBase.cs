using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used by all worms
//used to manage what actions should be done by the worm
//all of the movement is done by WormMovement
//the entire update behaviour is called from BossManager
public class WormBase : MonoBehaviour {

	private WormMovement moveScript;
	protected WormSegment[] wormSegments;

	protected Transform head;

	private Vector3 deactivePosition;

	public float minXAttackDist = 4f;
	public int bodyDamage = 3;

	public HealthBar myHealthBar;

	private bool attacking;
	protected float undergroundTime;
	private float currentUndergroundTime;

	public float minRespawnTime;
	public float maxRespawnTime;
	[Range(0f, 1f)]public float intensity;//bepaald hoe snel de worm weer tevoorschijn komt 1f = immediate respawn
	public float rndmIntensity = 0.2f;
	private float intensityFactor;

	public Color explodeColor = new Color (0.6f, 0f, 0f, 0.7f);
	public float explodePower;
	public float explodeDelay = 2f;
	public int explodeDamage = 1;

	public delegate void WormDelegate(WormBase worm);
	public event WormDelegate onWormDied;

	protected virtual void Start(){
		BossManager.instance.InitializeWorm (this);
		moveScript = GetComponent<WormMovement> ();
		deactivePosition = transform.position;
		head = transform.GetChild (0);

		wormSegments = GetComponentsInChildren<WormSegment> ();
		moveScript.onAttackEnded += OnAttackEnded;

		CharacterCombat combatScript = GetComponent<CharacterCombat> ();
		combatScript.onDie += OnDie;
		myHealthBar.Init (combatScript, false);

		OnAttackEnded ();
	}

	public virtual void WormUpdate(){
		if (attacking) {
			moveScript.AttackUpdate ();
		} 
		else {
			Cooldown ();
		}
	}

	private void Cooldown(){
		currentUndergroundTime += Time.deltaTime;

		if (currentUndergroundTime >= undergroundTime) {
			StartAttack ();
		}		
	}

	protected virtual void StartAttack(){
		attacking = true;
		moveScript.PrepareAttack (intensityFactor);
	}

	//calculate when we will appear again
	protected virtual void OnAttackEnded(){
		attacking = false;
		float rndmIntensityFactor = Random.Range (-rndmIntensity, rndmIntensity) + intensity;
		intensityFactor = Mathf.Clamp01 (rndmIntensityFactor);
		undergroundTime = Mathf.Lerp (maxRespawnTime, minRespawnTime, intensityFactor);
		currentUndergroundTime = 0f;
	}

	#region damage handling
	private void OnTriggerEnter2D(Collider2D other){
		TryHitOther (other);
	}

	public void TryHitOther(Collider2D other){
		if (other.tag == "Player") {
			CharacterCombat target = other.GetComponent<CharacterCombat> ();
			if (target.ValidTarget ()) {
				target.ApplyDamage (bodyDamage, transform.position, target.transform.position - transform.position);
			}
		}
	}

	//destroy the gameobject after time, first explode
	protected void OnDie(){
		if (onWormDied != null) {
			onWormDied (this);
		}
		StartCoroutine (DestroySelf ());
		bodyDamage = explodeDamage;
	}
	#endregion

	private IEnumerator DestroySelf(){
		Debug.Log ("destroying self: " + transform.name);
		//set up for explosion
		Vector2 explodeCenter = wormSegments [wormSegments.Length / 2].Position();

		head.GetComponent<SpriteRenderer> ().color = explodeColor;
		List<Rigidbody2D> explodables = new List<Rigidbody2D> ();
		Rigidbody2D headRB = head.gameObject.AddComponent<Rigidbody2D> ();
		headRB.gravityScale = 0f;
		explodables.Add (headRB);
		foreach (WormSegment segment in wormSegments) {
			Rigidbody2D rb = segment.Detach (explodeColor);
			rb.gravityScale = 0f;
			explodables.Add (rb);
		}

		yield return new WaitForSeconds (explodeDelay);

		//explode
		foreach (Rigidbody2D rb in explodables) {
			float x = Random.Range (-1f, 1f);
			float y = Random.Range (-0.2f, 1f);
			Vector2 force = new Vector2(x, y).normalized * explodePower;
			rb.gravityScale = 1f;
			rb.AddForceAtPosition (force, explodeCenter);
		}

		//destroy after time
		yield return new WaitForSeconds (1.5f);
		foreach (Rigidbody2D rb in explodables) {
			Destroy (rb.gameObject);
		}
		Destroy (gameObject);
	}

	public Vector3 GetHeadPosition(){
		return head.position;
	}
}
