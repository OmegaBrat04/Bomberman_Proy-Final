using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SeleccionJugadores : MonoBehaviour
{
    [SerializeField] private TMP_Text textoBotonJugadores;
    private string nivelObjetivo;

    private void Start()
    {
        ActualizarTexto();
    }

    public void ConfigurarNivel(string nombreNivel)
    {
        nivelObjetivo = nombreNivel;
        GameSettings.NivelSeleccionado = nombreNivel;
    }

    public void SetPlayers(int cantidad)
    {
        GameSettings.CantidadJugadores = Mathf.Clamp(cantidad, 1, 4);
        ActualizarTexto();
    }
    private void ActualizarTexto()
    {
        if (textoBotonJugadores != null)
            textoBotonJugadores.text = "Jugadores: " + GameSettings.CantidadJugadores;
    }

    public void Confirmar()
    {
        // --- AQUÍ VA EL CÓDIGO PARA DETENER LA MÚSICA DEL MENÚ ---
        // Primero, verificamos si existe una instancia de nuestro MenuMusicManager
        if (MenuMusicManager.instance != null)
        {
            // Si existe, llamamos a la función para detener la música
            MenuMusicManager.instance.StopMenuMusic();
        }
        // --------------------------------------------------------

        var nivel = string.IsNullOrEmpty(GameSettings.NivelSeleccionado) ? "Bomberman_Nivel1" : GameSettings.NivelSeleccionado;
        SceneManager.LoadScene(nivel);
    }
}