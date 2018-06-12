using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

//used for laser attack
[RequireComponent(typeof(WormMovement))]
public class AttackLineBehaviour : MonoBehaviour, IWormTraverseable {

	public float undergroundY = -1.5f;
	private Vector2 startPoint, endPoint;
	private List<float> possibleHeights{
		get { 
			return BossManager.instance.regionHeights;
		}
	}

	private WormMovement moveScript;
	private AttackState attackState;

	public float moveDownSpeed = 5f;

	public int attackDamage;

	public LaserAttack laserAttack;
	private Vector3 laserTargetPos;
    private AudioSource audioSource;

    private void Start(){
		startPoint = endPoint = new Vector2 ();

		moveScript = GetComponent<WormMovement> ();

		GameObject laserObj = GameObject.Instantiate (laserAttack.prefab, transform) as GameObject;
		laserObj.transform.localPosition = Vector3.zero;
		laserObj.GetComponent<Laser> ().damage = attackDamage;
		laserAttack.Init (laserObj, transform, moveScript);

        if (audioSource == null)
        { audioSource = transform.gameObject.AddComponent<AudioSource>(); }
        else
        {
            audioSource = transform.GetComponent<AudioSource>();
        }

    }

	public void Prepare(Vector3 startPos, Vector3 enemyPos){
		moveScript.onReachedLineEnd += StartAttack;
		startPoint = startPos;
		endPoint.x = startPoint.x;
		int rndmIndex = Random.Range (0, possibleHeights.Count);
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
		int rndmIndex = Random.Range (0, possibleHeights.Count);
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

	public bool SpawnCentered(){
		return true;
	}

	private void MoveUp(WormSegment[] wormSegments){
		foreach (WormSegment segment in wormSegments) {
			segment.TraverseCurve (this);
		}		
	}

	private void StartAttack(){
		moveScript.onReachedLineEnd -= StartAttack;
		attackState = AttackState.Attack;
        audioSource.PlayOneShot(AudioManager.instance.GetClipFromName("MagmaWorm", 3));
        moveScript.RotateHeadToPos (laserTargetPos);
		laserAttack.StartAttack (laserTargetPos, StopAttack);
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

	public void RotateHeadToLaserpoint(Vector3 point){
		moveScript.RotateHeadToPos (point);
	}

	[System.Serializable]
	public class LaserAttack{
		private WormMovement moveScript;
		private Transform parentTransform;
		private GameObject laser;
		private Transform laserLooker;
		private Collider2D laserColl;

		public GameObject prefab;

		public float appearDuration;
		public float rotateDuration;
		public float stayDuration;

		private UnityAction finishCallback;
        private AudioSource audioSource;


		public void Init(GameObject spawnedLaser, Transform transform, WormMovement _moveScript){
			laser = spawnedLaser;
			laser.SetActive (false);
			laserColl = laser.GetComponent<Collider2D> ();
			parentTransform = transform;
			laserLooker = laser.transform.GetChild (0);
			moveScript = _moveScript;
            if (audioSource == null)
            { audioSource = transform.gameObject.AddComponent<AudioSource>(); }
            else
            {
                audioSource = transform.GetComponent<AudioSource>();
            }

        }

		public void StartAttack(Vector3 targetPos, UnityAction callback){
			finishCallback = callback;
			moveScript.StartCoroutine (BeamAttack (
					parentTransform.position, 
					targetPos)
			);
		}

		private IEnumerator BeamAttack(Vector3 myPos, Vector3 targetPos){
			//prepare:
			Vector3 startEuler = (targetPos.x > myPos.x) ? -Vector3.forward * 90f : Vector3.forward * 90f;
			Quaternion startRot = Quaternion.Euler (startEuler);
			Quaternion endRot = laser.transform.rotation;
			laser.transform.rotation = startRot;
			laser.SetActive (true);

			//appear
			float t = 0;
			while (t < 1f) {
				t += Time.deltaTime / appearDuration;
				laser.transform.localScale = new Vector3 (t, 1f, 1f);
				yield return null;
			}
			laserColl.enabled = true;

            //sound here
            audioSource.PlayOneShot(AudioManager.instance.GetClipFromName("MagmaWorm", 2));

            //rotate
            t = 0;
			while (t < 1f) {
				t += Time.deltaTime / rotateDuration;
				laser.transform.rotation = Quaternion.Lerp (startRot, endRot, t);
				moveScript.RotateHeadToPos (laserLooker.position);
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
