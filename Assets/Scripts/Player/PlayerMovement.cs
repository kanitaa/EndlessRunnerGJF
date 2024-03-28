using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;

    [SerializeField] private float _movementSpeed, _climbSpeed;
   
    [SerializeField] private bool _isClimbing = false;

    [SerializeField] private bool _isJumping = false;

    [SerializeField] private float _jumpForceUp = 5f;
    [SerializeField] private float _jumpForceForward = 5f;
    [SerializeField] private float _jumpDuration = 0.3f; // Adjust the duration of the jump as needed
    private float _jumpTimer = 0f; // Timer to track the duration of the jump


    [SerializeField] private bool _isGrounded = true;
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }


    [SerializeField] private bool _gripSequenceStarted = false;
    public bool GripSequenceStarted { get => _gripSequenceStarted; set => _gripSequenceStarted = value; }

    [SerializeField] private float _gripSpeed;
    public float GripSpeed { get => _gripSpeed; set => _gripSpeed = value; }


    [SerializeField] private bool _isMonsterChasing = false;
    public bool IsMonsterChasing { get => _isMonsterChasing; set => _isMonsterChasing = value; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

  
    private void FixedUpdate()
    {
        if (_isJumping)
        {
            _jumpTimer += Time.fixedDeltaTime;

            if (_jumpTimer >= _jumpDuration)
            {
                _isJumping = false;
                _jumpTimer = 0f;
            }
        }
        else if (_isGrounded && !_isClimbing)
        {
            _rb.velocity = transform.forward * _movementSpeed;

        }
        else if (_isClimbing)
        {
            _rb.velocity = transform.forward * _climbSpeed;
        }
        else
        {
            _rb.AddForce(Physics.gravity * _rb.mass);

        }
    }

    public void MoveLeft()
    {
        // Move two units to left considering PathManager limits.
        Vector3 newPosition = transform.position + Vector3.left*2;
        newPosition.x = Mathf.Clamp(newPosition.x, GameManager.Instance.MinPathValue, GameManager.Instance.MaxPathValue);
        _rb.MovePosition(newPosition);
    }

    public void MoveRight()
    {
        // Move two units to right considering PathManager limits.
        Vector3 newPosition = transform.position + Vector3.right*2;
        newPosition.x = Mathf.Clamp(newPosition.x, GameManager.Instance.MinPathValue, GameManager.Instance.MaxPathValue);
        _rb.MovePosition(newPosition);
    }

    public void Walk()
    {
        _rb.useGravity = true;
        _isClimbing = false;
        _isJumping = false;
        _jumpTimer = 0f;
        _anim.SetBool("Run", true);
    }
    public void WallClimb(bool isClimbing)
    {
        if (isClimbing)
        {
            _isClimbing = true;
            _rb.useGravity = false;

            _anim.SetBool("Climb", true);

            transform.localRotation = Quaternion.Euler(-90, 0, 0);
            transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
            Debug.Log("Climbing");
        }
        else
        {
            
            _isClimbing = false;
            _rb.useGravity = true;
            Jump(true);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            _anim.SetBool("Climb", false);
            Debug.Log("Finished climb");
        }
        
    }

    public void MonsterChase(bool isChasing)
    {
        if (isChasing)
        {
            Camera.main.GetComponent<CameraFollow>().IsPlayerChased = true;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            Camera.main.GetComponent<CameraFollow>().IsPlayerChased = false;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

    }

    public void Jump(bool climbJump=false)
    {
        if(_isGrounded && !_isJumping && !_isClimbing)
        {

            // Set the Rigidbody's velocity to the desired jump velocity
            Vector3 jumpVelocity = Vector3.up * _jumpForceUp;

            // Add forward velocity to the jump (optional)
            jumpVelocity += transform.forward * _jumpForceForward;

            // Set the Rigidbody's velocity
            _rb.velocity = jumpVelocity;

            // Set jump flag and trigger jump animation
            _isJumping = true;
            _anim.SetTrigger("Jump");

            Debug.Log("Jump");

        }
        if (climbJump)
        {
            Debug.Log("Climju");
            // Set the Rigidbody's velocity to the desired jump velocity
            Vector3 jumpVelocity = Vector3.up * _jumpForceUp/2;

           
            jumpVelocity = Vector3.up * _jumpForceForward;

            // Set the Rigidbody's velocity
            _rb.velocity = jumpVelocity;

            // Set jump flag and trigger jump animation
            _isJumping = true;
            _anim.SetTrigger("Jump");

        }
       
    }
   
    public void StartGripping()
    {
        _gripSequenceStarted = true;
       // _rb.useGravity = false;
    }
    public void MoveToGrip(Transform grip, bool gripFailed=false)
    {
        StartCoroutine(GripMovement(grip, GripSpeed, gripFailed));
    }

    IEnumerator GripMovement(Transform target, float duration, bool gripFailed)
    {
        Vector3 startPosition = transform.position;
        Vector3 halfwayPosition = Vector3.zero;

        if (gripFailed) 
            halfwayPosition = (startPosition + target.position) * 0.5f;

        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;

            if (!gripFailed)
                transform.position = Vector3.Lerp(startPosition, target.position, t);
            else
            {
                transform.position = Vector3.Lerp(startPosition, halfwayPosition, t);
               
            }

            yield return null;
        }

        // Ensure the object is exactly at the target position
        if (!gripFailed) 
            transform.position = target.position;
        else
            GetComponent<PlayerHealth>().Die();

    }

   
  
}
