using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//handle the movement of the magma worm
//the body exists out of segments, and each segments just follows the head
//handles the magma worm attacks:
//attacks the targets it has by changing the moveCurve.points
public class WormMovement : MonoBehaviour {

	public WormSegment[] wormSegments{ get; private set; }

	public float startDelay = 1f;
	private float delay = 0f;
	private Transform head;
	public float headLength;

	private IWormTraverseable[] attackTraverseables;
	private IWormTraverseable attackTraverseable;
	private float moveT = 0f;
	private float traverseSpeed;

	public float minMoveSpeed = 5f;
	public float maxMoveSpeed = 10f;
	private float movementSpeed = 2f;

	public event SimpleDelegate onReachedLineEnd;
	public event SimpleDelegate onAttackEnded;

	private void Start(){
		head = transform.GetChild (0);

		attackTraverseables = GetComponents<IWormTraverseable> ();
		wormSegments = GetComponentsInChildren<WormSegment> ();
	}

	public void AttackUpdate(){
		if (delay > 0f) {
			delay -= Time.deltaTime;
		} 
		else {
			if (moveT < 1f) {
				moveT += traverseSpeed * Time.deltaTime;
				transform.position = attackTraverseable.GetPoint (moveT);

				Vector3 dir = attackTraverseable.GetPoint (moveT + 0.1f) - transform.position;
				float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg + 180;
				head.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

				if (moveT >= 1f && onReachedLineEnd != null) {
					Debug.Log ("reached the end");
					onReachedLineEnd ();
				}
			}

			attackTraverseable.Run (wormSegments);
		}
	}

	public void PrepareAttack(Vector3 startPos, Vector3 enemyPos, float attackIntensity){
		int rndmTraverseableIndex = Random.Range (0, attackTraverseables.Length);
		attackTraverseable = attackTraverseables [rndmTraverseableIndex];
		attackTraverseable.Prepare (startPos, enemyPos);
		StartAttack (attackIntensity);
	}

	public void EndAttack(){
		if (onAttackEnded != null) {
			onAttackEnded ();
		}
	}

	private void StartAttack(float attackIntensity){
		float moveCurveLength = attackTraverseable.GetLength ();
		movementSpeed = Mathf.Lerp (minMoveSpeed, maxMoveSpeed, attackIntensity);
		traverseSpeed = movementSpeed / moveCurveLength;

		Transform prevSegment = head;
		float beforeSegmentDelay = headLength / moveCurveLength;
		foreach (WormSegment segment in wormSegments) {
			segment.Prepare (prevSegment, beforeSegmentDelay, traverseSpeed);
			beforeSegmentDelay += segment.length / moveCurveLength;
			prevSegment = segment.transform;
		}

		moveT = 0f;
		delay = startDelay;
	}
}
