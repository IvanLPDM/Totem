using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float maxJumpTime = 0.2f; 

    private Rigidbody2D rb;
    private float moveDirection = 0f;

    public bool isJumping = false;
    private float jumpTimeCounter;

    public float coyoteTime = 0.06f;
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    public LayerMask groundLayer;
    public Collider2D groundCheck;

    [Header("Lateral Collision")]
    public Collider2D wallCheck_R;
    public Collider2D wallCheck_L;
    public LayerMask wallLayer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimiento lateral
        moveDirection = Input.GetAxisRaw("Horizontal");

        // Coyote Time
        if (IsGrounded())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Jump Buffer (solo botón X del mando)
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Iniciar salto
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f; // Consumimos el buffer
        }

        // Mantener salto (mientras se mantenga presionado el botón X)
        if (Input.GetKey(KeyCode.JoystickButton1) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // Soltar salto
        if (Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            isJumping = false;
        }


    }

    void FixedUpdate()
    {
        float targetVelocityX = moveDirection * moveSpeed;

        if ((IsTouchingWall_R() && moveDirection > 0) || (IsTouchingWall_L() && moveDirection < 0))
        {
            // Bloquea solo si intenta moverse hacia la pared
            targetVelocityX = 0;
        }

        rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);
    }

    // Detectar si está en el suelo
    bool IsGrounded()
    {
       return groundCheck.IsTouchingLayers(groundLayer);
        
    }

    bool IsTouchingWall_L()
    {
        return wallCheck_L.IsTouchingLayers(wallLayer);
    }

    bool IsTouchingWall_R()
    {
        return wallCheck_R.IsTouchingLayers(wallLayer);
    }
}