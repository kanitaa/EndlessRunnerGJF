using System.Collections;
using UnityEngine;

public class Grip : MonoBehaviour
{
    enum GripPosition { Left, Middle, Right };

    [SerializeField] GripPosition _position;

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

    public void DestroyGrip()
    {
        StartCoroutine(GripFall());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
            DestroyGrip();
    }

    IEnumerator GripFall()
    {
        float startY = transform.position.y;
        float targetY = 0f;

        while (transform.position.y > targetY)
        {
            float step = _fallSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), step);
            yield return null;
        }

        Debug.Log("Hit ground");
        // Do something else after hitting the ground


    }
}

