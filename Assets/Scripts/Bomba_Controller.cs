using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomba_Controller : MonoBehaviour
{
    [Header("Bomba")]
    public GameObject bombaPrefab;
    public KeyCode inputKey = KeyCode.Space;
    public float ActivacionBombas = 3f;
    public int maxBombas = 1;
    private int BombasRestantes;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask layerMuroIndestructible;
    public LayerMask layerDestruible;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Audio")]
    public AudioClip sonidoExplosion; // Arrastra tu sonido aquí en Unity

    [Header("Otros")]
    public Tilemap TilesDestruibles;
    public Destruibles destruiblesPrefab;

    // --- MECHA CORTA ---
    private float tiempoOriginal;
    private bool mechaCortaActiva = false;
    public float tiempoMechaCorta = 1f; // Explotar en 1 seg
    public float duracionPowerUp = 8f;  // El efecto dura 8 seg
    // -------------------

    private void Awake()
    {
        if (TilesDestruibles == null)
            TilesDestruibles = FindFirstObjectByType<Tilemap>();

        tiempoOriginal = ActivacionBombas; // Recordar que lo normal es 3 seg
    }

    private void OnEnable()
    {
        BombasRestantes = maxBombas;
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputKey) && BombasRestantes > 0)
        {
            StartCoroutine(ColocarBomba());
        }
    }

    private IEnumerator ColocarBomba()
    {
        Vector2 posicion = transform.position;
        // Centrar en la rejilla
        posicion.x = Mathf.Round(posicion.x - 0.5f) + 0.5f;
        posicion.y = Mathf.Round(posicion.y - 0.5f) + 0.5f;

        GameObject bomba = Instantiate(bombaPrefab, posicion, Quaternion.identity);
        BombasRestantes--;

        // Esperar tiempo de detonación (Variable según Mecha Corta)
        yield return new WaitForSeconds(ActivacionBombas);

        // SONIDO DE EXPLOSIÓN
        if (sonidoExplosion != null)
        {
            AudioSource.PlayClipAtPoint(sonidoExplosion, bomba.transform.position, 1f);
        }

        // Lógica de expansión de explosión
        posicion = bomba.transform.position;
        posicion.x = Mathf.Round(posicion.x - 0.5f) + 0.5f;
        posicion.y = Mathf.Round(posicion.y - 0.5f) + 0.5f;

        Explosion nuevaExplosion = Instantiate(explosionPrefab, posicion, Quaternion.identity);
        nuevaExplosion.IniciarExplosion(nuevaExplosion.inicioExplosion);
        nuevaExplosion.DestruirDespues(explosionDuration);

        ExploteBomba(posicion, Vector2.up, explosionRadius);
        ExploteBomba(posicion, Vector2.down, explosionRadius);
        ExploteBomba(posicion, Vector2.left, explosionRadius);
        ExploteBomba(posicion, Vector2.right, explosionRadius);

        Destroy(bomba);
        BombasRestantes++;
    }

    private void ExploteBomba(Vector2 posicion, Vector2 direccion, int alcance)
    {
        if (alcance <= 0) return;
        posicion += direccion;
        if (Physics2D.OverlapBox(posicion, Vector2.one / 2f, 0f, layerMuroIndestructible)) return;
        if (Physics2D.OverlapBox(posicion, Vector2.one / 2f, 0f, layerDestruible))
        {
            LimpiarDestruibleEnPosicion(posicion);
            return;
        }
        Explosion nuevaExplosion = Instantiate(explosionPrefab, posicion, Quaternion.identity);
        nuevaExplosion.IniciarExplosion(alcance > 1 ? nuevaExplosion.medioExplosion : nuevaExplosion.finExplosion);
        nuevaExplosion.Direccion(direccion);
        nuevaExplosion.DestruirDespues(explosionDuration);
        ExploteBomba(posicion, direccion, alcance - 1);
    }
    public void LimpiarDestruibleEnPosicion(Vector2 posicion)
    {
        if (TilesDestruibles == null) return;
        Vector3Int celda = TilesDestruibles.WorldToCell(posicion);
        TileBase tile = TilesDestruibles.GetTile(celda);
        if (tile != null)
        {
            Instantiate(destruiblesPrefab, posicion, Quaternion.identity);
            TilesDestruibles.SetTile(celda, null);
        }
    }

    public void AñadirBomba()
    {
        maxBombas++;
        BombasRestantes++;
    }

    public void ActivarMechaCorta()
    {
        if (mechaCortaActiva) { CancelInvoke(nameof(DesactivarMechaCorta)); Invoke(nameof(DesactivarMechaCorta), duracionPowerUp); return; }

        mechaCortaActiva = true;
        ActivacionBombas = tiempoMechaCorta; // Cambiar a 1 seg
        Invoke(nameof(DesactivarMechaCorta), duracionPowerUp);
    }

    private void DesactivarMechaCorta()
    {
        mechaCortaActiva = false;
        ActivacionBombas = tiempoOriginal; // Volver a 3 seg
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bomba")) other.isTrigger = false;
    }
}