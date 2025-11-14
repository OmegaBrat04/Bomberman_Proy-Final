using UnityEngine;

public class Recoger_Items : MonoBehaviour
{
    public enum TipoItem
    {
        BombaExtra,
        RadioExplosion,
        VelocidadMovimiento,
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
            case TipoItem.BombaExtra:
                bombaController.AñadirBomba();
                break;
            case TipoItem.RadioExplosion:
                bombaController.explosionRadius++;
                break;
            case TipoItem.VelocidadMovimiento:
                movimientoController.velocidad += 1f;
                break;
        }
        Destroy(gameObject);
    }
}
