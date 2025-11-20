using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para la gestión de escenas

public class MenuMusicManager : MonoBehaviour
{

    public static MenuMusicManager instance;

    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;


        DontDestroyOnLoad(gameObject);
    }

    public void StopMenuMusic()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PlayMenuMusic()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}