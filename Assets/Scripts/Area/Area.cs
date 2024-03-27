using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] Transform _endPoint;

    [SerializeField] private bool _areaHasMonster;
    public bool AreaHasMonster { get => _areaHasMonster; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
            GameManager.Instance.AddArea(_endPoint);
    }
}
