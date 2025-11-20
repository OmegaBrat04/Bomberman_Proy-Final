
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public static class GameSettings {
    public static string NivelSeleccionado;
    public static int CantidadJugadores=2;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    [Header("Jugadores colocados en la escena (en orden)")]
    [SerializeField] private GameObject[] jugadoresEnEscena;

    private readonly List<GameObject> vivos = new List<GameObject>();
    private bool fin;

    private void Awake()
    {
        if (Instancia == null) Instancia = this;
        else { Destroy(gameObject); return; }
    }

     private void Start()
    {
        vivos.Clear();
        int n = Mathf.Clamp(GameSettings.CantidadJugadores > 0 ? GameSettings.CantidadJugadores : jugadoresEnEscena.Length, 1, 4);

        for (int i = 0; i < jugadoresEnEscena.Length; i++)
        {
            if (jugadoresEnEscena[i] == null) continue;
            bool activo = i < n;
            jugadoresEnEscena[i].SetActive(activo);
            if (activo) vivos.Add(jugadoresEnEscena[i]);
        }
    }

    public void UnregisterPlayer(GameObject jugador)
    {
        if (fin) return;
        vivos.Remove(jugador);
        VerificarGanador();
    }

    private void VerificarGanador()
    {
        if (vivos.Count <= 1)
        {
            fin = true;
            if (vivos.Count == 1) Debug.Log("Ganador: " + vivos[0].name);
            else Debug.Log("Empate: nadie queda vivo");
            Invoke(nameof(RegresarMenu), 3f);

        }
    }
     private void RegresarMenu() {
        SceneManager.LoadScene("Niveles");
    }
    
    private void NuevaRonda()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
