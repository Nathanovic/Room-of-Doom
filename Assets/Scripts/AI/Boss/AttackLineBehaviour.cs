using UnityEngine;
using System.Collections;
using UnityEngine.Events;

//used for laser attack
[RequireComponent(typeof(WormMovement))]
public class AttackLineBehaviour : MonoBehaviour, IWormTraverseable {

	public float undergroundY = -1.5f;
	private Vector2 startPoint, endPoint;
	private float[] possibleHeights;

	private WormMovement moveScript;
	private AttackState attackState;

	public float moveDownSpeed = 5f;

	public int attackDamage;

	public LaserAttack laserAttack;
	private Vector3 laserTargetPos;

	private void Start(){
		startPoint = endPoint = new Vector2 ();
		possibleHeights = BossManager.instance.regionHeights;

		moveScript = GetComponent<WormMovement> ();

		GameObject laserObj = GameObject.Instantiate (laserAttack.prefab, transform) as GameObject;
		laserObj.transform.localPosition = Vector3.zero;
		laserObj.GetComponent<Laser> ().damage = attackDamage;
		laserAttack.Init (laserObj, transform);
	}

	public void Prepare(Vector3 startPos, Vector3 enemyPos){
		moveScript.onReachedLineEnd += StartAttack;
		startPoint = startPos;
		endPoint.x = startPoint.x;
		int rndmIndex = Random.Range (0, possibleHeights.Length);
		endPoint.y = possibleHeights [rndmIndex];
		attackState = AttackState.MoveUp;

		laserTargetPos = new Vector2 ();
		int dir = enemyPos.x > startPos.x ? 1 : -1;
		laserTargetPos.x = startPoint.x + dir;
		laserTargetPos.y = endPoint.y;
	}

	public void Prepare(Vector2 start){
		startPoint = start;
		endPoint.x = start.x;
		int rndmIndex = Random.Range (0, possibleHeights.Length);
		endPoint.y = possibleHeights [rndmIndex];
		attackState = AttackState.MoveUp;
	}

	public float GetLength () {
		return endPoint.y - startPoint.y;
	}

	public void Run(WormSegment[] wormSegments){
		switch (attackState) {
		case AttackState.MoveUp:
			MoveUp (wormSegments);
			break;
		case AttackState.MoveDown:
			MoveDown ();
			break;
		}
	}

	private void MoveUp(WormSegment[] wormSegments){
		foreach (WormSegment segment in wormSegments) {
			segment.TraverseCurve (this);
		}		
	}

	private void StartAttack(){
		moveScript.onReachedLineEnd -= StartAttack;
		attackState = AttackState.Attack;

		moveScript.RotateHeadToPos (laserTargetPos);
		laserAttack.StartAttack (this, laserTargetPos, StopAttack);
	}

	private void StopAttack(){
		attackState = AttackState.MoveDown;				
	}

	private void MoveDown(){
		transform.Translate (Vector3.down * moveDownSpeed * Time.deltaTime);
		if (transform.position.y < undergroundY) {
			moveScript.EndAttack ();
		}
	}

	public Vector3 GetPoint (float t) {
		return Vector3.Lerp (startPoint, endPoint, t);
	}

	private void ReachedLineEnd(){
		attackState = AttackState.MoveDown;
	}

	[System.Serializable]
	public class LaserAttack{
		private Transform parentTransform;
		private GameObject laser;
		private Collider2D laserColl;

		public GameObject prefab;

		public float appearDuration;
		public float rotateDuration;
		public float stayDuration;

		private UnityAction finishCallback;

		public void Init(GameObject spawnedLaser, Transform transform){
			laser = spawnedLaser;
			laser.SetActive (false);
			laserColl = laser.GetComponent<Collider2D> ();
			this.parentTransform = transform;
		}

		public void StartAttack(MonoBehaviour ienumeratable, Vector3 targetPos, UnityAction callback){
			finishCallback = callback;
			laser.SetActive (true);
			laser.transform.rotation = Quaternion.Euler (0, 0, 0);
			ienumeratable.StartCoroutine (BeamAttack (
					parentTransform.position, 
					targetPos)
			);
		}

		private IEnumerator BeamAttack(Vector3 myPos, Vector3 targetPos){
			//appear
			float t = 0;
			while (t < 1f) {
				t += Time.deltaTime / appearDuration;
				laser.transform.localScale = new Vector3 (t, 1f, 1f);
				yield return null;
			}
			laserColl.enabled = true;

			//rotate
			Quaternion startRot = laser.transform.rotation;
			Vector3 endEuler = (targetPos.x > myPos.x) ? -Vector3.forward * 90f : Vector3.forward * 90f;
			Quaternion endRot = Quaternion.Euler (endEuler);
			t = 0;
			while (t < 1f) {
				t += Time.deltaTime / rotateDuration;
				laser.transform.rotation = Quaternion.Lerp (startRot, endRot, t);
				yield return null;
			}

			//stay
			yield return new WaitForSeconds (stayDuration);

			StopAttack ();
		}

		private void StopAttack(){
			laserColl.enabled = false;
			laser.SetActive (false);
			finishCallback ();
		}
	}

	public enum AttackState{
		MoveUp,
		Attack,
		MoveDown
	}
}
