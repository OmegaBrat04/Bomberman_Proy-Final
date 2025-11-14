using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] jugadores;

    public void Ganador()
    {
        int jugadoresActivos = 0;
        foreach (GameObject jugador in jugadores)
        {
            if (jugador.activeSelf)
            {
                jugadoresActivos++;
            }
        }
        if (jugadoresActivos <= 1)
        {
            Invoke(nameof(NuevaRonda), 2f);
        }
    }
    
    private void NuevaRonda()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
