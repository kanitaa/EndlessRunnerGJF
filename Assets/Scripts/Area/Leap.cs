using System.Collections;
using UnityEngine;

public class Leap : MonoBehaviour
{
    [SerializeField] private KeyCode _keyCode;
    public KeyCode KeyCode { get => _keyCode; }

    private Vector3 _startPosition;

    [SerializeField] private float _fallSpeed;

    private MeshRenderer _mesh;
    [SerializeField] private Material _ogMaterial, _glowMaterial;
    [SerializeField] private GameObject _glowParticle;

    private Rigidbody _rb;

    private void Start()
    {
        _startPosition = transform.localPosition;
        _mesh = GetComponent<MeshRenderer>();
        if (GetComponent<Rigidbody>() != null)
        {
            _rb = GetComponent<Rigidbody>();
        }
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

    public void DestroyLeap(Vector3 startPosition)
    {
        StartCoroutine(LeapDestruction(startPosition));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            DestroyLeap(other.transform.position);
        }
    }

    private IEnumerator LeapDestruction(Vector3 startPosition)
    {
        //Calculate direction from start position to current position.
        Vector3 explosionDirection = (transform.position - startPosition).normalized;

        // Apply force.
        _rb.AddForce(explosionDirection * 50, ForceMode.Impulse);

        // Wait before resetting the leap object.
        yield return new WaitForSeconds(30);

        //Stop rigidbody's movement and reset position.
        _rb.isKinematic = true;
        transform.localPosition = _startPosition;
        _rb.isKinematic = false;
    }
}

