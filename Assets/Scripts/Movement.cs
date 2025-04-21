using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Animator animator;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float maxJumpTime = 0.2f; 

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float moveDirection = 0f;

    public bool isJumping = false;
    private float jumpTimeCounter;

    public float coyoteTime = 0.06f;
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    public LayerMask groundLayer;
    public Collider2D groundCheck;

    public Collider2D checkRoof;

    [Header("Lateral Collision")]
    public Collider2D wallCheck_R;
    public Collider2D wallCheck_L;
    public LayerMask wallLayer;

    [Header("Attack")]
    private bool atacking;
    public GameObject attackHitbox;
    public float attackDuration = 0.3f;
    public Transform swordHolder;

    [Header("Stats")]
    public float health;

    [Header("Sounds")]
    public SFX_Manager sfx;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movimiento lateral
        float sensibility = 0.2f; // sensibilidad mínima
        float rawInput = Input.GetAxis("Horizontal");

        if (rawInput > sensibility)
        {
            moveDirection = 1f;
        }
        else if (rawInput < -sensibility)
        {
            moveDirection = -1f;
        }
        else
        {
            moveDirection = 0f;
        }

        if(moveDirection!=0)
        {
            Flip(moveDirection);
        }

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
        if (Input.GetKey(KeyCode.JoystickButton1) && isJumping && !IsRoof())
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
        else
            isJumping = false;

        // Soltar salto
        if (Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            isJumping = false;
        }

        //Atack
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            StartCoroutine(Attack());
        }

    }

    void FixedUpdate()
    {
        float targetVelocityX = moveDirection * moveSpeed;

        if (!IsGrounded() && ((IsTouchingWall_R() && moveDirection > 0) || (IsTouchingWall_L() && moveDirection < 0)))
        {
            // Bloquea solo si intenta moverse hacia la pared
            targetVelocityX = 0;
        }

        rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);


        if (rb.velocity.x > 0f)
        {
            sr.flipX = true;
        }
        if (rb.velocity.x < 0f)
        {
            sr.flipX = false;
        }

        animator.SetFloat("movement", rb.velocity.x);
        animator.SetFloat("falling", rb.velocity.y);
        animator.SetBool("ground", IsGrounded());
    }

    IEnumerator Attack()
    {
        atacking = true;
        animator.SetTrigger("attack");
        attackHitbox.GetComponent<Sword>().EnableCollider();

        yield return new WaitForSeconds(0.1f); // Espera 2 segundos
        attackHitbox.GetComponent<Sword>().DisableCollider();

        atacking = false;
    }

    public void TakeDamage(float damage)
    {
        health = health - damage;
    }

    void Flip(float direction)
    {   
        Vector3 holderScale = swordHolder.localScale;
        holderScale.x = Mathf.Abs(holderScale.x) * direction;
        swordHolder.localScale = holderScale;
    }


    public void PlayGroundSound()
    {
        sfx.Play_ground_sound();
    }


    // Detectar si está en el suelo
    bool IsGrounded()
    {
       return groundCheck.IsTouchingLayers(groundLayer);
    }
    
    bool IsRoof()
    {
       return checkRoof.IsTouchingLayers(groundLayer);
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