using UnityEngine;

public class Destruibles : MonoBehaviour
{
    public float tiempoDestruccion = 1f;

    [Range(0f, 1f)]
    public float PorcentajeItem = 0.3f;

    public GameObject[] itemsPrefab;
    private void Start()
    {
        Destroy(gameObject, tiempoDestruccion);
    }

    private void OnDestroy()
    {
        float probabilidad = Random.Range(0f, 1f);
        if (itemsPrefab.Length > 0 && probabilidad <= PorcentajeItem)
        {
            int indiceItem = Random.Range(0, itemsPrefab.Length);
            Instantiate(itemsPrefab[indiceItem], transform.position, Quaternion.identity);
        }
    }
}
