using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPoint : MonoBehaviour
{
    [Header("UI del menú de victoria")]
    [SerializeField] private GameObject winPanel;        // ← arrastra aquí tu panel “Win”

    [Header("Escena a cargar")]
    [SerializeField] private string nextLevelName = "Level 2";   // pon el nombre exacto de la próxima escena
    private string home = "MenuPrincipal";

    [SerializeField] private GameObject mensajeDiamantes;
    [SerializeField] private float duracionMensaje = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Verifica si ya se recolectaron todos los diamantes
        if (GameManager.instance.DiamondCollected >= GameManager.instance.TotalDiamonds)
        {
            // Mostrar panel de victoria
            if (winPanel != null)
                winPanel.SetActive(true);

            // Pausar el juego
            Time.timeScale = 0f;
        }
        else
        {
            
            if (mensajeDiamantes != null)
                StartCoroutine(MostrarMensajeTemporal()); // mostrar mensaje
        }
    }

    private IEnumerator MostrarMensajeTemporal() // corrrutina mensaje temporal
    {
        mensajeDiamantes.SetActive(true);
        yield return new WaitForSeconds(duracionMensaje);
        mensajeDiamantes.SetActive(false);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;                       // quitar la pausa
        SceneManager.LoadScene(nextLevelName);
    }

    public void BackMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(home);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Quitar la pausa por si estaba pausado
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recargar la escena actual
    }
}

