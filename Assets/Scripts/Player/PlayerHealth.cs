using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _health;
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    public void TakeDamage()
    {
        _anim.SetBool("Run", false);

        _health--;
        if (_health < 1)
            Die();
        else
            _anim.SetTrigger("TakeHit");

        
    }

    public void Die()
    {
        Debug.Log("Dead");

        Camera.main.GetComponent<DeathCamera>().Death();
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;

        

        _anim.SetTrigger("Die");

        Invoke("EndGame", 3);

    }

    private void EndGame()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.End);
    }
}
