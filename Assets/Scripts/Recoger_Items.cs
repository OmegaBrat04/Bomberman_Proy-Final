using UnityEngine;

public class Recoger_Items : MonoBehaviour
{
    // 1. Definimos TODOS los tipos de items disponibles en el juego
    public enum TipoItem
    {
        BombaExtra,
        RadioExplosion,
        VelocidadMovimiento,
        Tortuga,         // Tuyo
        MechaCorta,      // Tuyo
        Escudo,          // De Juan
        VirusPropio,     // De Juan (Te enfermas tú)
        VirusEnemigos    // De Juan (Enfermas a los demás)
    }

    public TipoItem tipoItem;

    // Candado para evitar que el item se recoja doble vez por error
    private bool itemRecogido = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (itemRecogido) return; // Si ya se usó, ignorar

        if (collision.CompareTag("Player"))
        {
            itemRecogido = true; // Cerramos el candado
            RecogerItem(collision.gameObject);
        }
    }

    private void RecogerItem(GameObject player)
    {
        MovimientoController movimiento = player.GetComponent<MovimientoController>();
        Bomba_Controller bomba = player.GetComponent<Bomba_Controller>();
        switch (tipoItem)
        {
            case TipoItem.BombaExtra:
                bomba.AñadirBomba();
                break;
            case TipoItem.RadioExplosion:
                bomba.explosionRadius++;
                break;
            case TipoItem.VelocidadMovimiento:
                movimiento.velocidad++;
                break;
            case TipoItem.Tortuga:
                movimiento.ActivarTortuga();
                break;
            case TipoItem.MechaCorta:
                bomba.ActivarMechaCorta();
                break;
            case TipoItem.Escudo:
                movimiento.ActivarEscudo();
                break;
            case TipoItem.VirusPropio:
                movimiento.ActivarMaldicion();
                break;
            case TipoItem.VirusEnemigos:
                AplicarVirusAEnemigos(player);
                break;
        }
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }

    private void AplicarVirusAEnemigos(GameObject quienLoRecogio)
    {
        // Buscamos todos los scripts de movimiento en la escena
        MovimientoController[] todosLosJugadores = FindObjectsByType<MovimientoController>(FindObjectsSortMode.None);

        foreach (MovimientoController jugador in todosLosJugadores)
        {
            // Si el jugador NO es el que recogió el item, ¡maldición pa' él!
            if (jugador.gameObject != quienLoRecogio)
            {
                jugador.ActivarMaldicion();
            }
        }
        Debug.Log("¡Virus enviado a los rivales!");
    }
}