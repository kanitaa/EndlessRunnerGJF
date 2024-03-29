using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] Transform _endPoint;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddArea(_endPoint);
            Debug.Log("Player entered area.");
        }
           
    }
}
