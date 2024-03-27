using UnityEngine;

public class DeathCamera : MonoBehaviour
{
    private bool _playerDied;
    Transform _target;
   public void Death()
    {
        _target = transform.parent;
        transform.parent = null;
        _playerDied = true;
        
    }

    private void Update()
    {
        if(_playerDied) 
            transform.LookAt(_target);
    }
}
