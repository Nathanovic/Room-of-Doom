using UnityEngine;

//used to control the lave, this is done from the BossManager
public class LavaController : MonoBehaviour {

	public float maxY = 2f;
	public float riseSpeed = 3f;

	public MoveState moveState = MoveState.Stay;
	public enum MoveState{
		Rise,
		Stay
	}

	public void Rise(float newMaxY){
		maxY = newMaxY;
		moveState = MoveState.Rise;
	}

	private void Update(){
		if(moveState == MoveState.Rise){
			transform.Translate (Vector3.up * riseSpeed * Time.deltaTime, Space.World);
			if (transform.position.y > maxY) {
				moveState = MoveState.Stay;
			}
		}
	}
}
