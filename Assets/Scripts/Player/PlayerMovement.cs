using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;
    private CameraFollow _cam;
    private PlayerHealth _health;

    //Walk
    [SerializeField] private float _movementSpeed;
    private bool _isGrounded = true;
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }

    //Climb
    [SerializeField] private float _climbSpeed;
    private bool _isClimbing = false;

    //Jump
    private bool _isJumping = false;
    [SerializeField] private float _jumpForceUp = 5f;
    [SerializeField] private float _jumpForceForward = 5f;
    [SerializeField] private float _jumpDuration = 0.3f; 
    private float _jumpTimer = 0f;

    //Grip
    private bool _isLeaping = false;
    public bool IsLeaping { get => _isLeaping; set => _isLeaping = value; }

    [SerializeField] private float _gripSpeed;
    public float GripSpeed { get => _gripSpeed; set => _gripSpeed = value; }

    //Monster chase
    private bool _isMonsterChasing = false;
    public bool IsMonsterChasing { get => _isMonsterChasing; set => _isMonsterChasing = value; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _cam = Camera.main.GetComponent<CameraFollow>();
        _health = GetComponent<PlayerHealth>();
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
        //Move two units to left considering PathManager limits.
        Vector3 newPosition = transform.position + Vector3.left*2;
        newPosition.x = Mathf.Clamp(newPosition.x, GameManager.Instance.MinPathValue, GameManager.Instance.MaxPathValue);
        _rb.MovePosition(newPosition);
    }

    public void MoveRight()
    {
        //Move two units to right considering PathManager limits.
        Vector3 newPosition = transform.position + Vector3.right*2;
        newPosition.x = Mathf.Clamp(newPosition.x, GameManager.Instance.MinPathValue, GameManager.Instance.MaxPathValue);
        _rb.MovePosition(newPosition);
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
        }
        else
        {
            _isClimbing = false;
            _rb.useGravity = true;

            _anim.SetBool("Climb", false);
            
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            Jump(true);
        }
        
    }

    public void MonsterChase(bool isChasing)
    {
        if (isChasing)
        {
            _cam.IsPlayerChased = true;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            _cam.IsPlayerChased = false;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void Jump(bool climbJump=false)
    {
        //Jump only when grounded and ensure player isn't doing anything else.
        if(_isGrounded && !_isJumping && !_isClimbing)
        {
            Vector3 jumpVelocity = Vector3.up * _jumpForceUp;
            jumpVelocity += transform.forward * _jumpForceForward;

            _rb.velocity = jumpVelocity;

            _isJumping = true;
            _anim.SetTrigger("Jump");

        }
        //After climbing ends, make a smaller jump than usual.
        if (climbJump)
        {
            Vector3 jumpVelocity = Vector3.up * _jumpForceUp/2;
            _rb.velocity = jumpVelocity;

            _isJumping = true;
            _anim.SetTrigger("Jump");

        }
       
    }
   
    public void MoveToPosition(Vector3 targetPosition, bool gripFailed=false)
    {
        StartCoroutine(Movement(targetPosition, GripSpeed, gripFailed));
    }

    IEnumerator Movement(Vector3 targetPosition, float duration, bool gripFailed)
    {
        Vector3 startPosition = transform.position;
        Vector3 halfwayPosition = Vector3.zero;

        //Move target position slightly higher, so player isn't inside grip object.
        Vector3 modifiedTargetPosition = new Vector3(targetPosition.x, targetPosition.y + 2, targetPosition.z);

        //If grip fails, move player to half way where they were going.
        if (gripFailed)
        {
            halfwayPosition = (startPosition + modifiedTargetPosition) * 0.5f;
        }
            
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;

            if (!gripFailed)
            {
                transform.position = Vector3.Lerp(startPosition, modifiedTargetPosition, t);
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, halfwayPosition, t); 
            }

            yield return null;
        }

        // Ensure the object is exactly at the target position
        if (!gripFailed)
        {
            transform.position = modifiedTargetPosition;
        }
        else
        {
            _health.Die();
        }
            
        _isLeaping = false;
    }
}
