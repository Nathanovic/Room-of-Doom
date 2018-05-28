using UnityEngine;
using System.Collections.Generic;

//handle the movement of the magma worm
//the body exists out of segments, and each segments just follows the head
//handles the magma worm attacks:
//attacks the targets it has by changing the moveCurve.points
public class MagmaWorm : MonoBehaviour {

	public Transform[] targets;//the players
	private SpawnPositions spawner;
	public ParticleSystem feedForwardVFX;

	private WormSegment[] wormSegments;
	private WormSegment lastSegment;
	public LayerMask groundLM;

	public float startDelay = 1f;
	private float delay = 0f;
	private Transform head;
	public float headLength;
	private BezierCurve movementCurve;
	private float moveT = 0f;
	private float curveMoveSpeed;
	public float movementSpeed = 2f;

	private Vector3 deactivePosition;
	private bool curveAttack;

	public int attackDamage;
	public float minXAttackDist = 4f;

	public HealthBar myHealthBar;

	private float undergroundTime;
	private float currentUndergroundTime;
	public float maxRespawnTime;
	[Range(0f, 1f)]public float intensity;//bepaald hoe snel de worm weer tevoorschijn komt 1f = immediate respawn
	public float rndmIntensity = 0.2f;

	private void Start(){
		head = transform.GetChild (0);
		feedForwardVFX.transform.SetParent (null);
		deactivePosition = transform.position;

		movementCurve = GetComponentInChildren<BezierCurve> ();
		spawner = GetComponentInChildren<SpawnPositions> ();
		movementCurve.DetachSelf ();
		wormSegments = GetComponentsInChildren<WormSegment> ();
		lastSegment = wormSegments[wormSegments.Length - 1];
		lastSegment.onFinishedMoving += OnAttackEnded;

		CharacterCombat combatScript = GetComponent<CharacterCombat> ();
		combatScript.onDie += DestroySelf;
		myHealthBar.Init (combatScript, false);

		BossManager.instance.InitializeBoss (this);
	}

	private void Update(){
		if (curveAttack) {
			if (delay > 0f) {
				delay -= Time.deltaTime;
			} 
			else {
				moveT += curveMoveSpeed * Time.deltaTime;
				transform.position = movementCurve.GetPoint (moveT);

				Vector3 dir = movementCurve.GetPoint (moveT + 0.1f) - transform.position;
				float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg + 180;
				head.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

				foreach (WormSegment segment in wormSegments) {
					segment.TraverseCurve (movementCurve);
				}
			}
		}
		else {
			currentUndergroundTime += Time.deltaTime;
			if (currentUndergroundTime >= undergroundTime) {
				StartCurveAttack ();
			}
		}

		if (Input.GetKeyUp (KeyCode.Space) && targets.Length > 0) {
			StartCurveAttack ();
		}
	}

	private void OnTriggerEnter2D(Collider2D other){
		TryHitOther (other);
	}

	public void TryHitOther(Collider2D other){
		if (other.tag == "Player") {
			CharacterCombat target = other.GetComponent<CharacterCombat> ();
			if (target.ValidTarget ()) {
				target.ApplyDamage (attackDamage, transform.position, target.transform.position - transform.position);
			}
		}
	}

	private void DestroySelf(){
		Destroy (gameObject);
	}

	private void StartCurveAttack(){
		transform.position = deactivePosition;

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

		//calculate a control point
		Vector3 controlPoint = Vector3.Lerp (startPos, enemyPos, 0.7f);
		float xDist = Mathf.Abs (startPos.x - enemyPos.x);
		controlPoint.y += xDist * 1.5f;

		//make sure our targetPos is beneath the ground surface:
		Vector3 targetPos = enemyPos;
		Vector3 enemyDir = (enemyPos - controlPoint);
		RaycastHit2D[] hits = Physics2D.RaycastAll(enemyPos, enemyDir, 20f, groundLM);
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider != null && hit.point.y < 0.3f) {
				targetPos = hit.point + (Vector2)enemyDir.normalized * 2.5f;
				Debug.DrawLine (controlPoint, targetPos, Color.yellow, 2f);
				break;
			}
		}

		if (targetPos.y > 0f)
			targetPos.y = -1.5f;

		movementCurve.points [0] = startPos;
		movementCurve.points [1] = controlPoint;
		movementCurve.points [2] = targetPos;

		//debug visually:
		Debug.DrawLine (startPos, controlPoint, Color.red, 1f);
		Debug.DrawLine (controlPoint, targetPos, Color.red, 1f);

		float moveCurveLength = movementCurve.GetLength ();
		curveMoveSpeed = movementSpeed / moveCurveLength;

		Transform prevSegment = head;
		float beforeSegmentDelay = headLength / moveCurveLength;
		foreach (WormSegment segment in wormSegments) {
			segment.Prepare (prevSegment, beforeSegmentDelay, curveMoveSpeed);
			beforeSegmentDelay += segment.length / moveCurveLength;
			prevSegment = segment.transform;
		}

		moveT = 0f;
		delay = startDelay;
		curveAttack = true;
	}

	//calculate when we will appear again
	private void OnAttackEnded(){
		curveAttack = false;
		float intensityFactor = Random.Range (-rndmIntensity, rndmIntensity) + 1f - intensity;
		intensityFactor = Mathf.Clamp01 (intensityFactor);
		undergroundTime = maxRespawnTime * intensityFactor;
		currentUndergroundTime = 0f;
	}

	public Vector3 GetHeadPosition(){
		return head.position;
	}
}
