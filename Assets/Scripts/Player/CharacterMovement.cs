using UnityEngine;

//takes care of the character movement using the rb2D
public class CharacterMovement : MonoBehaviour {

	public float moveSpeed;
	public float jumpForce;

	private Rigidbody2D rb;
	private Animator anim;

	[Header("ground check values:")]
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask groundLM;

	[Header("jump countdown values:")]
	public float minJumpCountdownTime = 0.5f;
	private Counter jumpCounter;
	private bool canJump;

	private void Start(){
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren<Animator> ();

		jumpCounter = new Counter (minJumpCountdownTime);
		jumpCounter.onCount += EnableJumping;
		canJump = true;
	}

	private void Update(){
		float horizontalInput = Input.GetAxis ("Horizontal");
		float horizontalSpeed = horizontalInput * moveSpeed;
		rb.velocity = new Vector2 (horizontalSpeed, rb.velocity.y);

		if (horizontalSpeed != 0) {
			CheckFacingDirection (transform.localScale.x, horizontalSpeed);
		}

		if (Input.GetButtonDown ("Jump") && canJump) {
			//jump if we are grounded:
			if (Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLM)) {
				canJump = false;
				jumpCounter.StartCounting ();
				rb.AddForce (Vector2.up * jumpForce);
			}
		}
	}

	//set canJump to true (only called by jumpCounter.onCount)
	private void EnableJumping(){
		canJump = true;
	}

	//draw the gizmos for the ground check
	private void OnDrawGizmos(){
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere (groundCheck.position, groundCheckRadius);
	}

	//make sure we are looking in the right direction
	private void CheckFacingDirection(float localX, float moveSpeed){
		if((moveSpeed > 0 && localX < 0) || (moveSpeed < 0 && localX > 0)){
			float newXScale = (transform.localScale.x > 0f) ? -1f : 1f;
			transform.localScale = new Vector3 (newXScale, 1f, 1f);
		}
	}
}
