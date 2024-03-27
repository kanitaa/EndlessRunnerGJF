using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _chaseOffset;

    private bool _isPlayerChased = false;
    public bool IsPlayerChased { get => _isPlayerChased; set => _isPlayerChased = value; }


    [SerializeField] private float smoothTime = 0.125f;

    private Vector3 velocity = Vector3.zero;


    void LateUpdate()
    {
        if (_target != null)
        {
            Vector3 newPosition;

            if (!_isPlayerChased)
                newPosition = _target.position + _offset;
            else
                newPosition = _target.position + _chaseOffset;

            
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

            //Keep the camera's x position the same.
            smoothedPosition.x = transform.position.x;

            transform.position = smoothedPosition;
        }
    }
}
