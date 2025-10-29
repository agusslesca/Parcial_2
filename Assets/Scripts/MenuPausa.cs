using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;

    // Función auxiliar para reproducir el sonido de clic
    private void PlayClickSound()
    {
        // El nombre "Click" debe coincidir EXACTAMENTE con el campo 'Nombre' en el Inspector
        if (SFX_Controller.Instance != null)
        {
            SFX_Controller.Instance.PlaySFX("Click");
        }
    }

    public void Pause()
    {
        // LLAMADA DE AUDIO
        PlayClickSound();

        Time.timeScale = 0f; // Importante: Se pausa con 0f, no 1f.
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
    }

    public void Resume()
    {
        // LLAMADA DE AUDIO
        PlayClickSound();

        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
    }

    public void Restart()
    {
        // LLAMADA DE AUDIO
        PlayClickSound();

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Back()
    {
        // LLAMADA DE AUDIO
        PlayClickSound();

        Time.timeScale = 1f; // por si el jeugo esta pausado
        SceneManager.LoadScene("MenuPrincipal");
    }
}