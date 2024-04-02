using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _chaseOffset;

    private bool _isPlayerChased = false;
    public bool IsPlayerChased { set => _isPlayerChased = value; }


    [SerializeField] private float smoothTime = 0f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (_target != null)
        {
            Vector3 newPosition;

            if (!_isPlayerChased)
            {
                newPosition = _target.position + _offset;
            }
            else
            {
                newPosition = _target.position + _chaseOffset;
            }
                
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

            //Keep the camera's x position.
            smoothedPosition.x = transform.position.x;
            transform.position = smoothedPosition;
        }
    }
}
