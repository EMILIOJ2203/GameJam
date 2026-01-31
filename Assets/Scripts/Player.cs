using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float speed = 5f;
    public float jumpForce = 8f;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        // 1. Capturar entrada (Horizontal y Salto)
        // Se hace en Update para que sea instantáneo
        moveInput = Input.GetAxisRaw("Horizontal"); 

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0) 
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 2. Aplicar movimiento físico
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }
}