using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    private PlayerMovement _movement;
    private PlayerHealth _health;

    [SerializeField] private LayerMask _groundLayer;

    private void Start()
    {
        _movement = transform.parent.GetComponent<PlayerMovement>();
        _health = transform.parent.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        //Ground detection.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1, _groundLayer))
        {
            _movement.IsGrounded = true;
        }
        else
        {
            _movement.IsGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            _movement.WallClimb(true);
        }

        if (other.CompareTag("LeapTrigger") && !_movement.IsLeaping)
        {
            other.GetComponentInParent<LeapContainer>().StartLeapSequence(_movement, other.gameObject);
            _movement.IsLeaping = true;
        }

        if (other.CompareTag("Monster") && !_health.IsDead)
        {
            Vector3 endPosition = other.GetComponent<Monster>().BounceEnd.position;
            _movement.MoveToPosition(endPosition);
        }

        if (other.CompareTag("Obstacle"))
        {
            _health.TakeDamage();
        }

        if (other.CompareTag("Death"))
        {
            _health.Die();
        }
           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            _movement.WallClimb(false);
        }
    }
}
