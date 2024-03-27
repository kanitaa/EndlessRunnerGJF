using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GhostDeath : MonoBehaviour
{
    [SerializeField] GameObject _ghostHead;
    Vector3 _ghostHeadStartPosition;
    [SerializeField] Transform _ghostHeadTarget;

    [SerializeField] GameObject _ghostBody;
    Vector3 _ghostBodyStartPosition;
    [SerializeField] Transform _ghostBodyTarget;

    [SerializeField] GameObject _ghostTail;
    Vector3 _ghostTailStartPosition;
    [SerializeField] Transform _ghostTailTarget;


    private void Start()
    {
        _ghostHeadStartPosition = _ghostHead.transform.position;
        _ghostBodyStartPosition = _ghostBody.transform.position;
        _ghostTailStartPosition = _ghostTail.transform.position;

        gameObject.SetActive(false);

    }
    public void DeathEffect()
    {
        StartCoroutine(BreakGhost());

    }
    private IEnumerator BreakGhost()
    {
        float startTime = Time.time;

        while (Time.time < startTime + 0.5f)
        {
            float t = (Time.time - startTime) / 0.5f;


            _ghostHead.transform.position = Vector3.Lerp(_ghostHeadStartPosition, _ghostHeadTarget.position, t);
            _ghostBody.transform.position = Vector3.Lerp(_ghostBodyStartPosition, _ghostBodyTarget.position, t);
            _ghostTail.transform.position = Vector3.Lerp(_ghostTailStartPosition, _ghostTailTarget.position, t);


            yield return null;
        }

        _ghostHead.transform.position = _ghostHeadStartPosition;
        _ghostBody.transform.position = _ghostBodyStartPosition;
        _ghostTail.transform.position = _ghostTailStartPosition;

        gameObject.SetActive(false);

    }

}
