using System.Collections;
using TMPro;
using UnityEngine;

public class Leap : MonoBehaviour
{
    enum LeapPosition { Left, Right };

    [SerializeField] LeapPosition _position;

    public KeyCode KeyCode;


    private Vector3 _startPosition;
    [SerializeField] private float _fallSpeed;

    private MeshRenderer _mesh;

    [SerializeField] Material _ogMaterial, _glowMaterial;
    [SerializeField] GameObject _glowParticle;

    private void Start()
    {
        _startPosition = transform.localPosition;
        _mesh = GetComponent<MeshRenderer>();
    }

    public void EnableGlow(bool IsGlowing)
    {
        if (IsGlowing)
        {
            _mesh.material = _glowMaterial;
            _glowParticle.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            _mesh.material = _ogMaterial;
            _glowParticle.SetActive(false);
            _glowParticle.SetActive(true);
        }
    }

    public void DestroyGrip(Vector3 startPosition)
    {
        StartCoroutine(GripDestruction(startPosition));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
            DestroyGrip(other.transform.position);
    }

    IEnumerator GripDestruction(Vector3 startPosition)
    {
        // Calculate direction from start position to current position
        Vector3 explosionDirection = (transform.position - startPosition).normalized;

        // Apply force in the calculated direction
        GetComponent<Rigidbody>().AddForce(explosionDirection * 50, ForceMode.Impulse);

        // Wait for a short duration to allow the object to move due to the applied force
        yield return new WaitForSeconds(30);

        // Perform any other actions after the explosion effect
        //Stop rigidbody's movement.
        GetComponent<Rigidbody>().isKinematic = true;
        transform.localPosition = _startPosition;
        GetComponent<Rigidbody>().isKinematic = false;

    }
}

