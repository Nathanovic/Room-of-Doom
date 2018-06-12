using UnityEngine;

//takes care of the character movement using the rb2D
public class PlayerMovement : MonoBehaviour {

	private PlayerInput input;
	private Rigidbody2D rb;
	private Animator anim;
	private PlayerBase baseScript;
    private CharacterAbilityBehaviour charBehabiour;
    private AudioSource audioSource;

    [Header("ground check values:")]
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask groundLM;

	[Header("movement values:")]
	public float moveSpeed;
	public AnimationCurve accelerationCurve;
	public float accelerationTime = 0.3f;
	private bool accelerate;
	public AnimationCurve deccelerationCurve;
	public float deccelerationTime = 0.3f;
	private float startDeccelerationMoment;

	private float curveT = 0f;//1 means max movementspeed; 0 means stand still
	private float curveValue;


    [Header("jump values:")]
    public bool canDoubleJump;
	public float doubleJumpFactor = 0.7f;
	public float jumpForce;
	public float minJumpCountdownTime = 0.5f;
	public float doubleJumpTime = 1.2f;
	private float remainingDoubleJumpTime = 0f;
	private Counter jumpCounter;
	private bool canJump;
	private bool didDoubleJump;
	private bool grounded;
    public ParticleSystem jumpPS;
	public ParticleSystem groundedPS;

	private void Start(){
		input = GetComponent<PlayerInput> ();
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren<Animator> ();
		baseScript = GetComponent<PlayerBase> ();
        charBehabiour = GetComponent<CharacterAbilityBehaviour>();

		jumpCounter = new Counter (minJumpCountdownTime);
		jumpCounter.onCount += EnableJumping;
		canJump = true;

        if (audioSource == null)
        { audioSource = transform.gameObject.AddComponent<AudioSource>(); }
        else
        {
            audioSource = transform.GetComponent<AudioSource>();
        }
    }

	private void Update(){
		if (!baseScript.canControl)
			return;

        if (charBehabiour.isCasting == false && charBehabiour.isStunned == false){
            MoveBehaviour();
            JumpBehaviour();
        }

        
	}

    public void StartCasting(){
        rb.velocity = new Vector2(0, 0);
        anim.SetFloat("moveSpeed", 0f);
    }

    #region Move Behaviour
    private void MoveBehaviour(){
		float inputValue = input.Lhorizontal;
        if (inputValue > 0 && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(AudioManager.instance.GetRandomClipFromName("Walk"));
        }

		if (inputValue != 0 && !accelerate) {
			curveT = GetCurveT (accelerationCurve);
			accelerate = true;
		} else if (inputValue == 0 && accelerate) {
			curveT = GetCurveT (deccelerationCurve);
			accelerate = false;
		}

		if (accelerate) {
			if (curveT < 1f) {
				curveT += Time.deltaTime / accelerationTime;
			}
			curveValue = accelerationCurve.Evaluate (curveT);
			CheckFacingDirection (transform.localScale.x, inputValue);
		} else {
			if (curveT < 1f) {
				curveT += Time.deltaTime / accelerationTime;
			}
			curveValue = deccelerationCurve.Evaluate (curveT);
			inputValue = transform.localScale.x;
		}

		rb.velocity = new Vector2 (curveValue * moveSpeed * inputValue, rb.velocity.y);
        

		anim.SetFloat ("moveSpeed", curveValue * Mathf.Abs(inputValue));
	}

	//used to update the curveT value if we switch from accelerating to deccelerating or in reverse
	private float GetCurveT(AnimationCurve newCurve){
		float closestDiff = 1f;
		float preferredT = 0f;

		float t = 0f;
		float step = 0.01f;

		while(t < 1f){
			t += step;
			float value = newCurve.Evaluate(t);
			float diff = Mathf.Abs (value - curveValue);
			if(diff < closestDiff) { 
				closestDiff = diff;
				preferredT = t;
			}
		}   
			
		return preferredT;
	}
	#endregion

	#region Jump Behaviour
	private void JumpBehaviour(){
		bool newGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLM);
		if (!grounded && newGrounded) {
			groundedPS.Play ();
		}
		grounded = newGrounded;

		if (remainingDoubleJumpTime > 0f) {
			remainingDoubleJumpTime -= Time.deltaTime;
		}
		if (input.ButtonIsDown(PlayerInput.Button.A) || Input.GetKeyDown(KeyCode.J)) {
			TryJump ();
		}		

		anim.SetBool ("grounded", grounded);
	}

	private void TryJump(){
		if ((canJump && grounded) || 
           ((!didDoubleJump && remainingDoubleJumpTime <= 0f) && canDoubleJump)){

            float jumpPower = jumpForce;
			if (!grounded) {
				canJump = false;
				didDoubleJump = true;
				jumpPower *= doubleJumpFactor;
			}
			else {
				remainingDoubleJumpTime = doubleJumpTime;
				didDoubleJump = false;
			}

			//jump if we are grounded/can double jump:
			jumpCounter.StartCounting ();
			rb.AddForce (Vector2.up * jumpPower);
			jumpPS.Play ();
		}
	}

	//set canJump to true (only called by jumpCounter.onCount)
	private void EnableJumping(){
		canJump = true;
	}
	#endregion

	//draw the gizmos for the ground check
	private void OnDrawGizmos(){
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere (groundCheck.position, groundCheckRadius);
	}

	//make sure we are facing the right direction
	private void CheckFacingDirection(float localX, float moveSpeed){
		if((moveSpeed > 0 && localX < 0) || (moveSpeed < 0 && localX > 0)){
			float newXScale = (transform.localScale.x > 0f) ? -1f : 1f;
			transform.localScale = new Vector3 (newXScale, 1f, 1f);
		}
	}
}
