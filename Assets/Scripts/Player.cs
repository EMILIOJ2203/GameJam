using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Si el tiempo está pausado (Modo Máscara), no hacemos nada
        if (Time.timeScale == 0) 
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Movimiento constante a la derecha
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
    }
}