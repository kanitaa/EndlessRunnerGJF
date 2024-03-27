using UnityEngine;

public class Grip : MonoBehaviour
{
    enum GripPosition { Left, Middle, Right };

    [SerializeField] GripPosition _position;

    public KeyCode KeyCode;

  
    public void DestroyGrip()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
            DestroyGrip();
    }
}

