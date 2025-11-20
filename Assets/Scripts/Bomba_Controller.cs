using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
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

    [Header("Otros")]
    public Tilemap TilesDestruibles;
    public Destruibles destruiblesPrefab;

    private void Awake()
    {
        if (TilesDestruibles == null)
            TilesDestruibles = FindFirstObjectByType<Tilemap>(); // Asegura referencia
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
        posicion.x = Mathf.Round(posicion.x - 0.5f) + 0.5f;
        posicion.y = Mathf.Round(posicion.y - 0.5f) + 0.5f;

        GameObject bomba = Instantiate(bombaPrefab, posicion, Quaternion.identity);
        BombasRestantes--;
        yield return new WaitForSeconds(ActivacionBombas);

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
        if (alcance <= 0)
        {
            return;
        }
        posicion += direccion;

        if (Physics2D.OverlapBox(posicion, Vector2.one / 2f, 0f, layerMuroIndestructible))
        {
            return; // detiene propagación
        }

        // Luego destructible
        if (Physics2D.OverlapBox(posicion, Vector2.one / 2f, 0f, layerDestruible))
        {
            LimpiarDestruibleEnPosicion(posicion);
            return; // detiene propagación tras destruir
        }
        Explosion nuevaExplosion = Instantiate(explosionPrefab, posicion, Quaternion.identity);
        nuevaExplosion.IniciarExplosion(alcance > 1 ? nuevaExplosion.medioExplosion : nuevaExplosion.finExplosion);
        nuevaExplosion.Direccion(direccion);
        nuevaExplosion.DestruirDespues(explosionDuration);

        ExploteBomba(posicion, direccion, alcance - 1);
    }

    public void LimpiarDestruibleEnPosicion(Vector2 posicion)
    {
        if (TilesDestruibles == null)
        {
            Debug.LogWarning("TilesDestruibles no está asignado.");
            return;
        }
        Vector3Int celda = TilesDestruibles.WorldToCell(posicion);
        TileBase tile = TilesDestruibles.GetTile(celda);
        if (tile != null)
        {
            Instantiate(destruiblesPrefab, posicion, Quaternion.identity);
            TilesDestruibles.SetTile(celda, null);
        }else
        {
            Debug.LogWarning("No se encontró un tile destructible en la posición dada: " + posicion);
        }
    }
    
    public void AñadirBomba()
    {
        maxBombas++;
        BombasRestantes++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bomba"))
        {
            other.isTrigger = false;
        }
    }
}
