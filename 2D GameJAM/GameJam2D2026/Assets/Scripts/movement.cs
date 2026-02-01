using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Ajustes")]
    public float velocidad = 3f;
    public float fuerzaSalto = 4f; 

    public AudioSource audioCorrer;

    [Header("Referencias")]
    public Transform pies;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private float inputHorizontal;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1. INPUT
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        // 2. GIRO DEL PERSONAJE (Flip)
        if (inputHorizontal > 0) 
            spriteRenderer.flipX = false; // Mirar derecha
        else if (inputHorizontal < 0)
            spriteRenderer.flipX = true;  // Mirar izquierda

        // 3. LÓGICA DE ANIMACIÓN
        
        // PARAMETRO 1: VELOCIDAD (Idle vs Running)
        // Enviamos siempre valor positivo (0 si está quieto, 1 si se mueve)
        animator.SetFloat("Velocidad", Mathf.Abs(inputHorizontal));

        // PARAMETRO 2: SUBIENDO (Running vs Climbing)
        // Para estar "trepando/subiendo" deben cumplirse 3 cosas:
        // A. Estar tocando el suelo (para que no se active al saltar).
        // B. Tener velocidad vertical positiva (Y > 0.1) -> Significa que va hacia arriba.
        // C. Moverse horizontalmente.
        bool estaSubiendo = enSuelo && rb.linearVelocity.y > 0.1f && Mathf.Abs(inputHorizontal) > 0;
        
        animator.SetBool("Subiendo", estaSubiendo);

        bool seEstaMoviendo = Mathf.Abs(inputHorizontal) > 0.1f;

        if (seEstaMoviendo && enSuelo)
        {
            // Solo le damos Play si NO está sonando ya para evitar tartamudeo
            if (!audioCorrer.isPlaying)
            {
                // Opcional: Variar un poco el tono para que no suene robótico
                audioCorrer.pitch = Random.Range(0.9f, 1.1f); 
                audioCorrer.Play();
            }
        }
        else
        {
            // Si saltamos o nos detenemos, el sonido para
            audioCorrer.Stop();
        }
        
        // 4. SALTO
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }
    }

    void FixedUpdate()
    {
        // Movimiento físico
        rb.linearVelocity = new Vector2(inputHorizontal * velocidad, rb.linearVelocity.y);
        
        // Detección de suelo
        enSuelo = Physics2D.OverlapCircle(pies.position, radioSuelo, capaSuelo);
    }
}