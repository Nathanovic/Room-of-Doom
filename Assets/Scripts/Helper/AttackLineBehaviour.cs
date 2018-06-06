using UnityEngine;

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
	public float attackDuration = 2f;
	public float laserReach = 30f;
	private float remainingAttackTime;

	public Laser laser;
	private Vector3 laserTargetPos;

	private void Start(){
		startPoint = endPoint = new Vector2 ();
		possibleHeights = BossManager.instance.regionHeights;

		moveScript = GetComponent<WormMovement> ();
		moveScript.onReachedLineEnd += StartAttack;

		laser = Laser.Instantiate (laser, transform) as Laser;
		laser.Init (attackDamage);
	}

	public void Prepare(Vector3 startPos, Vector3 enemyPos){
		startPoint = startPos;
		endPoint.x = startPoint.x;
		int rndmIndex = Random.Range (0, possibleHeights.Length);
		endPoint.y = possibleHeights [rndmIndex];
		attackState = AttackState.MoveUp;

		laserTargetPos = new Vector2 ();
		int dir = enemyPos.x > startPos.x ? 1 : -1;
		laserTargetPos.x = startPoint.x + dir * laserReach;
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
		case AttackState.Attack:
			Attack ();
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
		remainingAttackTime = attackDuration;
		attackState = AttackState.Attack;
		Vector3[] laserPoints =	new Vector3[] {
				transform.position, 
				laserTargetPos
			};

		Vector2 laserOffset = new Vector2 (0.5f * (laserTargetPos.x - transform.position.x), 0f);
		Vector2 laserSize = new Vector2 (Mathf.Abs(laserOffset.x) * 2f, 1f);

		laser.Enable (laserOffset, laserSize, laserPoints);
	}

	private void Attack(){
		remainingAttackTime -= Time.deltaTime;
		if (remainingAttackTime <= 0f) {
			laser.Disable ();
			attackState = AttackState.MoveDown;		
		}
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

	public enum AttackState{
		MoveUp,
		Attack,
		MoveDown
	}
}
