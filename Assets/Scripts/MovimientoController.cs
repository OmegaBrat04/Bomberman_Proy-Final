using UnityEngine;

public class MovimientoController : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    private Vector2 movimiento = Vector2.down;

    [Header("Estadísticas")]
    public float velocidad = 5f;

    [Header("Controles")]
    public KeyCode teclaArriba = KeyCode.W;
    public KeyCode teclaAbajo = KeyCode.S;
    public KeyCode teclaIzquierda = KeyCode.A;
    public KeyCode teclaDerecha = KeyCode.D;

    [Header("Animaciones")]
    public RenderAnimacion renderAnimacionArriba;
    public RenderAnimacion renderAnimacionAbajo;
    public RenderAnimacion renderAnimacionIzquierda;
    public RenderAnimacion renderAnimacionDerecha;
    public RenderAnimacion renderAnimacionMuerte;
    private RenderAnimacion renderAnimacionActual;

    // --- ESTADOS ALTERADOS (POWER UPS / DEBUFFS) ---
    private float velocidadOriginal; // Para restaurar velocidad después de tortuga/virus

    // Tortuga
    private bool esTortuga = false;
    public float velocidadTortuga = 2f;

    // Escudo
    public bool tieneEscudo = false;

    // Virus / Maldición
    private bool estaMaldito = false;
    private bool controlesInvertidos = false;
    public float duracionEfecto = 8f; // Duración general para efectos temporales
    // -----------------------------------------------

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        renderAnimacionActual = renderAnimacionAbajo;
    }

    private void Update()
    {
        KeyCode arriba = controlesInvertidos ? teclaAbajo : teclaArriba;
        KeyCode abajo = controlesInvertidos ? teclaArriba : teclaAbajo;
        KeyCode izq = controlesInvertidos ? teclaDerecha : teclaIzquierda;
        KeyCode der = controlesInvertidos ? teclaIzquierda : teclaDerecha;

        if (Input.GetKey(arriba)) Direccion(Vector2.up, renderAnimacionArriba);
        else if (Input.GetKey(abajo)) Direccion(Vector2.down, renderAnimacionAbajo);
        else if (Input.GetKey(izq)) Direccion(Vector2.left, renderAnimacionIzquierda);
        else if (Input.GetKey(der)) Direccion(Vector2.right, renderAnimacionDerecha);
        else Direccion(Vector2.zero, renderAnimacionActual);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimiento * velocidad * Time.fixedDeltaTime);
    }

    private void Direccion(Vector2 nuevaDireccion, RenderAnimacion renderAnimacion)
    {
        movimiento = nuevaDireccion;
        renderAnimacionArriba.enabled = renderAnimacion == renderAnimacionArriba;
        renderAnimacionAbajo.enabled = renderAnimacion == renderAnimacionAbajo;
        renderAnimacionIzquierda.enabled = renderAnimacion == renderAnimacionIzquierda;
        renderAnimacionDerecha.enabled = renderAnimacion == renderAnimacionDerecha;
        renderAnimacionActual = renderAnimacion;
        renderAnimacionActual.animacionActiva = movimiento != Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            Muerte();
        }
    }
    // --- GESTIÓN DE MUERTE Y ESCUDO ---
    private void Muerte()
    {
        // 1. Si tiene escudo, lo pierde pero NO muere
        if (tieneEscudo)
        {
            tieneEscudo = false;
            GetComponent<SpriteRenderer>().color = Color.white; // Quitamos color azul
            Debug.Log("¡El escudo te salvó!");
            return;
        }

        // 2. Si no tiene escudo, muere normalmente
        enabled = false;
        GetComponent<Bomba_Controller>().enabled = false;

        renderAnimacionArriba.enabled = false;
        renderAnimacionAbajo.enabled = false;
        renderAnimacionIzquierda.enabled = false;
        renderAnimacionDerecha.enabled = false;
        renderAnimacionMuerte.enabled = true;

        Invoke(nameof(DestruirJugador), 1f);
    }

    private void DestruirJugador()
    {
        var gm = GameManager.Instancia != null ? GameManager.Instancia : FindFirstObjectByType<GameManager>();
        if (gm != null) gm.UnregisterPlayer(gameObject);
        Destroy(gameObject);
    }

    // --- FUNCIONES DE EFECTOS (TORTUGA, ESCUDO, VIRUS) ---

    public void ActivarTortuga()
    {
        if (esTortuga) { ResetearTemporizador(nameof(DesactivarTortuga)); return; }

        velocidadOriginal = velocidad; // Guardar velocidad actual
        esTortuga = true;
        velocidad = velocidadTortuga;

        GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.8f, 0.2f); // Verde
        Invoke(nameof(DesactivarTortuga), duracionEfecto);
    }

    private void DesactivarTortuga()
    {
        if (!esTortuga) return;
        esTortuga = false;
        velocidad = velocidadOriginal; // Restaurar velocidad
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void ActivarEscudo()
    {
        tieneEscudo = true;
        GetComponent<SpriteRenderer>().color = Color.cyan; // Azul Cian
    }

    public void ActivarMaldicion() // Virus
    {
        if (estaMaldito) { ResetearTemporizador(nameof(DesactivarMaldicion)); return; }

        estaMaldito = true;
        GetComponent<SpriteRenderer>().color = Color.magenta; // Magenta/Morado

        // 50% probabilidad: O te hace lento, O invierte controles
        if (Random.value > 0.5f)
        {
            controlesInvertidos = true;
            Debug.Log("¡Virus: Controles Invertidos!");
        }
        else
        {
            velocidadOriginal = velocidad;
            velocidad = 1.5f; // Muy lento
            Debug.Log("¡Virus: Lentitud!");
        }

        Invoke(nameof(DesactivarMaldicion), duracionEfecto);
    }

    private void DesactivarMaldicion()
    {
        estaMaldito = false;
        controlesInvertidos = false;
        if (velocidad < 4f) velocidad = 5f; // Restaurar velocidad base si estaba lento
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void ResetearTemporizador(string metodo)
    {
        CancelInvoke(metodo);
        Invoke(metodo, duracionEfecto);
    }
}