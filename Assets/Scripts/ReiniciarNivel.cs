using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarNivel : MonoBehaviour
{
    public KeyCode teclaReiniciar = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(teclaReiniciar))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}