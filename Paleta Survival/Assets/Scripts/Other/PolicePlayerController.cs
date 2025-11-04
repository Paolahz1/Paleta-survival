using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Animator animator;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Captura de entradas
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalizar vector (evita diagonales más rápidas)
        if (movement.sqrMagnitude > 1)
            movement = movement.normalized;

        // --- Animaciones ---
        if (movement.x > 0f) // Derecha
        {
            animator.SetInteger("Direction", 4); // WalkSide
            spriteRenderer.flipX = false;
        }
        else if (movement.x < 0f) // Izquierda (reusa WalkRight + flipX)
        {
            animator.SetInteger("Direction", 4);
            spriteRenderer.flipX = true;
        }
        else if (movement.y > 0f) // Arriba
        {
            animator.SetInteger("Direction", 1); // WalkUp
        }
        else if (movement.y < 0f) // Abajo
        {
            animator.SetInteger("Direction", 2); // WalkDown
        }
        else
        {
            animator.SetInteger("Direction", 0); // Idle
        }
    }

    void FixedUpdate()
    {
        // Movimiento real
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
    }
}
