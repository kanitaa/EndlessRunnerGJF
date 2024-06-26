using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action UpdateLives;

    private Animator _anim;
    private PlayerInput _input;
    private PlayerMovement _movement;
    private Rigidbody _rb;
    private CameraFollow _camFollow;
    private DeathCamera _deathCam;

    [SerializeField] private int _health;
    public int Health { get => _health; }

    private bool _isDead = false;
    public bool IsDead { get => _isDead; }

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        _anim = GetComponent<Animator>();
        _input = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody>();
        _camFollow = Camera.main.GetComponent<CameraFollow>();
        _deathCam = Camera.main.GetComponent<DeathCamera>();
    }
    public void TakeDamage()
    {
        if (_isDead)
        {
            return;
        }
      
        _health--;
       
        AudioManager.Instance.PlaySound("110010__tuberatanka__cat-meow-ii", true);

        if (_health < 1)
        {
            Die();
        }
        else
        {
            _anim.SetTrigger("TakeHit");
            UpdateLives?.Invoke();
        }
        
    }
  
    public void Die()
    {
        if (_isDead)
        {
            return;
        }

        _isDead = true;
        _health = 0;
        UpdateLives?.Invoke();

        _anim.SetTrigger("Die");
        AudioManager.Instance.PlayMusic("Rise (live vocals)");

        _movement.enabled = false;
        _input.enabled = false;
        _rb.useGravity = true;

        _camFollow.enabled = false;
        _deathCam.enabled = true;

        Invoke("EndGame", 5);

    }

    private void EndGame()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.End);
    }
}
