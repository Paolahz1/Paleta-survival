using UnityEngine;
using BreakingCat_Project.Assets.Scripts.Domain.Model;
using TMPro;

public class PoliceController : MonoBehaviour
{
    public float velocidad = 2f;
    public Vector3 puntoCentral;
    public Transform posicionJugador;
    public LayerMask layerJugador;
    public MaquinaEstados estadoActual;
    public float radio = 0.56f; // Radio rojo de ataque
    public float distanciaMaxima = 3.33f; // Radio azul de detección/persecución
    public float distanciaMaximaDelCentro = 15f; // Distancia máxima que puede alejarse del punto central
    public Transform jugadorTransform;

    // Variables para movimiento en grid
    public float gridSize = 1f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isLerpingToTarget = false; // Control de movimiento

    // Variables para patrullaje con sistema de mapa
    [Header("Sistema de Mapa")]
    public int cantidadManzanasX = 5;
    private Mapa mapa;
    private Vector2[] esquinasPatrullaje; // Las 3 esquinas por las que patrullar
    private int indiceEsquinaActual = 0;
    private float waitTime = 2f;
    private float waitCounter;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicializar el sistema de mapa
        mapa = new Mapa(SceneInfo.cantidadManzanasX, SceneInfo.cantidadManzanasY);

        // Generar las 3 esquinas de patrullaje aleatorias
        GenerarNuevasEsquinasPatrullaje();

        // Posicionar al policía en la primera esquina (superior izquierda aleatoria)
        if (esquinasPatrullaje != null && esquinasPatrullaje.Length > 0)
        {
            Vector3 posicionInicial = new Vector3(esquinasPatrullaje[0].x, esquinasPatrullaje[0].y, transform.position.z);
            transform.position = SnapToGrid(posicionInicial); // Asegurar alineación a cuadrícula
            puntoCentral = transform.position;
            targetPosition = transform.position;
            indiceEsquinaActual = 0;
        }
        else
        {
            puntoCentral = SnapToGrid(transform.position);
            transform.position = puntoCentral;
            targetPosition = transform.position;
        }

        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Asegurar que comience en estado de patrulla
        estadoActual = MaquinaEstados.Patrulla;
        waitCounter = waitTime;
    }

    void OnDrawGizmos()
    {
        // Radio rojo: área de ataque (se mueve con el policía)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);

        // Radio azul: área de detección/persecución (se mueve con el policía)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanciaMaxima);

        // Mostrar las esquinas de patrullaje
        if (esquinasPatrullaje != null && esquinasPatrullaje.Length > 0)
        {
            for (int i = 0; i < esquinasPatrullaje.Length; i++)
            {
                // Esquina actual
                Gizmos.color = (i == indiceEsquinaActual) ? Color.magenta : Color.cyan;
                Gizmos.DrawWireSphere(esquinasPatrullaje[i], 1f);

                // Dibujar líneas conectando las esquinas
                if (i < esquinasPatrullaje.Length - 1)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(esquinasPatrullaje[i], esquinasPatrullaje[i + 1]);
                }
                else
                {
                    // Conectar la última esquina con la primera
                    Gizmos.DrawLine(esquinasPatrullaje[i], esquinasPatrullaje[0]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // mostrar estado actual cada 5 segundos
        if (Time.time % 5 < 0.1f)
        {
            // Debug.Log($"Estado actual: {estadoActual}, Posición: {transform.position}, Target: {targetPosition}");
        }

        switch (estadoActual)
        {
            case MaquinaEstados.Patrulla:
                Patrullar();
                break;
            case MaquinaEstados.Persecucion:
                Perseguir();
                break;
            case MaquinaEstados.Ataque:
                Perseguir();
                break;
            case MaquinaEstados.Regresando:
                Regresar();
                break;
        }

        // Mover hacia la posición objetivo en grid
        MoveToTarget();
    }

    void Patrullar()
    {
        // Verificar primero si hay jugador en radio rojo (ataque directo)
        Collider2D jugadorEnRadioRojo = Physics2D.OverlapCircle(transform.position, radio, layerJugador);
        if (jugadorEnRadioRojo != null && jugadorEnRadioRojo.CompareTag("Player"))
        {
            // Debug.Log("¡Jugador detectado en radio rojo! Cambiando directo a ATAQUE");
            estadoActual = MaquinaEstados.Ataque;
            posicionJugador = jugadorEnRadioRojo.transform;
            return;
        }

        // Verificar jugador en radio azul (persecución)
        Collider2D jugadorEnRadioAzul = Physics2D.OverlapCircle(transform.position, distanciaMaxima, layerJugador);
        if (jugadorEnRadioAzul != null && jugadorEnRadioAzul.CompareTag("Player"))
        {
            // Debug.Log("¡Jugador detectado en radio azul! Cambiando a Persecución");
            estadoActual = MaquinaEstados.Persecucion;
            posicionJugador = jugadorEnRadioAzul.transform;
            return;
        }

        // Por si acaso
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, distanciaMaxima);
        // Debug.Log($"Objetos cerca del policía (círculo azul): {nearbyObjects.Length}");

        foreach (Collider2D obj in nearbyObjects)
        {
            if (obj.CompareTag("Player"))
            {
                float distanciaAlJugador = Vector3.Distance(transform.position, obj.transform.position);

                if (distanciaAlJugador <= radio)
                {
                    // Debug.Log("¡Jugador encontrado por TAG en radio rojo!");
                    estadoActual = MaquinaEstados.Ataque;
                    posicionJugador = obj.transform;
                    return;
                }
                else if (distanciaAlJugador <= distanciaMaxima)
                {
                    // Debug.Log("¡Jugador encontrado por TAG en radio azul!");
                    estadoActual = MaquinaEstados.Persecucion;
                    posicionJugador = obj.transform;
                    return;
                }
                break;
            }
        }

        // Si no se detectó jugador, continuar patrullaje normal
        // Debug.Log("No se detectó jugador en ningún radio. Continuando patrullaje entre esquinas...");
        // Patrullaje entre las 3 esquinas
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f && !isMoving)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                SetNextPatrolTarget();
                waitCounter = waitTime;
            }
        }
    }

    void Perseguir()
    {

        if (posicionJugador != null)
        {
            float distanciaAlJugador = Vector3.Distance(transform.position, posicionJugador.position);

            // Debug.Log($"Persiguiendo jugador... Distancia: {distanciaAlJugador:F2} (Radio rojo: {radio}, Radio azul: {distanciaMaxima})");

            // Si el jugador está en el radio rojo, cambiar a ataque
            if (distanciaAlJugador <= radio)
            {
                // Debug.Log("¡Jugador en radio rojo! Cambiando a ATAQUE");
                estadoActual = MaquinaEstados.Ataque;
                return;
            }

            // Si el jugador sale del radio azul, dejar de perseguir
            if (distanciaAlJugador > distanciaMaxima)
            {
                // Debug.Log("Jugador salió del radio azul. Dejando de perseguir...");
                posicionJugador = null;

                // Decidir si regresar al punto inicial o continuar patrullando
                if (EstaFueraDeSusEsquinas())
                {
                    // Debug.Log("Fuera de esquinas, regresando al punto inicial...");
                    estadoActual = MaquinaEstados.Regresando;
                }
                else
                {
                    // Debug.Log("Cerca de esquinas, continuando patrullaje...");
                    estadoActual = MaquinaEstados.Patrulla;
                }
                return;
            }

            targetPosition = SnapToGrid(posicionJugador.position);

            // Validar que el target esté dentro del mapa
            if (EsPosicionDentroDelMapa(targetPosition))
            {
                MoveToTarget(); // Usar el mismo método que en patrullaje
            }
            else
            {
                // Debug.Log("Jugador fuera de los límites del mapa");
            }
        }
        else
        {
            // Si no hay jugador, decidir qué hacer
            // Debug.Log("No hay jugador para perseguir...");

            if (EstaFueraDeSusEsquinas())
            {
                // Debug.Log("Regresando al punto inicial...");
                estadoActual = MaquinaEstados.Regresando;
            }
            else
            {
                // Debug.Log("Continuando patrullaje...");
                estadoActual = MaquinaEstados.Patrulla;
            }
        }
    }

    void Atacar()
    {
        // Debug.Log("¡ATACANDO AL JUGADOR!");

        // Verificar si el jugador sigue siendo detectado por algún radio
        Collider2D jugadorEnRadioAzul = Physics2D.OverlapCircle(transform.position, distanciaMaxima, layerJugador);
        Collider2D jugadorEnRadioRojo = Physics2D.OverlapCircle(transform.position, radio, layerJugador);

        if (jugadorEnRadioRojo != null)
        {
            // Jugador aún en radio rojo, continuar atacando
            // Debug.Log("Jugador en radio rojo, continuando ataque...");
            posicionJugador = jugadorEnRadioRojo.transform;
        }
        else if (jugadorEnRadioAzul != null)
        {
            // Jugador salió del radio rojo pero sigue en azul, cambiar a persecución
            // Debug.Log("Jugador salió del radio rojo pero sigue en el azul. Cambiando a persecución...");
            posicionJugador = jugadorEnRadioAzul.transform;
            estadoActual = MaquinaEstados.Persecucion;
            return;
        }
        else
        {
            // Jugador no detectado en ningún radio
            // Debug.Log("Jugador no detectado en ningún radio desde ataque...");
            posicionJugador = null;

            if (EstaFueraDeSusEsquinas())
            {
                // Debug.Log("Regresando al punto inicial desde ataque...");
                estadoActual = MaquinaEstados.Regresando;
            }
            else
            {
                // Debug.Log("Continuando patrullaje desde ataque...");
                estadoActual = MaquinaEstados.Patrulla;
            }
            return;
        }

        // Lógica de ataque continuo
        waitCounter -= Time.deltaTime;
        if (waitCounter <= 0)
        {
            // Debug.Log("Tiempo de ataque agotado...");
            posicionJugador = null;

            if (EstaFueraDeSusEsquinas())
            {
                // Debug.Log("Regresando al punto inicial tras timeout...");
                estadoActual = MaquinaEstados.Regresando;
            }
            else
            {
                // Debug.Log("Continuando patrullaje tras timeout...");
                estadoActual = MaquinaEstados.Patrulla;
            }
            waitCounter = waitTime;
        }
    }

    void Regresar()
    {

        // El objetivo es regresar al punto inicial de patrullaje (primera esquina)
        Vector3 puntoInicialPatrullaje = Vector3.zero;

        if (esquinasPatrullaje != null && esquinasPatrullaje.Length > 0)
        {
            puntoInicialPatrullaje = new Vector3(esquinasPatrullaje[0].x, esquinasPatrullaje[0].y, transform.position.z);
        }
        else
        {
            puntoInicialPatrullaje = puntoCentral;
        }

        // Usar el mismo sistema de movimiento que en patrullaje
        targetPosition = SnapToGrid(puntoInicialPatrullaje);

        // Validar que el target esté dentro del mapa
        if (EsPosicionDentroDelMapa(targetPosition))
        {
            MoveToTarget(); // Usar el mismo método que en patrullaje
        }

        // Verificar si ya llegó al punto inicial
        if (Vector3.Distance(transform.position, puntoInicialPatrullaje) < 0.5f)
        {
            // Debug.Log("Regresó al punto inicial, reanudando patrullaje");
            estadoActual = MaquinaEstados.Patrulla;
            indiceEsquinaActual = 0; // Reiniciar desde la primera esquina
            waitCounter = waitTime; // Reiniciar el contador de espera
        }
    }

    void MoveToTarget()
    {
        // Solo usar movimiento discreto para patrullaje
        if (!isLerpingToTarget && Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Iniciar movimiento hacia el siguiente punto de la cuadrícula
            Vector3 currentGridPos = SnapToGrid(transform.position);
            Vector3 targetGridPos = SnapToGrid(targetPosition);

            // Determinar el siguiente paso en la cuadrícula
            Vector3 nextGridStep = GetNextGridStep(currentGridPos, targetGridPos);

            if (nextGridStep != currentGridPos)
            {
                isLerpingToTarget = true;
                isMoving = true;
                StartCoroutine(MoveToGridPosition(nextGridStep));
            }
        }

        if (!isMoving && animator != null)
        {
            animator.SetInteger("Direction", 0); // Idle
        }
    }

    // Obtiene el siguiente paso en la cuadrícula hacia el objetivo
    Vector3 GetNextGridStep(Vector3 current, Vector3 target)
    {
        Vector3 direction = target - current;

        // Priorizar movimiento horizontal o vertical 
        if (Mathf.Abs(direction.x) > 0.1f)
        {
            // Moverse horizontalmente
            float stepX = Mathf.Sign(direction.x) * gridSize;
            return new Vector3(current.x + stepX, current.y, current.z);
        }
        else if (Mathf.Abs(direction.y) > 0.1f)
        {
            // Moverse verticalmente
            float stepY = Mathf.Sign(direction.y) * gridSize;
            return new Vector3(current.x, current.y + stepY, current.z);
        }

        return current; // Ya está en el objetivo
    }

    // Mover el policía de manera discreta de una casilla a otra
    System.Collections.IEnumerator MoveToGridPosition(Vector3 targetGridPos)
    {
        Vector3 startPos = transform.position;
        Vector3 direction = (targetGridPos - startPos).normalized;
        float journey = 0f;
        float gridMoveSpeed = velocidad * 2f; // Hacer el movimiento de grid un poco más rápido

        // Actualizar animación
        UpdateAnimation(direction);

        while (journey <= 1f)
        {
            journey += Time.deltaTime * gridMoveSpeed;
            transform.position = Vector3.Lerp(startPos, targetGridPos, journey);
            yield return null;
        }

        // Asegurar posición exacta
        transform.position = targetGridPos;
        isLerpingToTarget = false;
        isMoving = false;
    }

    void UpdateAnimation(Vector3 direction)
    {
        if (animator == null || spriteRenderer == null) return;

        if (direction.x > 0.1f) // Derecha
        {
            animator.SetInteger("Direction", 4);
            spriteRenderer.flipX = false;
        }
        else if (direction.x < -0.1f) // Izquierda
        {
            animator.SetInteger("Direction", 4);
            spriteRenderer.flipX = true;
        }
        else if (direction.y > 0.1f) // Arriba
        {
            animator.SetInteger("Direction", 1);
        }
        else if (direction.y < -0.1f) // Abajo
        {
            animator.SetInteger("Direction", 2);
        }
    }

    // Verifica si el policía está fuera de sus esquinas de patrullaje
    bool EstaFueraDeSusEsquinas()
    {
        if (esquinasPatrullaje == null || esquinasPatrullaje.Length == 0)
            return true;

        float distanciaMinima = float.MaxValue;
        Vector3 posicionActual = transform.position;

        // Calcular la distancia a cada esquina de patrullaje
        foreach (Vector2 esquina in esquinasPatrullaje)
        {
            float distancia = Vector3.Distance(posicionActual, new Vector3(esquina.x, esquina.y, posicionActual.z));
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
            }
        }

        // Considerar que está "fuera" si está a más de 3 unidades de cualquier esquina
        float umbralDistancia = 3f;
        bool estaFuera = distanciaMinima > umbralDistancia;

        if (estaFuera)
        {
            // Debug.Log($"Policía fuera de esquinas. Distancia mínima: {distanciaMinima:F2}");
        }

        return estaFuera;
    }

    // Verifica si una posición está dentro de los límites del mapa
    bool EsPosicionDentroDelMapa(Vector3 posicion)
    {
        if (mapa != null)
        {
            Vector2[] limitesMapa = mapa.ObtenerLimitesMapa();
            if (limitesMapa != null && limitesMapa.Length == 4)
            {
                // limitesMapa[0] = superior izquierdo, limitesMapa[1] = superior derecho
                // limitesMapa[2] = inferior derecho, limitesMapa[3] = inferior izquierdo
                return posicion.x >= limitesMapa[3].x && // Límite izquierdo
                       posicion.x <= limitesMapa[1].x && // Límite derecho  
                       posicion.y >= limitesMapa[3].y && // Límite inferior
                       posicion.y <= limitesMapa[0].y;   // Límite superior
            }
        }

        // Si no hay mapa, permitir movimiento
        return true;
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector3(snappedX, snappedY, position.z);
    }

    Vector3 GetCardinalDirection(Vector3 from, Vector3 to)
    {
        Vector3 direction = to - from;

        // Determinar si moverse más en X o Y
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Moverse horizontalmente
            return new Vector3(Mathf.Sign(direction.x) * gridSize, 0, 0) + from;
        }
        else
        {
            // Moverse verticalmente
            return new Vector3(0, Mathf.Sign(direction.y) * gridSize, 0) + from;
        }
    }
    // Genera nuevas esquinas de patrullaje aleatorias usando el sistema de mapa
    void GenerarNuevasEsquinasPatrullaje()
    {
        if (mapa != null)
        {
            esquinasPatrullaje = mapa.GenerarPatrullajeAleatorioCardinal();
            indiceEsquinaActual = 0;
        }
        else
        {
            Debug.LogWarning("Mapa no inicializado, usando esquinas por defecto");
            // Esquinas por defecto cerca del punto central
            esquinasPatrullaje = new Vector2[]
            {
                new Vector2(transform.position.x, transform.position.y),
                new Vector2(transform.position.x, transform.position.y - 8.57f),
                new Vector2(transform.position.x + 13.5f, transform.position.y - 8.57f)
            };
            indiceEsquinaActual = 0;
        }
    }

    // Establece la siguiente esquina de patrullaje como objetivo
    void SetNextPatrolTarget()
    {
        if (esquinasPatrullaje == null || esquinasPatrullaje.Length == 0)
        {
            GenerarNuevasEsquinasPatrullaje();
            return;
        }

        // No cambiar objetivo si está en movimiento
        if (isLerpingToTarget) return;

        // Moverse a la siguiente esquina
        indiceEsquinaActual = (indiceEsquinaActual + 1) % esquinasPatrullaje.Length;

        // Asegurar que el objetivo esté alineado a la cuadrícula
        Vector3 newTarget = new Vector3(esquinasPatrullaje[indiceEsquinaActual].x, esquinasPatrullaje[indiceEsquinaActual].y, transform.position.z);
        targetPosition = SnapToGrid(newTarget);

        // Debug.Log($"Nuevo objetivo de patrulla: Esquina {indiceEsquinaActual} en {targetPosition}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInteraction playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
            if (playerInteraction == null || playerInteraction.gatoPrincipal == null)
            {
                Debug.LogWarning("PlayerInteraction o gatoPrincipal es null en el jugador.");
                return;
            }

            playerInteraction.gatoPrincipal.PerderVida();

            if (playerInteraction.gatoPrincipal.vida <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                playerInteraction.gatoPrincipal.vida = 3;
            }

            UpdateText(playerInteraction);

            CatPlayerController catPlayerController = other.gameObject.GetComponent<CatPlayerController>();
            if (catPlayerController != null)
            {
                catPlayerController.ActivatePower();
                catPlayerController.catBoltTimer = 1f;
            }
        }
    }

    private void UpdateText(PlayerInteraction playerInteraction)
    {
        GameObject vidaObj = GameObject.Find("Vida");
        if (vidaObj != null)
        {
            TextMeshProUGUI vidaText = vidaObj.GetComponent<TextMeshProUGUI>();
            if (vidaText != null)
            {
                vidaText.text = playerInteraction.gatoPrincipal.vida.ToString();
            }
        }
    }
}
