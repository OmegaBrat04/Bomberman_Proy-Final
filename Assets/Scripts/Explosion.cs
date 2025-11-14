using UnityEngine;

public class Explosion : MonoBehaviour
{
    public RenderAnimacion inicioExplosion;
    public RenderAnimacion medioExplosion;
    public RenderAnimacion finExplosion;


    public void IniciarExplosion(RenderAnimacion renderAnimacion)
    {
        inicioExplosion.enabled = renderAnimacion == inicioExplosion;
        medioExplosion.enabled = renderAnimacion == medioExplosion;
        finExplosion.enabled = renderAnimacion == finExplosion;
        
    }

    public void Direccion(Vector2 direccion)
    {
        float angulo = Mathf.Atan2(direccion.y, direccion.x);
        transform.rotation = Quaternion.AngleAxis(angulo * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestruirDespues(float delay)
    {
        Destroy(gameObject, delay);
    }

}
