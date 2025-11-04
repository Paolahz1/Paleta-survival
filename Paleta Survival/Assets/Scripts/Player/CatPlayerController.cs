using UnityEngine;
using UnityEngine.UI;

public class CatPlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;

    // Configuración del poder
    public float catBoltBoost = 2f;        // Multiplicador de velocidad
    public float catBoltDuration = 5f;     // Duración en segundos
    public float catBoltCooldown = 30f;    // Tiempo entre usos

    private bool isCatBolt = false;
    private bool canUseCatBolt = true;

    public float catBoltTimer = 0f;
    private float catBoltCooldownTimer = 0f;

    public Text catBoltText; 

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (catBoltText != null)
            catBoltText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Movimiento básico
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", Mathf.Abs(movement.x));
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Flip del sprite
        if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);

        // Activar poder con tecla R
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isCatBolt && canUseCatBolt)
            {
                ActivatePower();
            }
            else if (!canUseCatBolt && !isCatBolt)
            {
                // Mostrar mensaje solo si se intenta usar en cooldown
                if (catBoltText != null)
                {
                    catBoltText.text = "Faltan " + catBoltCooldownTimer.ToString("F1") + "s";
                    catBoltText.gameObject.SetActive(true);
                    // Ocultar mensaje después de un corto tiempo
                    CancelInvoke(nameof(HideText));
                    Invoke(nameof(HideText), 1.5f);
                }
            }
        }

        // Control del poder activo
        if (isCatBolt)
        {
            catBoltTimer -= Time.deltaTime;
            if (catBoltTimer <= 0)
            {
                DeactivatePower();
            }
        }

        // Control del cooldown
        if (!isCatBolt && !canUseCatBolt)
        {
            catBoltCooldownTimer -= Time.deltaTime;
            if (catBoltCooldownTimer <= 0f)
            {
                canUseCatBolt = true;
                catBoltCooldownTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void ActivatePower()
    {
        isCatBolt = true;
        canUseCatBolt = false;
        moveSpeed *= catBoltBoost;
        catBoltTimer = catBoltDuration;

        if (catBoltText != null)
        {
            catBoltText.text = "RUN!";
            catBoltText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideText));
            Invoke(nameof(HideText), 1.5f); // Oculta texto tras 1.5s
        }
    }

    void DeactivatePower()
    {
        isCatBolt = false;
        moveSpeed = 2f;

        // Iniciar cooldown
        catBoltCooldownTimer = catBoltCooldown;

        // No mostrar ningún texto aquí 
        if (catBoltText != null)
            catBoltText.gameObject.SetActive(false);
    }

    void HideText()
    {
        if (catBoltText != null)
            catBoltText.gameObject.SetActive(false);
    }
}
