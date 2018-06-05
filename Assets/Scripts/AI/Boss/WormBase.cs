using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBase : MonoBehaviour {

	private WormMovement moveScript;
	private WormSegment[] wormSegments;

	protected bool isDead;

	public Transform[] targets;//the players
	private SpawnPositions spawner;
	private Transform head;
	public ParticleSystem feedForwardVFX;

	private Vector3 deactivePosition;

	public int attackDamage;
	public float minXAttackDist = 4f;

	public HealthBar myHealthBar;

	public int bodyDamage = 3;
	private bool attacking;
	private float undergroundTime;
	private float currentUndergroundTime;

	public float minRespawnTime;
	public float maxRespawnTime;
	[Range(0f, 1f)]public float intensity;//bepaald hoe snel de worm weer tevoorschijn komt 1f = immediate respawn
	public float rndmIntensity = 0.2f;

	public Color explodeColor = new Color (0.6f, 0f, 0f, 0.7f);
	public float explodePower;
	public float explodeDelay = 2f;
	public int explodeDamage = 1;

	public CameraShakeSettings dieShake;

	protected virtual void Start(){
		BossManager.instance.InitializeBoss (this);
		moveScript = GetComponent<WormMovement> ();
		deactivePosition = transform.position;
		feedForwardVFX.transform.SetParent (null);
		head = transform.GetChild (0);
		spawner = GetComponentInChildren<SpawnPositions> ();

		wormSegments = GetComponentsInChildren<WormSegment> ();
		WormSegment lastSegment = wormSegments[wormSegments.Length - 1];
		lastSegment.onFinishedMoving += OnAttackEnded;
		HandleLastSegment (lastSegment);

		CharacterCombat combatScript = GetComponent<CharacterCombat> ();
		combatScript.onDie += OnDie;
		myHealthBar.Init (combatScript, false);
	}

	protected virtual void HandleLastSegment(WormSegment lastSegment){}

	protected virtual void Update(){
		if (isDead)
			return;

		if (attacking) {
			moveScript.CurveAttackUpdate ();
		} 
		else {
			Cooldown ();
		}
	}

	private void Cooldown(){
		currentUndergroundTime += Time.deltaTime;

		if (currentUndergroundTime >= undergroundTime) {
			StartCurveAttack ();
		}		
	}

	private void StartCurveAttack(){
		int rndmTargetIndex = Random.Range (0, targets.Length);
		Vector3 enemyPos = targets [rndmTargetIndex].position;

		//choose a random start pos that is far away from it:
		List<Vector3> availableSpawnPositions = new List<Vector3>();
		Vector3 startPos = Vector3.zero;
		int i = 0;
		foreach (Vector3 point in spawner.points) {
			float dist = Mathf.Abs (point.x - enemyPos.x);
			if (dist > minXAttackDist) {
				availableSpawnPositions.Add (point);
			}
			i++;
		}

		int rndmIndex = Random.Range (0, availableSpawnPositions.Count);
		startPos = availableSpawnPositions [rndmIndex];
		feedForwardVFX.transform.position = new Vector3 (startPos.x, 0f, 0f);
		feedForwardVFX.Play ();

		moveScript.StartCurveAttack (startPos, enemyPos);
	}

	//calculate when we will appear again
	private void OnAttackEnded(){
		attacking = false;
		float intensityFactor = Random.Range (-rndmIntensity, rndmIntensity) + 1f - intensity;
		intensityFactor = Mathf.Clamp01 (intensityFactor);
		undergroundTime = maxRespawnTime * intensityFactor;
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
	private void OnDie(){
		CameraShake.instance.Shake (dieShake);
		isDead = true;
		StartCoroutine (DestroySelf ());
		attackDamage = explodeDamage;
	}
	#endregion

	private IEnumerator DestroySelf(){
		yield return new WaitForSeconds (explodeDelay);

		//set up for explosion
		Vector2 explodeCenter = wormSegments [wormSegments.Length / 2].Position();

		head.GetComponent<SpriteRenderer> ().color = explodeColor;
		List<Rigidbody2D> explodables = new List<Rigidbody2D> ();
		explodables.Add (gameObject.AddComponent<Rigidbody2D> ());
		foreach (WormSegment segment in wormSegments) {
			explodables.Add (segment.Detach(explodeColor));
		}

		//explode
		foreach (Rigidbody2D rb in explodables) {
			float x = Random.Range (-1f, 1f);
			float y = Random.Range (-0.2f, 1f);
			Vector2 force = new Vector2(x, y).normalized * explodePower;
			rb.AddForceAtPosition (force, explodeCenter);
		}

		//destroy after time
		yield return new WaitForSeconds (1.5f);
		foreach (Rigidbody2D rb in explodables) {
			Destroy (rb.gameObject);
		}
	}

	public Vector3 GetHeadPosition(){
		return head.position;
	}
}
