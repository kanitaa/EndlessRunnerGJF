using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWidthTrigger : MonoBehaviour
{
    [SerializeField] private int _minPath, _maxPath;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.MinPathValue = _minPath;
            GameManager.Instance.MaxPathValue = _maxPath;
        }
    }
}
