using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _health;
    private Animator _anim;

    private bool _isDead = false;
    private void Start()
    {
        _health = GameManager.Instance.PlayerHealth;
        _anim = GetComponent<Animator>();
    }
    public void TakeDamage()
    {
        if (_isDead) return;

        _anim.SetBool("Run", false);

        _health--;
        GameManager.Instance.PlayerHealth--;
        UIManager.Instance.UpdateLives();
        AudioManager.Instance.PlaySound("ScreamsShouts2_Humans_Female_shout-of-pain_028", true);

        if (_health < 1)
            Die();
        else
            _anim.SetTrigger("TakeHit");

        
    }
  
    public void Die()
    {
        if (_isDead) return;

        _isDead = true;

        GameManager.Instance.PlayerHealth=0;
        UIManager.Instance.UpdateLives();
       

        AudioManager.Instance.PlayMusic("Rise (live vocals)");
        Camera.main.GetComponent<DeathCamera>().Death();


        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;
        GetComponent<Rigidbody>().useGravity = true;

        
        _anim.SetBool("Run", false);

        _anim.SetTrigger("Die");

        Invoke("EndGame", 5);

    }

    private void EndGame()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.End);
    }
}
