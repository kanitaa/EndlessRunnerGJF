using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    private PlayerMovement _movement;
    private PlayerHealth _health;

    private void Start()
    {
        _movement = transform.parent.GetComponent<PlayerMovement>();
        _health = transform.parent.GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) 
            _movement.IsGrounded = true;

        if (other.CompareTag("Wall")) 
            _movement.WallClimb(true);

        if (other.CompareTag("GripTrigger") && !_movement.GripSequenceStarted)
        {
            other.GetComponentInParent<GripContainer>().StartGripSequence(_movement, other.gameObject);
            _movement.StartGripping();
        }

        if (other.CompareTag("Monster") && !_health.IsDead)
        {
            Debug.Log("Monster hit");
            Transform endPosition = other.GetComponent<Monster>().BounceEnd;
            _movement.MoveToGrip(endPosition);
        }

        if (other.CompareTag("Obstacle")) 
            _health.TakeDamage();

        if (other.CompareTag("Death")) 
            _health.Die();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            _movement.IsGrounded = true;
          
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall")) _movement.WallClimb(false);
        if (other.CompareTag("Ground")) _movement.IsGrounded = false;
    }
}
