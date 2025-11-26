// TU RenderAnimacion.cs (el que adjuntaste)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderAnimacion : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float velocidadAnimacion = 0.25f;
    public int frameActual { get; private set; }
    public Sprite[] framesAnimacion;
    public Sprite frameEstatico;

    public bool animacionActiva = true;
    public bool usarFrameEstatico = true; // Agregada si no la tenías

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

        // Si la animación está activa y llegamos al final de los frames, volvemos al inicio
        if (animacionActiva && frameActual >= framesAnimacion.Length)
        {
            frameActual = 0;
        }

        // Si estamos dentro del rango de frames, mostramos el frame actual
        if (frameActual >= 0 && frameActual < framesAnimacion.Length)
        {
            spriteRenderer.sprite = framesAnimacion[frameActual];
        }
        // Si no, y si debemos usar un frame estático, lo mostramos
        else if (usarFrameEstatico && frameEstatico != null) // Añadida verificación de null para frameEstatico
        {
            spriteRenderer.sprite = frameEstatico;
        }
    }

    // Método para cambiar el color del sprite
    public void CambiarColor(Color nuevoColor)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = nuevoColor;
        }
    }
}