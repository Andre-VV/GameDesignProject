using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    //player speed
    public float speed = 5f;

    //rigid body object
    private Rigidbody2D rb;

    //stores movement vector
    private Vector2 moveDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //assign a rigidbody to the object
        rb = GetComponent<Rigidbody2D>();
        
    }

    public void OnMove(InputValue value)
    {
            moveDirection = value.Get<Vector2>();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }




}
