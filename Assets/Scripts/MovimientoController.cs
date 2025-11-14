using UnityEngine;

public class MovimientoController : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    private Vector2 movimiento = Vector2.down;
    public float velocidad = 5f;

    public KeyCode teclaArriba = KeyCode.W;
    public KeyCode teclaAbajo = KeyCode.S;
    public KeyCode teclaIzquierda = KeyCode.A;
    public KeyCode teclaDerecha = KeyCode.D;

    public RenderAnimacion renderAnimacionArriba;
    public RenderAnimacion renderAnimacionAbajo;
    public RenderAnimacion renderAnimacionIzquierda;
    public RenderAnimacion renderAnimacionDerecha;

    public RenderAnimacion renderAnimacionMuerte;
    private RenderAnimacion renderAnimacionActual;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        renderAnimacionActual = renderAnimacionAbajo;
    }

    private void Update()
    {
        if (Input.GetKey(teclaArriba))
        {
            Direccion(Vector2.up, renderAnimacionArriba);
        }
        else if (Input.GetKey(teclaAbajo))
        {
            Direccion(Vector2.down, renderAnimacionAbajo);
        }
        else if (Input.GetKey(teclaIzquierda))
        {
            Direccion(Vector2.left, renderAnimacionIzquierda);
        }
        else if (Input.GetKey(teclaDerecha))
        {
            Direccion(Vector2.right, renderAnimacionDerecha);
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
        gameObject.SetActive(false);
        FindAnyObjectByType<GameManager>().Ganador();
    }
    

}
