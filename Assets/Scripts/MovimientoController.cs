using UnityEngine;

public class MovimientoController : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    private Vector2 movimiento = Vector2.down;
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

    [Header("Estados Alterados")]
    public bool tieneVirus = false; 
    public bool tieneEscudo = false;

    // --- VARIABLES DE LA TORTUGA (De tu compañero) ---
    private float velocidadGuardada; 
    private bool esTortuga = false;  
    public float velocidadTortuga = 2f; 
    public float duracionTortuga = 8f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        renderAnimacionActual = renderAnimacionAbajo;
    }

    private void Update()
    {
        // Mantenemos TU lógica de Virus (Inversión)
        Vector2 arriba = tieneVirus ? Vector2.down : Vector2.up;
        Vector2 abajo = tieneVirus ? Vector2.up : Vector2.down;
        Vector2 izquierda = tieneVirus ? Vector2.right : Vector2.left;
        Vector2 derecha = tieneVirus ? Vector2.left : Vector2.right;

        if (Input.GetKey(teclaArriba))
        {
            Direccion(arriba, tieneVirus ? renderAnimacionAbajo : renderAnimacionArriba);
        }
        else if (Input.GetKey(teclaAbajo))
        {
            Direccion(abajo, tieneVirus ? renderAnimacionArriba : renderAnimacionAbajo);
        }
        else if (Input.GetKey(teclaIzquierda))
        {
            Direccion(izquierda, tieneVirus ? renderAnimacionDerecha : renderAnimacionIzquierda);
        }
        else if (Input.GetKey(teclaDerecha))
        {
            Direccion(derecha, tieneVirus ? renderAnimacionIzquierda : renderAnimacionDerecha);
        }
        else
        {
            Direccion(Vector2.zero, renderAnimacionActual);
        }
    }

    private void FixedUpdate()
    {
        Vector2 posicion = rb.position;
        rb.MovePosition(posicion + movimiento * velocidad * Time.fixedDeltaTime);
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
    
    private void Muerte()
    {
        // Mantenemos TU lógica de Escudo
        if (tieneEscudo)
        {
            tieneEscudo = false;
            // Regresar a blanco en todas las animaciones
            renderAnimacionArriba.CambiarColor(Color.white);
            renderAnimacionAbajo.CambiarColor(Color.white);
            renderAnimacionIzquierda.CambiarColor(Color.white);
            renderAnimacionDerecha.CambiarColor(Color.white);
            return; 
        }

        // Muerte normal con aviso al GameManager
        enabled = false;
        if (GetComponent<Bomba_Controller>() != null)
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
        gameObject.SetActive(false);
        GameManager gm = FindAnyObjectByType<GameManager>();
        if (gm != null) gm.Ganador();
    }

    // --- MÉTODOS DE POWER-UPS ---

    public void ActivarVirus()
    {
        if (!tieneVirus)
        {
            tieneVirus = true;
            Invoke(nameof(DesactivarVirus), 10f);
        }
    }

    private void DesactivarVirus()
    {
        tieneVirus = false;
    }

    public void ActivarEscudo()
    {
        if (!tieneEscudo)
        {
            tieneEscudo = true;
            // Color Cyan
            renderAnimacionArriba.CambiarColor(Color.cyan);
            renderAnimacionAbajo.CambiarColor(Color.cyan);
            renderAnimacionIzquierda.CambiarColor(Color.cyan);
            renderAnimacionDerecha.CambiarColor(Color.cyan);
        }
    }

    // --- LÓGICA DE LA TORTUGA (Adaptada de tu compañero) ---
    public void ActivarTortuga()
    {
        // Si ya eres tortuga, reiniciamos el tiempo
        if (esTortuga)
        {
            CancelInvoke(nameof(DesactivarTortuga));
            Invoke(nameof(DesactivarTortuga), duracionTortuga);
            return;
        }

        // Guardamos velocidad
        velocidadGuardada = velocidad; 
        esTortuga = true;
        velocidad = velocidadTortuga;

        // CAMBIO IMPORTANTE: Usamos tu sistema de renderAnimacion, no SpriteRenderer directo
        Color colorVerde = new Color(0.2f, 0.8f, 0.2f);
        renderAnimacionArriba.CambiarColor(colorVerde);
        renderAnimacionAbajo.CambiarColor(colorVerde);
        renderAnimacionIzquierda.CambiarColor(colorVerde);
        renderAnimacionDerecha.CambiarColor(colorVerde);

        Invoke(nameof(DesactivarTortuga), duracionTortuga);
    }

    private void DesactivarTortuga()
    {
        if (!esTortuga) return;

        // Si tenías mejoras de velocidad, intentamos respetarlas, 
        // pero si ganaste velocidad siendo tortuga, hay que tener cuidado.
        // Por simplicidad, restauramos la guardada o una base.
        velocidad = velocidadGuardada; 
        esTortuga = false;

        // Restaurar color blanco
        renderAnimacionArriba.CambiarColor(Color.white);
        renderAnimacionAbajo.CambiarColor(Color.white);
        renderAnimacionIzquierda.CambiarColor(Color.white);
        renderAnimacionDerecha.CambiarColor(Color.white);
    }
}