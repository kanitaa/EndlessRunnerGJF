using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _nextLetter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_nextLetter != null)
            {
                _nextLetter.SetActive(true);
            }
            gameObject.SetActive(false);
            
        }
    }
}
