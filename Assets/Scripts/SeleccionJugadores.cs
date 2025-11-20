using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SeleccionJugadores : MonoBehaviour {
    [SerializeField] private TMP_Text textoBotonJugadores;
    private string nivelObjetivo;

    private void Start()
    {
        ActualizarTexto();
    }

    public void ConfigurarNivel(string nombreNivel) {
        nivelObjetivo = nombreNivel;
        GameSettings.NivelSeleccionado = nombreNivel;
    }

     public void SetPlayers(int cantidad)
    {
        GameSettings.CantidadJugadores = Mathf.Clamp(cantidad, 1, 4);
        ActualizarTexto();
    }
    private void ActualizarTexto() {
        if (textoBotonJugadores != null)
            textoBotonJugadores.text = "Jugadores: " + GameSettings.CantidadJugadores;
    }

    public void Confirmar()
    {
        var nivel = string.IsNullOrEmpty(GameSettings.NivelSeleccionado) ? "Bomberman_Nivel1" : GameSettings.NivelSeleccionado;
        SceneManager.LoadScene(nivel);
    }
}