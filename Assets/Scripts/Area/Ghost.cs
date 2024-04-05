using UnityEngine;

public class Ghost : MonoBehaviour
{
    private GameObject _ghostVisual;
    private CapsuleCollider _collider;
    [SerializeField] private GameObject _deathEffect;
    private ParticleSystem _deathParticles;

    [SerializeField] private int _scoreValue;

    private void Start()
    {
        _ghostVisual = transform.GetChild(0).gameObject;
        _collider = GetComponent<CapsuleCollider>();
        _deathParticles = _deathEffect.GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cake"))
        {
            Die();
        }
            
    }
    void Die()
    {
        AudioManager.Instance.PlaySound("Bloody punch", true);
        GameManager.Instance.IncreaseScore(_scoreValue);

        _ghostVisual.SetActive(false);
        _collider.enabled = false;

        _deathParticles.Play();
        
        Invoke("Respawn", 30);
    }

    private void Respawn()
    {
        _ghostVisual.SetActive(true);
        _collider.enabled = true;

        _deathEffect.SetActive(false);
        _deathEffect.SetActive(true);
    }
}
