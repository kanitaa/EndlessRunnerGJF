using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerMovement _movement;
    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
    }
    private void MovementInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) 
            _movement.MoveLeft();
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) 
            _movement.MoveRight();

        if (Input.GetKeyDown(KeyCode.Space)) 
            _movement.Jump();

    }
    private void UIInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) UIManager.Instance.TogglePause();

    }
    private void Update()
    {
        MovementInput();
        UIInput();
    }
}
