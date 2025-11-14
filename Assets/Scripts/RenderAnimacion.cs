using UnityEngine;

public class RenderAnimacion : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float velocidadAnimacion = 0.25f;
    private int frameActual = 0;
    public Sprite[] framesAnimacion;
    public Sprite frameEstatico;

    public bool animacionActiva = true;
    public bool usarFrameEstatico = true;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
       spriteRenderer.enabled = true;
    }
    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(SiguienteFrame), velocidadAnimacion, velocidadAnimacion);
    }
    
    private void SiguienteFrame()
    {
        frameActual++;
        if (animacionActiva && frameActual >= framesAnimacion.Length)
        {
            frameActual = 0;
        }
        if (frameActual >= 0 && frameActual < framesAnimacion.Length)
        {
            spriteRenderer.sprite = framesAnimacion[frameActual];
        }
        else if (usarFrameEstatico)
        {
            spriteRenderer.sprite = frameEstatico;
        }
    }
}
