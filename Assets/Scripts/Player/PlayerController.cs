using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire!");
    }

    public void MovementChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 move = context.ReadValue<Vector2>();
            Debug.Log("Move: " + move);
        }
        else if (context.canceled)
        {
            Debug.Log("Move: Vector2.zero");
        }
    }


}
