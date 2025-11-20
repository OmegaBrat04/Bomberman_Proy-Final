using UnityEngine;

public class Recoger_Items : MonoBehaviour
{
    public enum TipoItem
    {
        // --- BÁSICOS ---
        BombaExtra,
        RadioExplosion,
        VelocidadMovimiento,
        
        // --- TUS PODERES ---
        VirusPropio,        // Te afecta a ti
        VirusAtaqueEnemigos, // Afecta a los rivales
        Escudo,             // Te protege
        Intercambio,        // Swap de posición

        // --- PODERES DE TU COMPAÑERO ---
        Tortuga,            // Te hace lento
        MechaCorta          // Bombas explotan rápido
    }

    public TipoItem tipoItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RecogerItem(collision.gameObject);
        }
    }
    
    private void RecogerItem(GameObject player)
    {
        MovimientoController movimientoController = player.GetComponent<MovimientoController>();
        Bomba_Controller bombaController = player.GetComponent<Bomba_Controller>();

        switch (tipoItem)
        {
            // --- MEJORAS BÁSICAS ---
            case TipoItem.BombaExtra:
                bombaController.AñadirBomba();
                break;
            case TipoItem.RadioExplosion:
                bombaController.explosionRadius++;
                break;
            case TipoItem.VelocidadMovimiento:
                movimientoController.velocidad += 1f;
                break;

            // --- TUS PODERES ---
            case TipoItem.VirusPropio:
                movimientoController.ActivarVirus();
                break;
            case TipoItem.VirusAtaqueEnemigos:
                GameObject[] todos = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject p in todos)
                {
                    if (p != player) p.GetComponent<MovimientoController>().ActivarVirus();
                }
                break;
            case TipoItem.Escudo:
                movimientoController.ActivarEscudo();
                break;
            case TipoItem.Intercambio:
                GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");
                foreach(GameObject enemigo in jugadores)
                {
                    if(enemigo != player && enemigo.activeSelf) 
                    {
                        Vector3 temp = player.transform.position;
                        player.transform.position = enemigo.transform.position;
                        enemigo.transform.position = temp;
                        break; 
                    }
                }
                break;

            // --- PODERES DE TU COMPAÑERO ---
            case TipoItem.Tortuga:
                movimientoController.ActivarTortuga();
                break;

            case TipoItem.MechaCorta:
                // ¡OJO! Esta función debe existir en Bomba_Controller.
                // Si te marca error rojo aquí, es porque falta el siguiente script.
                bombaController.ActivarMechaCorta(); 
                break;
        }
        Destroy(gameObject);
    }
}