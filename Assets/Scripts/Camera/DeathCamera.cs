using UnityEngine;

public class DeathCamera : MonoBehaviour
{
    [SerializeField] Transform _target;
 
    private void Update()
    {
        transform.LookAt(_target);
    
    }
}
