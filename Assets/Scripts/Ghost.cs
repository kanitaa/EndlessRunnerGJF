using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private GameObject _dyingGhost;

    [SerializeField] private int _scoreValue;
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cake"))
            Die();
    }
    void Die()
    {
        AudioManager.Instance.PlaySound("Bloody punch", true);
        GameManager.Instance.IncreaseScore(_scoreValue);
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<CapsuleCollider>().enabled = false;
        _dyingGhost.SetActive(true);
        _dyingGhost.GetComponent<GhostDeath>().DeathEffect();

        Invoke("Respawn", 30);
    }

    private void Respawn()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<CapsuleCollider>().enabled = true;
    }
}
