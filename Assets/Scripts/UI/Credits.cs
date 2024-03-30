using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    private ScrollRect _scroll;

    [SerializeField] private float _targetPosition = 0f;
    private float _startPosition;

    [SerializeField] private float _scrollDuration;

    private void Start()
    {
        _scroll = GetComponent<ScrollRect>();
        _startPosition = _scroll.content.localPosition.y;
    }
  
    IEnumerator ScrollToPosition(float targetPosition)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _scrollDuration)
        {
            elapsedTime += Time.deltaTime;
            float newPosition = Mathf.Lerp(_startPosition, targetPosition, elapsedTime / _scrollDuration);
            _scroll.content.localPosition = new Vector2(0, newPosition);
            yield return null;
        }

        _scroll.content.localPosition = new Vector2(0, targetPosition);
    }

    public void PlayCredits()
    {
        _scroll.content.localPosition = new Vector2(0, _startPosition);

        StartCoroutine(ScrollToPosition(_targetPosition));
    }
}

