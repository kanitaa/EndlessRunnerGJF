using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement _movement;
    private Throw _throw;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _throw = GetComponent<Throw>();
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

    private void ShootInput()
    {
        if (GameManager.Instance.IsPaused) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                _throw.CakeThrow(hit.point);
            }

        }
    }
    private void UIInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            UIManager.Instance.TogglePause();

    }
    private void Update()
    {
        MovementInput();
        ShootInput();
        UIInput();
    }
}
