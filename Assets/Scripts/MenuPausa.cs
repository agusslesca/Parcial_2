using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;

    public void Pause()
    {
        Time.timeScale = 1f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);

    }

    public void Resume()
    {
        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Back()
    {
        Time.timeScale = 1f; // por si el jeugo esta pausado
        SceneManager.LoadScene("MenuPrincipal");
    }
}
