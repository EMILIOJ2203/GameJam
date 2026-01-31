using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Ajustes")]
    public float velocidad = 1.5f;
    public float fuerzaSalto = 4f;

    [Header("Referencias")]
    public Transform pies; // Un objeto vacío en los pies del personaje
    public float radioSuelo = 0.2f; // Tamaño del detector de suelo
    public LayerMask capaSuelo; // Qué capas cuentan como suelo

    private Rigidbody2D rb;
    private float inputHorizontal;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() // Inputs siempre aquí
    {
        // 1. Detectar input izquierda/derecha (A/D o Flechas)
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        // 2. Saltar solo si toca suelo
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }
    }

    void FixedUpdate() // Físicas siempre aquí
    {
        // 3. Mover al personaje
        rb.linearVelocity = new Vector2(inputHorizontal * velocidad, rb.linearVelocity.y);

        // 4. Detectar si estamos tocando el suelo
        // Crea un círculo invisible en los pies para ver si toca la capa "Suelo"
        enSuelo = Physics2D.OverlapCircle(pies.position, radioSuelo, capaSuelo);
    }
}