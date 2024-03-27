using UnityEngine;

public class MonsterCamera : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _transitionDuration = 1f;
    private float _transitionTimer = 0f;
    private bool _isTransitioning = false;

    public void MonsterChase(bool isChasing)
    {
        if (isChasing)
        {
            _startPosition = transform.localPosition;
            _endPosition = new Vector3(0, 6.25f, -14);
        }
        else
        {
            _startPosition = transform.localPosition;
            _endPosition = new Vector3(0, 6.25f, -7);
        }

        _transitionTimer = 0f;
        _isTransitioning = true;
    }

    private void Update()
    {
        if (_isTransitioning)
        {
            _transitionTimer += Time.deltaTime;

            // Calculate the interpolation factor (0 to 1)
            float t = Mathf.Clamp01(_transitionTimer / _transitionDuration);

            // Smoothly interpolate between start and end positions
            transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, t);

            // Check if transition is complete
            if (_transitionTimer >= _transitionDuration)
            {
                _isTransitioning = false;
            }
        }
    }
}
